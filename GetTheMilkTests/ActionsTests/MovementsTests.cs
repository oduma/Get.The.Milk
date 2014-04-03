using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Navigation;
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
                Assert.AreEqual(6, movement.ActiveCharacter.CellNumber);
                foreach (var followingObject in movement.ActiveCharacter.Inventory)
                {
                    Assert.AreEqual(6, followingObject.CellNumber);
                }

            }
            return movementResult.ResultType;
        }
    }
}