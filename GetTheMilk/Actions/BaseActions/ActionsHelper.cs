using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
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

        public static void PileageCharacter(ICharacter pileager, ICharacter pileagee)
        {
            var takeFrom = new TakeFrom();
            var pileageeInventory = pileagee.Inventory.ToList();
            foreach (var o in pileageeInventory)
            {
                if (pileager.Inventory.MaximumCapacity >= pileager.Inventory.Count)
                {
                    takeFrom.ActiveCharacter = pileager;
                    takeFrom.TargetCharacter = pileagee;
                    if(o.ObjectCategory==ObjectCategory.Weapon)
                    {
                        if (((Weapon) o).IsCurrentAttack)
                            pileagee.ActiveAttackWeapon = null;
                        if (((Weapon)o).IsCurrentDefense)
                            pileagee.ActiveDefenseWeapon = null;
                    }
                        
                    takeFrom.TargetObject = o;
                    takeFrom.Perform();
                }
                else
                {
                    break;
                }
            }
            var takeMoneyFrom = new TakeMoneyFrom();
            takeMoneyFrom.ActiveCharacter = pileager;
            takeMoneyFrom.TargetCharacter = pileagee;
            takeMoneyFrom.Amount = (pileager.Walet.MaxCapacity - pileager.Walet.CurrentCapacity >
                                    pileagee.Walet.CurrentCapacity)
                ? pileagee.Walet.CurrentCapacity
                : (pileager.Walet.MaxCapacity - pileager.Walet.CurrentCapacity);
            takeMoneyFrom.Perform();
        }
    }
}
