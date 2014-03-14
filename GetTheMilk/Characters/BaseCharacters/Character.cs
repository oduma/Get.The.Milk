using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using GetTheMilk.UI;
using GetTheMilk.Utils;
using Newtonsoft.Json;

namespace GetTheMilk.Characters.BaseCharacters
{
    public class Character : ICharacter
    {
        private SortedList<string, ActionReaction[]> _interactionRules;
        private Inventory _inventory;
        private Walet _walet;

        public Character()
        {
            Health = GameSettings.GetInstance().FullDefaultHealth;
            Interactivity = (new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>(
                "No");
            InteractionRules=new SortedList<string, ActionReaction[]>();
        }

        public static T Load<T>(CharacterSavedPackages  characterPackages) where T:Character
        {
            var character= JsonConvert.DeserializeObject<T>(characterPackages.CharacterCore, new CharacterJsonConverter());
            character.Inventory = JsonConvert.DeserializeObject<Inventory>(characterPackages.CharacterInventory,
                                                                           new NonChracterObjectConverter());
            character.Inventory.LinkObjectsToInventory();
            character.InteractionRules =
                JsonConvert.DeserializeObject<SortedList<string, ActionReaction[]>>(
                    characterPackages.CharacterInteractionRules, new ActionJsonConverter());
            return character;
        }
        public CharacterCollection StorageContainer { get; set; }

        [JsonIgnore]
        public IInteractivity Interactivity { get; protected set; }

        public int Health { get; set; }

        public int Experience { get; set; }

        public Inventory Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                if(_inventory!=null)
                    _inventory.Owner = this;
            }
        }

        public Walet Walet
        {
            get { return _walet; }
            set
            {
                _walet = value;
                _walet.Owner = this;
            }
        }

        public SortedList<string, ActionReaction[]> InteractionRules
        {
            get { return _interactionRules; }
            set
            {
                _interactionRules = value;
                if(!_interactionRules.ContainsKey(GenericInteractionRulesKeys.All))
                {
                    _interactionRules.Add(GenericInteractionRulesKeys.All,
                                          new ActionReaction[]
                                              {
                                                  new ActionReaction
                                                      {
                                                          Action = new Attack(),
                                                          Reaction = new Attack()
                                                      },
                                                  new ActionReaction
                                                      {
                                                          Action = new Attack(),
                                                          Reaction = new Quit()
                                                      }
                                              });
                }
            }
        }
        public int Range { get; set; }

        public ActionResult TryPerformAction(GameAction action, params NonCharacterObject[] targetObjects)
        {
            var actionResult = new ActionResult { ResultType = ActionResultType.Ok, ForAction = action };
            foreach (var targetObject in targetObjects)
            {
                if (targetObject.AllowsIndirectAction(action, this))
                {
                    ((OneObjectAction)action).Perform(this, targetObject);
                }
                else
                {
                    actionResult.ResultType = ActionResultType.NotOk;
                }
            }
            return actionResult;
        }

        public ActionResult TryPerformAction(ObjectTransferAction action, ICharacter targetCharacter)
        {
            var result = new ActionResult { ResultType = ActionResultType.NotOk };
            if (action.UseableObject.AllowsIndirectAction(action, targetCharacter)
                && targetCharacter.AllowsIndirectAction(action, action.UseableObject)
                && Pay(action, targetCharacter, Walet.CanPerformTransaction)
                && action.Perform(this, targetCharacter))
                if (Pay(action, targetCharacter, Walet.PerformTransaction))
                    result.ResultType = ActionResultType.Ok;

            return result;
        }

        private bool Pay(ObjectTransferAction a, ICharacter c, Func<TransactionDetails, bool> currencyTransfer)
        {
            var fundsOk = true;
            if ((a.UseableObject is ITransactionalObject) && (a.TransactionType != TransactionType.None))
                fundsOk = currencyTransfer(GetTransactionDetails(a, c));
            return fundsOk;
        }

        public ActionResult TryPerformAction(ExposeInventory action, ICharacter targetCharacter)
        {
            var result = new ActionResult { ResultType = ActionResultType.NotOk };
            if (AllowsAction(action) && targetCharacter.AllowsIndirectAction(action, this))
            {
                result = action.Perform(this);
            }
            return result;
        }

        public ActionResult TryPerformAction(TakeMoneyFrom action, ICharacter targetCharacter)
        {
            ActionResult result = new ActionResult { ResultType = ActionResultType.Ok };
            action.Perform(targetCharacter, this);
            return result;
        }

        public ActionResult TryPerformObjectOnObjectAction(ObjectUseOnObjectAction action, ref NonCharacterObject passiveTargetObject)
        {
            var activeTargetObject = ChooseTool();
            var actionResult = new ActionResult
                                   {
                                       ResultType = ActionResultType.NotOk,
                                       ExtraData =
                                           string.Format("Cannot {0} the {1} using the {2}", action.Name,
                                                         passiveTargetObject.Name, activeTargetObject.Name)
                                   };
            if (activeTargetObject.AllowsAction(action) && passiveTargetObject.AllowsIndirectAction(action, activeTargetObject))
            {
                action.Perform(ref activeTargetObject, ref passiveTargetObject);
                actionResult.ResultType = ActionResultType.Ok;
                actionResult.ExtraData = string.Empty;
            }
            return actionResult;
        }



        private TransactionDetails GetTransactionDetails(ObjectTransferAction actionToPerform, ICharacter targetCharacter)
        {
            return new TransactionDetails
                       {
                           Price = (actionToPerform.TransactionType == TransactionType.Debit)
                                       ? ((ITransactionalObject)actionToPerform.UseableObject).BuyPrice
                                       : ((ITransactionalObject)actionToPerform.UseableObject).SellPrice,
                           Towards = targetCharacter,
                           TransactionType = actionToPerform.TransactionType
                       };
        }

        public ActionResult TryPerformMove(MovementAction movement, Map currentMap, IEnumerable<NonCharacterObject> allLevelObjects, IEnumerable<Character> allLevelCharacters)
        {

            ActionResult movementResult;
            if (movement.TargetCell != 0 && movement.Direction == Direction.None)
            {
                //teleport directly
                movementResult = currentMap.CalculateDirectMovement(currentMap.Number, movement.TargetCell,
                                                                    Direction.None,
                                                                    allLevelObjects ?? new NonCharacterObject[0],
                                                                    allLevelCharacters ?? new Character[0]);
            }
            else
            {
                movementResult = currentMap.CalculateMovement(this, movement.DefaultDistance, movement.Direction,
                 allLevelObjects ?? new NonCharacterObject[0],
                         allLevelCharacters ?? new Character[0]);
            }

            movementResult.ForAction = movement;

            if (movementResult.ResultType == ActionResultType.LevelCompleted)
            {
                if (movement.Perform(this))
                {
                    Game.CreateGameInstance().ProceedToNextLevel();
                }
            }
            if (movementResult.ResultType != ActionResultType.OriginNotOnTheMap)
            {
                movement.TargetCell = ((MovementActionExtraData)movementResult.ExtraData).MoveToCell;
                if (!movement.Perform(this))
                {
                    movementResult.ResultType = ActionResultType.UnknownError;
                }
            }
            movementResult.ForAction = movement;
            return movementResult;
        }

        public virtual void PrepareForBattle()
        {
            Interactivity.SelectWeapons(Inventory.Objects.Where(w => (w.ObjectCategory==ObjectCategory.Weapon) && 
                ((Weapon)w).WeaponTypes.Contains(WeaponType.Attack)).
                        Select(w => (Weapon)w).ToList(), ActiveAttackWeapon, Inventory.Objects.Where(w => (w.ObjectCategory == ObjectCategory.Weapon) && 
                            ((Weapon)w).WeaponTypes.Contains(WeaponType.Deffense)).
                        Select(w => (Weapon)w).ToList(), ActiveDefenseWeapon);
        }

        public Weapon ActiveDefenseWeapon { get; set; }

        public Weapon ActiveAttackWeapon { get; set; }

        public ActionResult TryPerformAction(FightAction action, ICharacter targetCharacter)
        {
            var result = new ActionResult { ResultType = ActionResultType.NotOk };
            if (AllowsAction(action) && targetCharacter.AllowsIndirectAction(action, this))
            {
                if (action is Attack)
                {
                    ((Attack)action).Hit = PrepareAttackHit();
                    return ((Attack)action).Perform(this, targetCharacter);
                }
                if (action is Quit)
                {
                    return ((Quit)action).Perform(this, targetCharacter);
                }
            }
            return result;
        }

        public NonCharacterObject ChooseTool()
        {
            return Interactivity.SelectAnObject(Inventory.Objects.Where(
                o => o.ObjectCategory==ObjectCategory.Tool).ToArray())[0];
        }


        public GameAction ChooseFromAnotherInventory(ExposeInventoryExtraData extraData)
        {
            return Interactivity.SelectAnActionAndAnObject(extraData);
        }

        public ActionResult StartInteraction(GameAction startingAction, ICharacter targetCharacter)
        {
            var interactionSetup = new InteractionSetup { Active = this, Passive = targetCharacter };
            if (startingAction is Attack)
            {
                PrepareForBattle();
                targetCharacter.PrepareForBattle();
            }
            return interactionSetup.Start(startingAction);

        }

        public ActionResult TryPerformAction(TwoCharactersAction action, Character targetCharacter)
        {
            var result = new ActionResult { ResultType = ActionResultType.NotOk };
            if (AllowsAction(action)
                && targetCharacter.AllowsIndirectAction(action, this))
            {
                if (action.Perform(this, targetCharacter))
                    result.ResultType = ActionResultType.Ok;
            }
            return result;
        }

        public ActionResult TryPerformAction(CommunicateAction action, ICharacter targetCharacter)
        {
            var result = new ActionResult { ExtraData = string.Empty, ResultType = ActionResultType.NotOk };
            if ((this.AllowsAction(action)) && (targetCharacter.AllowsIndirectAction(action, this)))
            {
                result.ExtraData = action.Message;
                result.ResultType = ActionResultType.Ok;
            }
            return result;
        }

        public GameAction TryContinueInteraction(GameAction incomingAction, ICharacter targetCharacter)
        {
            Func<ActionReaction, bool> selector = null;
            if (incomingAction is CommunicateAction)
            {
                selector = delegate(ActionReaction r)
                               {
                                   if (!(r.Action is CommunicateAction))
                                       return false;
                                   return (((CommunicateAction)r.Action).Message ==
                                           ((CommunicateAction)incomingAction).Message)
                                          && targetCharacter.AllowsIndirectAction(r.Reaction, this)
                                          && AllowsAction(r.Reaction);
                               };
            }
            else if (incomingAction is Attack)
            {
                selector = delegate(ActionReaction r)
                               {
                                   if (!(r.Action is Attack))
                                       return false;
                                   return targetCharacter.AllowsIndirectAction(r.Reaction, this) &&
                                          AllowsAction(r.Reaction);
                               };
            }
            else if (incomingAction is Quit)
            {
                selector = delegate(ActionReaction r)
                {
                    if (!(r.Action is Quit))
                        return false;
                    return targetCharacter.AllowsIndirectAction(r.Reaction, this) &&
                           AllowsAction(r.Reaction);
                };
            }
            else if (incomingAction is TwoCharactersAction)
            {
                selector =
                    delegate(ActionReaction r)
                    {
                        if (!(r.Action is TwoCharactersAction))
                            return false;
                        return (r.Action.Name.Infinitive == incomingAction.Name.Infinitive) &&
                               targetCharacter.AllowsIndirectAction(r.Reaction, this)
                               && AllowsAction(r.Reaction);
                    };
            }
            return (selector == null) ? null : SelectAppropriateAction(targetCharacter, selector);
        }

        private GameAction SelectAppropriateAction(ICharacter targetCharacter, Func<ActionReaction, bool> selector)
        {
            var options =
                InteractionRules.GetAllAplicableInteractionRules(targetCharacter.Name.Main).Where(selector).
                    Select(a => a.Reaction);
            if (!options.Any())
                return null;
            return ChooseAction(options.ToArray(), targetCharacter);

        }

        public GameAction ChooseAction(GameAction[] actions, ICharacter targetCharacter)
        {
            return Interactivity.SelectAnAction(actions, targetCharacter);
        }

        public Hit PrepareDefenseHit()
        {
            if (ActiveDefenseWeapon==null || ActiveDefenseWeapon.Durability == 0)
                return null;
            return new Hit
                       {
                           Power =
                               CalculationStrategies.CalculateDefensePower(
                                   ActiveDefenseWeapon.DefensePower, Experience),
                           WithWeapon = ActiveDefenseWeapon
                       };
        }

        public Hit PrepareAttackHit()
        {
            if (ActiveAttackWeapon==null || ActiveAttackWeapon.Durability == 0)
                return null;

            return new Hit
            {
                WithWeapon = ActiveAttackWeapon,
                Power = CalculationStrategies.CalculateAttackPower(
                    ActiveAttackWeapon.AttackPower, Experience)
            };
        }

        public ActionResult PileageCharacter(ICharacter targetCharacter, ActionResultType actionResultType)
        {
            double experienceTaken = 0;
            if (actionResultType == ActionResultType.Win)
            {
                experienceTaken = 0.5;
            }
            else if (actionResultType == ActionResultType.QuitAccepted)
            {
                experienceTaken = 0.25;
            }
            var exposeLooserInventory = new ExposeInventory
                                                {
                                                    AllowedNextActions =
                                                        new GameAction[] { new TakeFrom(), new TakeMoneyFrom(), new Kill { ExperienceTaken = experienceTaken } },
                                                    Amount =
                                                        ((targetCharacter.Walet.CurrentCapacity + Walet.CurrentCapacity) >
                                                         Walet.MaxCapacity)
                                                            ? (Walet.MaxCapacity - Walet.CurrentCapacity)
                                                            : targetCharacter.Walet.CurrentCapacity,
                                                    IncludeWallet = true
                                                };
            return new ActionResult
                       {
                           ExtraData = targetCharacter.TryPerformAction(exposeLooserInventory, this).ExtraData,
                           ResultType = actionResultType
                       };
        }

        public CharacterSavedPackages Save()
        {
            return new CharacterSavedPackages
                       {
                           CharacterCore = JsonConvert.SerializeObject(this),
                           CharacterInteractionRules = JsonConvert.SerializeObject(InteractionRules),
                           CharacterInventory = JsonConvert.SerializeObject(Inventory)
                       };
        }

        public ActionResult TryPerformAction(GameAction action, ICharacter passive)
        {
            if (action is CommunicateAction)
                return TryPerformAction((CommunicateAction)action, passive);

            if (action is ObjectTransferAction)
            {
                return TryPerformAction((ObjectTransferAction)action, passive);
            }
            if (action is FightAction)
            {
                return TryPerformAction((FightAction)action, passive);
            }
            if (action is TwoCharactersAction)
            {
                return TryPerformAction((TwoCharactersAction)action, passive);
            }
            if (action is ExposeInventory)
            {
                return TryPerformAction((ExposeInventory)action, passive);
            }
            if (action is TakeMoneyFrom)
            {
                return TryPerformAction((TakeMoneyFrom)action, passive);
            }
            return new ActionResult { ResultType = ActionResultType.NotOk };

        }

        public Noun Name { get; set; }
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public bool BlockMovement { get; set; }
        public string ObjectTypeId { get; set; }
        public Func<GameAction, bool> AllowsAction { get; set; }
        public Func<GameAction, IPositionable, bool> AllowsIndirectAction { get; set; }
    }
}
