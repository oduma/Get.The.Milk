using System;
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

namespace GetTheMilk.Characters.BaseCharacters
{
    public abstract class Character : ICharacter
    {
        private Inventory _rightHandObject;
        private Inventory _leftHandObject;
        private Inventory _toolInventory;
        private Inventory _weaponInventory;
        private Walet _walet;
        private IInteractivity _interactivity;

        protected Character()
        {
            Health = GameSettings.FullDefaultHealth;
        }

        public IInteractivity Interactivity { get
        {
            return _interactivity = (_interactivity) ??
                             (new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>(
                                 "No");
        }
            protected internal set { _interactivity = value; }
        }

        public int Health { get; set; }

        public int Experience { get { return _experience = (_experience == 0) ? 1 : _experience; } set { _experience = value; } }

        public Inventory ToolInventory
        {
            get { return _toolInventory = (_toolInventory) ?? new Inventory {Owner = this,InventoryType=InventoryType.CharacterInventory,MaximumCapacity=20}; }
        }

        public Inventory WeaponInventory
        {
            get { return _weaponInventory = (_weaponInventory) ?? new Inventory {Owner = this,InventoryType=InventoryType.CharacterInventory,MaximumCapacity=20}; }
        }

        public Walet Walet { get { return _walet = (_walet) ?? new Walet{Owner=this}; } }

        private Personality _personality;
        private int _experience;

        public virtual Personality Personality
        {
            get
            {
                if (_personality == null)
                {
                    _personality = new Personality();
                    _personality.InteractionRules.Add(GenericInteractionRulesKeys.All,
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
                return _personality;
            }
        }

        public virtual int Range { get { return 1; } }

        public Inventory RightHandObject
        {
            get { return _rightHandObject = (_rightHandObject) ?? new Inventory {MaximumCapacity = 1,Owner=this,InventoryType=InventoryType.InHand}; }
        }

        public Inventory LeftHandObject 
        {
            get { return _leftHandObject = (_leftHandObject) ?? new Inventory { MaximumCapacity = 1, Owner = this, InventoryType=InventoryType.InHand}; }
        }

        public ActionResult TryPerformAction(GameAction action, params IPositionableObject[] targetObjects)
        {
            var actionResult = new ActionResult{ResultType=ActionResultType.Ok,ForAction=action};
            foreach(var targetObject in targetObjects)
            {
                if(targetObject.AllowsIndirectAction(action,this))
                {
                    ((OneObjectAction)action).Perform(this,targetObject);
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
            var result = new ActionResult {ResultType = ActionResultType.NotOk};
            if(action.UseableObject.AllowsIndirectAction(action, targetCharacter) 
                && targetCharacter.AllowsIndirectAction(action, action.UseableObject)
                && Pay(action,targetCharacter,Walet.CanPerformTransaction)
                && action.Perform(this,targetCharacter))
                if(Pay(action,targetCharacter,Walet.PerformTransaction))
                    result.ResultType = ActionResultType.Ok;

            return result;
        }

        private bool Pay(ObjectTransferAction a, ICharacter c,Func<TransactionDetails,bool> currencyTransfer)
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
            ActionResult result = new ActionResult {ResultType = ActionResultType.Ok};
            action.Perform(targetCharacter,this);
            return result;
        }

        public ActionResult TryPerformObjectOnObjectAction(ObjectUseOnObjectAction action, ref IPositionableObject passiveTargetObject)
        {
            var activeTargetObject = ChooseTool();
            var actionResult = new ActionResult
                                   {
                                       ResultType = ActionResultType.NotOk,
                                       ExtraData = 
                                           string.Format("Cannot {0} the {1} using the {2}", action.Name,
                                                         passiveTargetObject.Name, activeTargetObject.Name)
                                   };
            if(activeTargetObject.AllowsAction(action) && passiveTargetObject.AllowsIndirectAction(action, activeTargetObject))
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
                                       ? ((ITransactionalObject) actionToPerform.UseableObject).BuyPrice
                                       : ((ITransactionalObject) actionToPerform.UseableObject).SellPrice,
                           Towards = targetCharacter,
                           TransactionType = actionToPerform.TransactionType
                       };
        }

        public bool TryAnySuitableInventories(IPositionableObject targetObject)
        {
            if(!RightHandObject.Objects.Any())
            {
                RightHandObject.Add(targetObject);
                return true;
            }
            if (!LeftHandObject.Objects.Any())
            {
                LeftHandObject.Add(targetObject);
                return true;
            }
            if (targetObject is Tool)
                return ToolInventory.Add(targetObject as Tool);
            if (targetObject is Weapon)
                return WeaponInventory.Add(targetObject as Weapon);
            return false;
        }

        public ActionResult TryPerformMove(MovementAction movement, Map currentMap,IEnumerable<IPositionableObject> allLevelObjects,IEnumerable<IPositionableObject> allLevelCharacters)
        {

            ActionResult movementResult;
            if(movement.TargetCell!=0 && movement.Direction==Direction.None)
            {
                //teleport directly
                movementResult = currentMap.CalculateDirectMovement(currentMap.Number, movement.TargetCell,
                                                                    Direction.None,
                                                                    allLevelObjects ?? new IPositionableObject[0],
                                                                    allLevelCharacters ?? new IPositionableObject[0]);
            }
            else
            {
                movementResult = currentMap.CalculateMovement(this, movement.DefaultDistance, movement.Direction,
                 allLevelObjects ?? new IPositionableObject[0],
                         allLevelCharacters ?? new IPositionableObject[0]);
            }

            movementResult.ForAction = movement;

            if(movementResult.ResultType==ActionResultType.LevelCompleted)
            {
                if(movement.Perform(this))
                {
                    Game.CreateGameInstance().ProceedToNextLevel();
                }
            }
            if(movementResult.ResultType!=ActionResultType.OriginNotOnTheMap)
            {
                movement.TargetCell = ((MovementActionExtraData)movementResult.ExtraData).MoveToCell;
                if(!movement.Perform(this))
                {
                    movementResult.ResultType=ActionResultType.UnknownError;
                }
            }
            movementResult.ForAction = movement;
            return movementResult;
        }

        public virtual void PrepareForBattle()
        {
            //free both hands if not free
            if(RightHandObject.Objects.Any())
                TryAnySuitableInventories(RightHandObject.Objects[0]);
            if(LeftHandObject.Objects.Any())
                TryAnySuitableInventories(LeftHandObject.Objects[0]);

            Interactivity.SelectWeapons(WeaponInventory.Objects.Where(w => ((Weapon)w).WeaponTypes.Contains(WeaponType.Attack)).
                        Select(w => (Weapon)w).ToList(),RightHandObject.Objects,WeaponInventory.Objects.Where(w => ((Weapon) w).WeaponTypes.Contains(WeaponType.Deffense)).
                        Select(w => (Weapon) w).ToList(),LeftHandObject.Objects);
        }

        public ActionResult TryPerformAction(FightAction action, ICharacter targetCharacter)
        {
            var result = new ActionResult {ResultType = ActionResultType.NotOk};
            if (AllowsAction(action) && targetCharacter.AllowsIndirectAction(action, this))
            {
                if (action is Attack)
                {
                    ((Attack) action).Hit = PrepareAttackHit();
                    return ((Attack) action).Perform(this, targetCharacter);
                }
                if (action is Quit)
                {
                    return ((Quit) action).Perform(this, targetCharacter);
                }
            }
            return result;
        }
    
        public IPositionableObject ChooseTool()
        {

            return Interactivity.SelectAnObject(new[] {ToolInventory.Objects, RightHandObject.Objects, LeftHandObject.Objects}.SelectMany(
                c => c.Where(o => o is Tool), (c, o) => o).ToArray())[0];
        }


        public GameAction ChooseFromAnotherInventory(ExposeInventoryExtraData extraData)
        {
            return Interactivity.SelectAnActionAndAnObject(extraData);
        }

        public ActionResult StartInteraction(GameAction startingAction,ICharacter targetCharacter)
        {
            var interactionSetup = new InteractionSetup {Active = this, Passive = targetCharacter};
            if(startingAction is Attack)
            {
                PrepareForBattle();
                targetCharacter.PrepareForBattle();
            }
            return interactionSetup.Start(startingAction);

        }

        public ActionResult TryPerformAction(TwoCharactersAction action, ICharacter targetCharacter)
        {
            var result = new ActionResult {ResultType = ActionResultType.NotOk};
            if(AllowsAction(action)
                &&targetCharacter.AllowsIndirectAction(action,this))
            {
                if(action.Perform(this, targetCharacter))
                    result.ResultType = ActionResultType.Ok;
            }
            return result;
        }

        public ActionResult TryPerformAction(CommunicateAction action, ICharacter targetCharacter)
        {
            var result = new ActionResult {ExtraData = string.Empty, ResultType = ActionResultType.NotOk};
            if((this.AllowsAction(action)) && (targetCharacter.AllowsIndirectAction(action,this)))
            {
                result.ExtraData = action.Message;
                result.ResultType = ActionResultType.Ok;
            }
            return result;
        }

        public GameAction TryContinueInteraction(GameAction incomingAction, ICharacter targetCharacter)
        {
            Func<ActionReaction, bool> selector= null;
            if(incomingAction is CommunicateAction)
            {
                selector = delegate(ActionReaction r)
                               {
                                   if (!(r.Action is CommunicateAction))
                                       return false;
                                   return (((CommunicateAction) r.Action).Message ==
                                           ((CommunicateAction) incomingAction).Message)
                                          && targetCharacter.AllowsIndirectAction(r.Reaction, this)
                                          && AllowsAction(r.Reaction);
                               };
            }
            else if(incomingAction is Attack)
            {
                selector = delegate(ActionReaction r)
                               {
                                   if(!(r.Action is Attack))
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
            else if(incomingAction is TwoCharactersAction)
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

        private GameAction SelectAppropriateAction(ICharacter targetCharacter,Func<ActionReaction, bool> selector)
        {
            var options =
                Personality.InteractionRules.GetAllAplicableInteractionRules(targetCharacter.Name.Main).Where(selector).
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
            if (!LeftHandObject.Objects.Any(o => o is Weapon) || ((Weapon)LeftHandObject.Objects[0]).Durability==0)
                return null;
            return new Hit
                       {
                           Power =
                               CalculationStrategies.CalculateDefensePower(
                                   ((Weapon) LeftHandObject.Objects[0]).DefensePower, Experience),
                           WithWeapon = (Weapon) LeftHandObject.Objects[0]
                       };
        }

        public Hit PrepareAttackHit()
        {
            if (!RightHandObject.Objects.Any(o => o is Weapon) || ((Weapon)RightHandObject.Objects[0]).Durability == 0)
                return null;

            return new Hit
            {
                WithWeapon = (Weapon)RightHandObject.Objects[0],
                Power = CalculationStrategies.CalculateAttackPower(
                    ((Weapon)RightHandObject.Objects[0]).AttackPower, Experience)
            };
        }
        
        public ActionResult PileageCharacter(ICharacter targetCharacter, ActionResultType actionResultType)
        {
            double experienceTaken=0;
            if(actionResultType==ActionResultType.Win)
            {
                experienceTaken = 0.5;
            }
            else if(actionResultType==ActionResultType.QuitAccepted)
            {
                experienceTaken = 0.25;
            }
            var exposeLooserInventory = new ExposeInventory
                                                {
                                                    AllowedNextActions =
                                                        new GameAction[]
                                                            {new TakeFrom(), new TakeMoneyFrom(), new Kill{ExperienceTaken=experienceTaken}},
                                                    Amount =
                                                        ((targetCharacter.Walet.CurrentCapacity + Walet.CurrentCapacity) >
                                                         Walet.MaxCapacity)
                                                            ? (Walet.MaxCapacity - Walet.CurrentCapacity)
                                                            : targetCharacter.Walet.CurrentCapacity,
                                                    IncludeWallet=true
                                                };
                return new ActionResult
                           {
                               ExtraData = targetCharacter.TryPerformAction(exposeLooserInventory, this).ExtraData,
                               ResultType = actionResultType
                           };
        }

        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public virtual Noun Name { get; protected set; }
        public bool BlockMovement { get; protected set; }
        public virtual bool AllowsAction(GameAction a)
        {
            return true;
        }
        public virtual bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return true;
        }
        public Inventory StorageContainer { get; set; }

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
            if(action is TakeMoneyFrom)
            {
                return TryPerformAction((TakeMoneyFrom) action, passive);
            }
            return new ActionResult { ResultType = ActionResultType.NotOk };

        }

    }
}
