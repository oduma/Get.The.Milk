using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using GetTheMilk.Utils;
using Newtonsoft.Json;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.Characters.BaseCharacters
{
    public class Character : ActionEnabledCharacter,ICharacter
    {
        private Inventory _inventory;
        private Walet _walet;
        private Weapon _noWeapon;


        public Character()
        {
            foreach(var action in GameSettings.GetInstance().AllCharactersActions)
                AddAvailableAction(action);
            Interactions=new SortedList<string, Interaction[]>();
            LoadInteractionsForAll();
            ActionsForExposedContents.Add(ContentActionsKeys.NPCFriendly, GameSettings.GetInstance().FriendlyContentActions);
            ActionsForExposedContents.Add(ContentActionsKeys.NPCFoe, GameSettings.GetInstance().FoeContentActions);
        }

        public static T Load<T>(ContainerWithActionsPackage  characterPackages) where T:Character
        {
            var character= JsonConvert.DeserializeObject<T>(characterPackages.Core, new CharacterJsonConverter());
            
            var actionTemplates=
                JsonConvert.DeserializeObject<List<BaseActionTemplate>>(characterPackages.ActionTemplates,
                                                                        new ActionTemplateJsonConverter());
            foreach(var actionTemplate in actionTemplates)
            {
                character.AddAvailableAction(actionTemplate);
            }
            character.Interactions = JsonConvert.DeserializeObject<SortedList<string, Interaction[]>>(
                characterPackages.Interactions, new ActionTemplateJsonConverter());
            character.Inventory = Inventory.Load(JsonConvert.DeserializeObject<CollectionPackage>(characterPackages.PackagedInventory));
            character.Inventory.LinkObjectsToInventory();
            if (character.Interactions.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
            {
                foreach (var interaction in character.Interactions[GenericInteractionRulesKeys.CharacterSpecific])
                {
                    interaction.Reaction
                        .ActiveCharacter =
                        character;
                    interaction.Action
                        .TargetCharacter =
                        character;
                }
            }
            if (character.Interactions.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
            {
                foreach (var interaction in character.Interactions[GenericInteractionRulesKeys.PlayerResponses])
                {
                    interaction.Action
                        .ActiveCharacter =
                        character;
                    interaction.Reaction
                        .TargetCharacter =
                        character;
                }

            }
            return character;
        }
        public CharacterCollection StorageContainer { get; set; }

        public int Health { get; set; }

        public int Experience { get; set; }

        public Inventory Inventory
        {
            get { return _inventory=(_inventory)??new Inventory{InventoryType=InventoryType.CharacterInventory}; }
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

        [JsonIgnore]
        public Func<BaseActionTemplate, IPositionable, bool> AllowsIndirectTemplateAction { get; set; }

        public int Range { get; set; }

        public Weapon ActiveDefenseWeapon { get; set; }

        public Weapon ActiveAttackWeapon { get; set; }

        protected internal Weapon NoWeapon{get
        {
            return _noWeapon = (_noWeapon) ?? new Weapon
            {
                Name = new Noun { Main = "Fist", Narrator = "bare fist" },
                AllowsIndirectTemplateAction =
                    new WeaponActions().AllowsIndirectTemplateAction,
                AllowsTemplateAction = new WeaponActions().AllowsTemplateAction,
                AttackPower = GameSettings.GetInstance().MinimumAttackPower,
                DefensePower = GameSettings.GetInstance().MinimumDefensePower,
                Durability = GameSettings.GetInstance().MaximumDurability
            };
 
        }
        }
        public Hit PrepareDefenseHit()
        {
            return new Hit
            {
                WithWeapon = (ActiveDefenseWeapon == null || ActiveDefenseWeapon.Durability == 0) ? NoWeapon : ActiveDefenseWeapon,
                Power = CalculationStrategies.CalculateAttackPower(
                    (ActiveDefenseWeapon == null || ActiveDefenseWeapon.Durability == 0) ? NoWeapon.DefensePower : ActiveDefenseWeapon.DefensePower, Experience)
            };
        }

        public Hit PrepareAttackHit()
        {
            return new Hit
            {
                WithWeapon = (ActiveAttackWeapon==null || ActiveAttackWeapon.Durability == 0)?NoWeapon:ActiveAttackWeapon,
                Power = CalculationStrategies.CalculateAttackPower(
                    (ActiveAttackWeapon==null || ActiveAttackWeapon.Durability == 0)?NoWeapon.AttackPower:ActiveAttackWeapon.AttackPower, Experience)
            };
        }

        public ContainerWithActionsPackage Save()
        {
            return new ContainerWithActionsPackage
                       {
                           Core = JsonConvert.SerializeObject(this),
                           PackagedInventory = JsonConvert.SerializeObject(Inventory.Save()),
                           ActionTemplates=JsonConvert.SerializeObject(AllActionsExcludeInteractions().Where(IsNonStandardActionTemplate)),
                           Interactions = JsonConvert.SerializeObject(Interactions)
                       };
        }


        protected virtual bool IsNonStandardActionTemplate(BaseActionTemplate baseActionTemplate)
        {
            return
                !(GameSettings.GetInstance().AllCharactersActions.Any(
                    a =>
                    a.Name.UniqueId==baseActionTemplate.Name.UniqueId));
        }


        public Noun Name { get; set; }
        public int CellNumber { get; set; }
        public bool BlockMovement { get; set; }
        public string ObjectTypeId { get; set; }

        [JsonIgnore]
        public Func<BaseActionTemplate, bool> AllowsTemplateAction { get; set; }

        public string CloseUpMessage { get; set; }

        public virtual void LoadInteractions(IActionEnabled objectInRange, string mainName)
        {
            if (objectInRange.Interactions!=null
                && objectInRange.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
            {
                if(Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
                    Interactions.Remove(GenericInteractionRulesKeys.AnyCharacterResponses);
                Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses,objectInRange.Interactions[
                                                          GenericInteractionRulesKeys.AnyCharacterResponses]);
                Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].ForEach(ar =>
                {
                    ar.Action.TargetCharacter = this;
                    ar.Reaction.ActiveCharacter = this;
                });
            }
            if (!Interactions.ContainsKey(mainName)
                && objectInRange.Interactions != null
                && objectInRange.Interactions.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
            {
                Interactions.Add(mainName, objectInRange.Interactions[
                                                          GenericInteractionRulesKeys.CharacterSpecific]);
                Interactions[mainName].ForEach(ar =>
                {
                    ar.Action.TargetCharacter = this;
                    ar.Reaction.ActiveCharacter = this;
                });
            }
            if (!Interactions.ContainsKey(mainName)
                && objectInRange.Interactions != null
                && objectInRange.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacter))
            {
                Interactions.Add(mainName,objectInRange.Interactions[
                                                          GenericInteractionRulesKeys.AnyCharacter]);
                Interactions[mainName].ForEach(ar =>
                {
                    AddAvailableAction(ar.Action);
                    ar.Action.ActiveCharacter = this;
                    if(ar.Reaction!=null)
                        ar.Reaction.ActiveCharacter = this;
                });
            }
        }
    }
}
