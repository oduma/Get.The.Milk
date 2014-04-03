using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
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
            character.Inventory = Inventory.Load(JsonConvert.DeserializeObject<InventoryPackages>(characterPackages.CharacterInventory,
                                                                           new NonChracterObjectConverter()));
            character.Inventory.LinkObjectsToInventory();
            character.InteractionRules =
                JsonConvert.DeserializeObject<SortedList<string, ActionReaction[]>>(
                    characterPackages.CharacterInteractionRules, new ActionJsonConverter());
            if (character.InteractionRules.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
            {
                character.InteractionRules[GenericInteractionRulesKeys.CharacterSpecific].ForEach(ar =>
                {
                    ar.Reaction
                        .ActiveCharacter =
                        character;
                    ar.Action
                        .TargetCharacter =
                        character;
                });
            }
            if (character.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
            {
                character.InteractionRules[GenericInteractionRulesKeys.PlayerResponses].ForEach(ar =>
                {
                    ar.Action
                        .ActiveCharacter =
                        character;
                    ar.Reaction
                        .TargetCharacter =
                        character;
                });
                
            }
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

        public ActionResult StartInteraction(GameAction startingAction)
        {
            var interactionSetup = new InteractionSetup { Active = this, Passive = startingAction.TargetCharacter };
            if (startingAction is Attack)
            {
                PrepareForBattle();
                startingAction.TargetCharacter.PrepareForBattle();
            }
            return interactionSetup.Start(startingAction);

        }

        public GameAction TryContinueInteraction(GameAction incomingAction, ICharacter targetCharacter)
        {
            Func<ActionReaction, bool> selector = null;
            if (incomingAction is Communicate)
            {
                selector = delegate(ActionReaction r)
                               {
                                   if (!(r.Action is Communicate))
                                       return false;
                                   return (((Communicate)r.Action).Message ==
                                           ((Communicate)incomingAction).Message)
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
                                                    AllowedNextActionTypes =
                                                        new InventorySubActionType[] { new InventorySubActionType{ActionType = ActionType.TakeFrom},  new InventorySubActionType{ActionType=ActionType.TakeMoneyFrom}, new InventorySubActionType{ActionType=ActionType.Kill,FinishInventoryExposure=true} },
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
                           CharacterInventory = JsonConvert.SerializeObject(Inventory.Save())
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


    }
}
