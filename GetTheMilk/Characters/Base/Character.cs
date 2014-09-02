using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using GetTheMilk.Utils;
using Newtonsoft.Json;

namespace GetTheMilk.Characters.Base
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
            if (character.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
            {
                foreach (var interaction in character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses])
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

        public int Health
        {
            get;
            set;
        }

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

        public virtual void LoadInteractions(IActionEnabled objectInRange, Type typeOfObject)
        {
            var mainName = ((IPositionable)objectInRange).Name.Main;
            var mainNameResponses = mainName + "_Responses";

            if (objectInRange.Interactions!=null
                && objectInRange.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
            {
                if(Interactions.ContainsKey(mainNameResponses))
                    Interactions.Remove(mainNameResponses);
                Interactions.Add(mainNameResponses,objectInRange.Interactions[
                                                          GenericInteractionRulesKeys.AnyCharacterResponses]);
                foreach(var ar in Interactions[mainNameResponses])
                {ar.Action.TargetCharacter = this;
                    if (typeOfObject == typeof(Character))
                        ar.Action.ActiveCharacter = (Character)objectInRange;
                    else
                        ar.Action.ActiveObject = (NonCharacterObject)objectInRange;
                    if (ar.Reaction != null)
                    {
                        ar.Reaction.ActiveCharacter = this;
                        if (typeOfObject == typeof(Character))
                            ar.Reaction.TargetCharacter = (Character)objectInRange;
                        else
                            ar.Reaction.TargetObject = (NonCharacterObject)objectInRange;                        
                    }
                };
            }
            if (!Interactions.ContainsKey(mainName)
                && objectInRange.Interactions != null
                && objectInRange.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacter))
            {
                Interactions.Add(mainName,objectInRange.Interactions[
                                                          GenericInteractionRulesKeys.AnyCharacter]);
                foreach(var ar in Interactions[mainName])
                {
                    ar.Action.ActiveCharacter = this;
                    if (typeOfObject == typeof(Character))
                        ar.Action.TargetCharacter = (Character)objectInRange;
                    else
                        ar.Action.TargetObject = (NonCharacterObject)objectInRange;
                    if (ar.Reaction != null)
                    {
                        ar.Reaction.TargetCharacter = this;
                        if (typeOfObject == typeof(Character))
                            ar.Reaction.ActiveCharacter = (Character)objectInRange;
                        else
                            ar.Reaction.ActiveObject = (NonCharacterObject)objectInRange;

                    }
                }
            }
        }
    }
}
