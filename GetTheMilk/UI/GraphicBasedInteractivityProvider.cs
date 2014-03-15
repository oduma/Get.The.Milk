using System;
using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.UI
{
    public class GraphicBasedInteractivityProvider:IInteractivity
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

        public GameAction SelectAnAction(GameAction[] availableObjects, ICharacter targetCharacter)
        {
            throw new NotImplementedException();
        }

        public GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }

        public void SelectWeapons(IEnumerable<Weapon> weapons, ref Weapon activeAttackWeapon, ref Weapon activeDefenseWeapon)
        {
            throw new NotImplementedException();
        }

        public GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }
    }
}
