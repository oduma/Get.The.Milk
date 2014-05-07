using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Fight;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
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

        public int Health { get; set; }

        public int Experience { get; set; }

        public Inventory Inventory
        {
            get { return _inventory=(_inventory)??new Inventory(); }
            set
            {
                _inventory = value;
                if(_inventory!=null)
                    _inventory.Owner = this;
            }
        }

        public Walet Walet
        {
            get { return _walet=(_walet)??new Walet(); }
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

        public Weapon ActiveDefenseWeapon { get; set; }

        public Weapon ActiveAttackWeapon { get; set; }

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

        public string CloseUpMessage { get; set; }
    }
}
