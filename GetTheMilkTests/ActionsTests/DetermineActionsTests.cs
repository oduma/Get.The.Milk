using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
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
            player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            var movementResult= player.EnterLevel(level);
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.Count());
            Assert.AreEqual("Keep",((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.First().Name.PerformerId);
            Assert.AreEqual(level.Player,((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.First().ActiveCharacter);

        }

        [Test]
        public void DetermineActionsOnCharacters()
        {
            var level = Level.Create(0);
            var player = new Player();

            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").CellNumber = 1;
            level.CurrentMap.Cells[1].AllObjects.First().CellNumber = 2;

            var movementResult = player.EnterLevel(level);
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.Count());
            Assert.AreEqual("Meet", ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0].Name.PerformerId);
            Assert.AreEqual(level.Player, ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0].ActiveCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0].TargetCharacter);

        }

        [Test]
        public void DetermineFollowUpActionsOnCharacters()
        {
            var level = Level.Create(0);
            var player = new Player();

            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").CellNumber = 1;
            level.CurrentMap.Cells[1].AllObjects.First().CellNumber = 2;

            var movementResult = player.EnterLevel(level);
            var actionResult = player.PerformAction(((MovementActionTemplateExtraData) movementResult.ExtraData).AvailableActionTemplates.ToArray()[1]);

            Assert.AreEqual("Talk",actionResult.ForAction.Name.PerformerId);
            Assert.AreEqual("How are you? Beautifull day out there better buy something!", ((TwoCharactersActionTemplate)actionResult.ForAction).Message);

            Assert.AreEqual(2, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("Talk", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.PerformerId);
            Assert.AreEqual("Talk", ((List<BaseActionTemplate>)actionResult.ExtraData)[1].Name.PerformerId);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<BaseActionTemplate>)actionResult.ExtraData)[0].TargetCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<BaseActionTemplate>)actionResult.ExtraData)[1].TargetCharacter);

            //((List<GameAction>) actionResult.ExtraData)[0].TargetCharacter = level.Player;
            //actionResult = ((List<GameAction>) actionResult.ExtraData)[0].Perform();

            //Assert.AreEqual(2, ((List<GameAction>)actionResult.ExtraData).Count);
            //Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);

        }


    }
}
