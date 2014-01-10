using System;
using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI;

namespace GetTheMilkTests.ActionsTests
{
    internal class StubedTextBasedInteractivity : IInteractivity
    {
        public IPositionableObject[] SelectAnObject(IPositionableObject[] availableObjects)
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

        public void SelectWeapons(List<Weapon> attackWeapons, List<IPositionableObject> rightHandObjects, List<Weapon> defenseWeapons, List<IPositionableObject> leftHandObjects)
        {
            throw new NotImplementedException();
        }

        public GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }

        public void EnterLevel(IPlayer player, ILevel level)
        {
            throw new NotImplementedException();
        }
    }
}
