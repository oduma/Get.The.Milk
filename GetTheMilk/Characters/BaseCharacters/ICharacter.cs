using GetTheMilk.Common;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;
using System;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface ICharacter : IPositionable
    {
        int Health { get; set; }
        int Experience { get; set; }
        int Range { get; set; }
        [JsonIgnore]
        CharacterCollection StorageContainer { get; set; }
        [JsonIgnore]
        Inventory Inventory { get; set; }
        Walet Walet { get; set; }
        Hit PrepareDefenseHit();
        Hit PrepareAttackHit();
        Weapon ActiveAttackWeapon { get; set; }
        Weapon ActiveDefenseWeapon { get; set; }
        ContainerWithActionsPackage Save();

        void LoadInteractions(IActionEnabled objectInRange, Type typeOfObject);
    }
}