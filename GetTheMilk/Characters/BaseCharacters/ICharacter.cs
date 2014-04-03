using System.Collections.Generic;
using GetTheMilk.Accounts;
using GetTheMilk.Actions.Fight;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface ICharacter:IPositionable
    {
        int Health { get; set; }
        int Experience { get; set; }
        int Range { get; set; }
        [JsonIgnore]
        CharacterCollection StorageContainer { get; set; }
        [JsonIgnore]
        Inventory Inventory { get; set; }
        Walet Walet { get; set; }
        [JsonIgnore]
        SortedList<string, ActionReaction[]> InteractionRules { get; set; }
        Hit PrepareDefenseHit();
        Hit PrepareAttackHit();
        Weapon ActiveAttackWeapon { get; set; }
        Weapon ActiveDefenseWeapon { get; set; }
        CharacterSavedPackages Save();

    }
}