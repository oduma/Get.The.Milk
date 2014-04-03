using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.UI
{
    public class NoInteractivityProvider:IInteractivity
    {
        public IPositionable[] SelectAnObject(IPositionable[] availableObjects)
        {
            throw new NotImplementedException();
        }

        public NonCharacterObject[] SelectAnObject(NonCharacterObject[] availableObjects)
        {
            throw new NotImplementedException();
        }

        public ICharacter SelectACharacter(ICharacter[] availableCharacters)
        {
            throw new NotImplementedException();
        }

        public GameAction SelectAnAction(GameAction[] availableActions, ICharacter targetCharacter)
        {
            var optionId = Randomizer.GetRandom(0,availableActions.Length);
            return availableActions[optionId];
        }

        public GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }

        public void SelectWeapons(IEnumerable<Weapon> weapons, ref Weapon activeAttackWeapon, ref Weapon activeDefenseWeapon)
        {
            weapons.ForEach(w=>w.IsCurrentAttack=false);
            activeAttackWeapon = weapons.FirstOrDefault(w => w.WeaponTypes.Contains(WeaponType.Attack));
            if (activeAttackWeapon != null)
                activeAttackWeapon.IsCurrentAttack = true;

            weapons.ForEach(w=>w.IsCurrentDefense=false);
            activeDefenseWeapon = weapons.FirstOrDefault(w => w.WeaponTypes.Contains(WeaponType.Deffense));
            if (activeDefenseWeapon != null)
                activeDefenseWeapon.IsCurrentDefense = true;
        }

        public GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }
    }
}
