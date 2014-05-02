using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public static class ActionsHelper
    {
        public static void SelectWeapon(ICharacter character, Weapon weapon, WeaponType weaponType)
        {
            var weapons =
                character.Inventory.Where(w => (w.ObjectCategory == ObjectCategory.Weapon))
                    .Select(w => (Weapon)w).ToArray();
            if (weaponType == WeaponType.Attack)
            {
                weapons.ForEach(w => { w.IsCurrentAttack = false; });
                character.ActiveAttackWeapon = weapon;
                if (character.ActiveAttackWeapon != null)
                    character.ActiveAttackWeapon.IsCurrentAttack = true;
            }
            if (weaponType == WeaponType.Deffense)
            {
                weapons.ForEach(w => { w.IsCurrentDefense = false; });
                character.ActiveDefenseWeapon = weapon;
                if (character.ActiveDefenseWeapon != null)
                    character.ActiveDefenseWeapon.IsCurrentDefense = true;
            }


        }

    }
}
