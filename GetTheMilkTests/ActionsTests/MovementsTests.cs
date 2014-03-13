using System.Collections.Generic;
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
        public ActionResultType MoveTest(Player active, Level level, MovementAction movement, Map currentMap,List<NonCharacterObject> blockingObjects,List<Character> blockingCharacters)
        {
            active.EnterLevel(level);
            var movementResult = active.TryPerformMove(movement, currentMap, blockingObjects, blockingCharacters);
            if(movement.Direction==Direction.North)
                Assert.AreEqual(active.CellNumber,((MovementActionExtraData)movementResult.ExtraData).MoveToCell);
            else if(movement.Direction==Direction.East)
            {
                Assert.AreEqual(1,((MovementActionExtraData)movementResult.ExtraData).MoveToCell);
                Assert.AreEqual(1,((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Length);
            }
            else if(movement is Run && movement.Direction==Direction.South)
            {
                Assert.AreEqual(4, ((MovementActionExtraData)movementResult.ExtraData).MoveToCell);
                Assert.AreEqual(1,((MovementActionExtraData)movementResult.ExtraData).CharactersBlocking.Length);
            }
            else
            {
                Assert.AreEqual(4,((MovementActionExtraData)movementResult.ExtraData).MoveToCell);
            }
            if(movementResult.ResultType==ActionResultType.Ok)
            {
                Assert.AreEqual(4,active.CellNumber);
                foreach(var followingObject in active.Inventory.Objects)
                {
                    Assert.AreEqual(4,followingObject.CellNumber);
                }
            }
            return movementResult.ResultType;
        }
    }
}