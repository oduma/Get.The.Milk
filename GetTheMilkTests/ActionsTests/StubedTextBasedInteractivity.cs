using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI;

namespace GetTheMilkTests.ActionsTests
{
    internal class StubedTextBasedInteractivity : IInteractivity
    {
        public NonCharacterObject[] SelectAnObject(NonCharacterObject[] availableObjects)
        {
            return availableObjects;
        }

        public ICharacter SelectACharacter(ICharacter[] availableCharacters)
        {
            throw new NotImplementedException();
        }

        public GameAction SelectAnAction(GameAction[] availableObjects, ICharacter targetCharacter)
        {
            return availableObjects[0];
        }

        public GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }

        public void SelectWeapons(IEnumerable<Weapon> weapons, ref Weapon activeAttackWeapon, ref Weapon activeDefenseWeapon)
        {
            weapons.ForEach(w => w.IsCurrentAttack = false);
            activeAttackWeapon = weapons.FirstOrDefault(w => w.WeaponTypes.Contains(WeaponType.Attack));
            if (activeAttackWeapon != null)
                activeAttackWeapon.IsCurrentAttack = true;

            weapons.ForEach(w => w.IsCurrentDefense = false);
            activeDefenseWeapon = weapons.FirstOrDefault(w => w.WeaponTypes.Contains(WeaponType.Deffense));
            if (activeDefenseWeapon != null)
                activeDefenseWeapon.IsCurrentAttack = true;

        }

        public void SelectWeapons(List<Weapon> attackWeapons, Weapon activeAttackWeapon, List<Weapon> defenseWeapons, Weapon activeDefenseWeapon)
        {
            throw new NotImplementedException();
        }

        public GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }
    }
}
