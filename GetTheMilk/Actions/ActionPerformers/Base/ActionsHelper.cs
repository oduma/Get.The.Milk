using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionPerformers.Base
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
                foreach(var w in weapons)
                    w.IsCurrentAttack = false;
                character.ActiveAttackWeapon = weapon;
                if (character.ActiveAttackWeapon != null)
                    character.ActiveAttackWeapon.IsCurrentAttack = true;
            }
            if (weaponType == WeaponType.Deffense)
            {
                foreach(var w in weapons)
                    w.IsCurrentDefense = false;
                character.ActiveDefenseWeapon = weapon;
                if (character.ActiveDefenseWeapon != null)
                    character.ActiveDefenseWeapon.IsCurrentDefense = true;
            }


        }

    }
}
