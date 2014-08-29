using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersPerformMovementAction
    {
        private Character _character;
        private Level _level;

        [SetUp]
        public void SetUp()
        {
            _character = new Character
                             {
                                 ObjectTypeId = "NPCFriendly",
                                 Inventory =
                                     new Inventory
                                         {MaximumCapacity = 200, InventoryType = InventoryType.CharacterInventory}
                             };
            var objAction = ObjectActionsFactory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

        }

        [Test]
        public void AvailableActionsCollectedOkAfterMove()
        {
            var movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            movementAction.CurrentMap = _level.CurrentMap;
            _level.Characters[0].CellNumber = 1;
            _level.Inventory[0].CellNumber = 3;
            movementAction.Direction = Direction.None;
            var movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.Ok,movementResult.ResultType);
            Assert.AreEqual(typeof(MovementActionTemplate),movementResult.ForAction.GetType());
            var extraData = movementResult.ExtraData as MovementActionTemplateExtraData;
            Assert.False(extraData.CharactersBlocking.Any());
            Assert.False(extraData.ObjectsBlocking.Any());
            Assert.False(extraData.ObjectsInCell.Any());
            Assert.AreEqual(1,extraData.CharactersInRange.Count());
            Assert.AreEqual(2, extraData.ObjectsInRange.Count());
            Assert.AreEqual(2,extraData.AvailableActionTemplates.Count(a=>a.StartingAction));
            Assert.True(
                _character.AllActions.Any(
                    a =>
                    a.Key == extraData.AvailableActionTemplates[0].Name.UniqueId));
            Assert.True(
                _character.AllActions.Any(
                    a =>
                    a.Key == extraData.AvailableActionTemplates[1].Name.UniqueId));
            Assert.True(extraData.AvailableActionTemplates.Any(a => a.GetType() == typeof(TwoCharactersActionTemplate)));
            Assert.True(extraData.AvailableActionTemplates.Any(a => a.GetType() == typeof(ObjectTransferActionTemplate)));
        }

        [Test]
        public void AvailableActionsCollectedOkAfterMoveFoeCharacter()
        {
            var movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = _level.CurrentMap;
            _level.Characters[1].CellNumber = 1;
            _level.Inventory[0].CellNumber = 3;
            movementAction.Direction = Direction.None;
            var movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.Ok, movementResult.ResultType);
            Assert.AreEqual(typeof(MovementActionTemplate), movementResult.ForAction.GetType());
            var extraData = movementResult.ExtraData as MovementActionTemplateExtraData;
            Assert.False(extraData.CharactersBlocking.Any());
            Assert.False(extraData.ObjectsBlocking.Any());
            Assert.False(extraData.ObjectsInCell.Any());
            Assert.AreEqual(1, extraData.CharactersInRange.Count());
            Assert.AreEqual(2, extraData.ObjectsInRange.Count());
            Assert.AreEqual(3, extraData.AvailableActionTemplates.Count(a => a.StartingAction));
            Assert.True(extraData.AvailableActionTemplates.Any(a => a.GetType() == typeof(TwoCharactersActionTemplate)));
            Assert.True(extraData.AvailableActionTemplates.Any(a => a.GetType() == typeof(ObjectTransferActionTemplate)));
        }

        [Test]
        public void CharacterMoveNotPermited()
        {
            var movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = _level.CurrentMap;
            _level.Characters[0].CellNumber = 3;
            _level.Inventory[0].CellNumber = 3;
            movementAction.Direction = Direction.None;
            var movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.Ok, movementResult.ResultType);

            //walk out of the map
            movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = _level.CurrentMap;
            movementAction.Direction = Direction.West;
            movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.OutOfTheMap, movementResult.ResultType);
            Assert.AreEqual(0,_character.CellNumber);

            //walk blocked by object
            movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = _level.CurrentMap;
            movementAction.Direction = Direction.East;
            movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            Assert.AreEqual(0, _character.CellNumber);
            var extraData = movementResult.ExtraData as MovementActionTemplateExtraData;
            Assert.False(extraData.CharactersBlocking.Any());
            Assert.True(extraData.ObjectsBlocking.Any());
            Assert.AreEqual(1, extraData.ObjectsBlocking.Count());

            //walk blocked by character
            movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = _level.CurrentMap;
            movementAction.Direction = Direction.South;
            movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            Assert.AreEqual(0, _character.CellNumber);
            extraData = movementResult.ExtraData as MovementActionTemplateExtraData;
            Assert.True(extraData.CharactersBlocking.Any());
            Assert.False(extraData.ObjectsBlocking.Any());
            Assert.AreEqual(1, extraData.CharactersBlocking.Count());

            //walk completely wrong from start
            movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Walk"); 
            movementAction.ActiveCharacter = _character;
            _character.CellNumber = 10;
            movementAction.CurrentMap = _level.CurrentMap;
            movementAction.Direction = Direction.South;
            movementResult = _character.PerformAction(movementAction);
            Assert.AreEqual(ActionResultType.OriginNotOnTheMap, movementResult.ResultType);
            Assert.AreEqual(10, _character.CellNumber);
            extraData = movementResult.ExtraData as MovementActionTemplateExtraData;
            Assert.IsNull(extraData.CharactersBlocking);
            Assert.IsNull(extraData.ObjectsBlocking);
        }
    }
}
