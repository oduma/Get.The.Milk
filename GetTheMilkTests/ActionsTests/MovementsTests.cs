using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class MovementsTests
    {
        [Test, TestCaseSource(typeof (DataGeneratorForActions), "TestCasesM")]
        public ActionResultType MoveTest(MovementAction movement)
        {
            var movementResult = movement.Perform();
            if(movement.Direction==Direction.North)
                Assert.AreEqual(0,movement.ActiveCharacter.CellNumber);
            else if(movement.Direction==Direction.East)
            {
                Assert.AreEqual(0, movement.ActiveCharacter.CellNumber);
                Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());
            }
            else if(movement is Run && movement.Direction==Direction.South)
            {
                Assert.AreEqual(3, movement.ActiveCharacter.CellNumber);
                Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).CharactersBlocking.Count());
            }
            else
            {
                Assert.AreEqual(3, movement.ActiveCharacter.CellNumber);
            }
            if(movementResult.ResultType==ActionResultType.Ok)
            {
                Assert.AreEqual(3, movement.ActiveCharacter.CellNumber);
                foreach (var followingObject in movement.ActiveCharacter.Inventory)
                {
                    Assert.AreEqual(4,followingObject.CellNumber);
                }
            }
            return movementResult.ResultType;
        }
    }
}