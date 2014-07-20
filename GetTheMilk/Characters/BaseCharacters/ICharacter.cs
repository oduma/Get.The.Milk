using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface ICharacter : IPositionable, IObjectHumanInterface
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

        void LoadInteractions(IActionEnabled objectInRange,string mainName);
    }
}