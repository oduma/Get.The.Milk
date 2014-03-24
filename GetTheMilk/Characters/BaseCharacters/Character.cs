using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Fight;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
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

        public virtual void PrepareForBattle()
        {
            Weapon activeAttackWeapon= new Weapon();
            Weapon activeDefenseWeapon=new Weapon();
            Interactivity.SelectWeapons(
                Inventory.Where(w => (w.ObjectCategory == ObjectCategory.Weapon)).Select(w => (Weapon) w),
                ref activeAttackWeapon, ref activeDefenseWeapon);
            ActiveAttackWeapon = activeAttackWeapon;
            ActiveDefenseWeapon = activeDefenseWeapon;
        }

        public Weapon ActiveDefenseWeapon { get; set; }

        public Weapon ActiveAttackWeapon { get; set; }

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
                                                    IncludeWallet = true,
                                                    ActiveCharacter=this,
                                                    TargetCharacter=targetCharacter
                                                };

            return new ActionResult
                       {
                           ExtraData = exposeLooserInventory.Perform().ExtraData,
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

        public Noun Name { get; set; }
        public int CellNumber { get; set; }
        public bool BlockMovement { get; set; }
        public string ObjectTypeId { get; set; }
        [JsonIgnore]
        public Func<GameAction, bool> AllowsAction { get; set; }
        [JsonIgnore]
        public Func<GameAction, IPositionable, bool> AllowsIndirectAction { get; set; }

        public IEnumerable<GameAction> DetermineAllPossibleActionsForTargetObject(NonCharacterObject targetObject)
        {
            foreach (var templateAction in ActionsFactory.GetFactory().GetActions())
            {
                if (templateAction is ObjectUseOnObjectAction)
                    foreach (var activeObject in Inventory)
                    {
                        var action = ActionsFactory.GetFactory().CreateNewActionInstance(templateAction.ActionType);

                        action.ActiveCharacter = this;
                        action.TargetObject = targetObject;
                        action.ActiveObject = activeObject;
                        if (action.CanPerform())
                            yield return action;
                    }
                else if (templateAction is GameAction)
                {
                    var action = ActionsFactory.GetFactory().CreateNewActionInstance(templateAction.ActionType);

                    action.ActiveCharacter = this;
                    action.TargetObject = targetObject;
                    if (action.CanPerform())
                        yield return action;
                }
            }
        }

    }
}
