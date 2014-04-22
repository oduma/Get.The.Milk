using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class DetermineActionsTests
    {
        [Test]
        public void DetermineActionsOnObjects()
        {
            var level = Level.Create(0);
            var player = new Player();

            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsAction = objAction.AllowsAction;
            player.AllowsIndirectAction = objAction.AllowsIndirectAction;

            var movementResult= player.EnterLevel(level);
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).AvailableActions.Count);
            Assert.AreEqual(ActionType.Keep,((MovementActionExtraData)movementResult.ExtraData).AvailableActions[0].ActionType);
            Assert.AreEqual(level.Player,((MovementActionExtraData)movementResult.ExtraData).AvailableActions[0].ActiveCharacter);

        }

        [Test]
        public void DetermineActionsOnCharacters()
        {
            var level = Level.Create(0);
            var player = new Player();

            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsAction = objAction.AllowsAction;
            player.AllowsIndirectAction = objAction.AllowsIndirectAction;

            level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").CellNumber = 1;
            level.CurrentMap.Cells[1].AllObjects.First().CellNumber = 2;

            var movementResult = player.EnterLevel(level);
            Assert.AreEqual(2, ((MovementActionExtraData)movementResult.ExtraData).AvailableActions.Count);
            Assert.AreEqual(ActionType.Meet, ((MovementActionExtraData)movementResult.ExtraData).AvailableActions[1].ActionType);
            Assert.AreEqual(level.Player, ((MovementActionExtraData)movementResult.ExtraData).AvailableActions[1].ActiveCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((MovementActionExtraData)movementResult.ExtraData).AvailableActions[1].TargetCharacter);

        }

        [Test]
        public void DetermineFollowUpActionsOnCharacters()
        {
            var level = Level.Create(0);
            var player = new Player();

            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsAction = objAction.AllowsAction;
            player.AllowsIndirectAction = objAction.AllowsIndirectAction;

            level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").CellNumber = 1;
            level.CurrentMap.Cells[1].AllObjects.First().CellNumber = 2;

            var movementResult = player.EnterLevel(level);
            var actionResult = ((MovementActionExtraData) movementResult.ExtraData).AvailableActions[1].Perform();

            Assert.AreEqual(ActionType.Communicate,actionResult.ForAction.ActionType);
            Assert.AreEqual("How are you? Beautifull day out there better buy something!", ((Communicate)actionResult.ForAction).Message);

            Assert.AreEqual(2, ((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[1].ActionType);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<GameAction>)actionResult.ExtraData)[0].TargetCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<GameAction>)actionResult.ExtraData)[1].TargetCharacter);

            //((List<GameAction>) actionResult.ExtraData)[0].TargetCharacter = level.Player;
            //actionResult = ((List<GameAction>) actionResult.ExtraData)[0].Perform();

            //Assert.AreEqual(2, ((List<GameAction>)actionResult.ExtraData).Count);
            //Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);

        }


    }
}
