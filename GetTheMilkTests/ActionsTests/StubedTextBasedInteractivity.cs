using System;
using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
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
