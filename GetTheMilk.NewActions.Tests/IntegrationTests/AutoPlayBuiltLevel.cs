using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.GameLevels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests.IntegrationTests
{
    [TestFixture]
    public class AutoPlayBuiltLevel
    {
        [Test]
        public void PlayTestLevel3_PlayerLoses()
        {
            PerformActionResult actionResult;
            PerformActionResult movementResult;
            var level = TestHelper.GetToTheFight(out movementResult,3);

            //the player attacks the fighter character

            var startAttack =
                ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.FirstOrDefault(
                    a => a.Name.UniqueId == "InitiateHostilities");

            actionResult = level.Player.PerformAction(startAttack);

            Assert.AreEqual(1, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("PrepareForBattle", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual(level.Player, ((List<BaseActionTemplate>)actionResult.ExtraData)[0].ActiveCharacter);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveAttackWeapon);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveDefenseWeapon);

            //simulated the UI for selecting weapons
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(1, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.Count());

            //select the knife for the attack
            var inventoryActionResult = level.Player.PerformAction(
                ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First());
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.IsNotNull(level.Player.ActiveAttackWeapon);

            //give the character a health boost
            ((InventoryExtraData)actionResult.ExtraData).FinishingAction.TargetCharacter.Health = 20;
            //attack the character
            actionResult = level.Player.PerformAction(((InventoryExtraData)actionResult.ExtraData).FinishingAction);

            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.AreEqual(2, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("Attack", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual("Quit", ((List<BaseActionTemplate>)actionResult.ExtraData)[1].Name.UniqueId);
            Assert.AreEqual(9, level.Player.Health);

            while (level.Player.Health > 1)
            {
                //attack again
                actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
                Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            }

            //and again
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.IsNull(actionResult.ExtraData);
            Assert.AreEqual(0, level.Player.Health);

        }

        [Test]
        public void PlayTestLevel3_PlayerQuits()
        {
            PerformActionResult actionResult;
            PerformActionResult movementResult;
            var level = TestHelper.GetToTheFight(out movementResult,3);

            var startAttack =
                ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.FirstOrDefault(
                    a => a.Name.UniqueId == "InitiateHostilities");

            actionResult = level.Player.PerformAction(startAttack);

            Assert.AreEqual(1, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("PrepareForBattle", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual(level.Player, ((List<BaseActionTemplate>)actionResult.ExtraData)[0].ActiveCharacter);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveAttackWeapon);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveDefenseWeapon);

            //simulated the UI for selecting weapons
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(1, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.Count());

            //select the knife for the attack
            var inventoryActionResult = level.Player.PerformAction(
                ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First());
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.IsNotNull(level.Player.ActiveAttackWeapon);

            //give the character a health boost
            ((InventoryExtraData)actionResult.ExtraData).FinishingAction.TargetCharacter.Health = 20;

            //attack the character
            actionResult = level.Player.PerformAction(((InventoryExtraData)actionResult.ExtraData).FinishingAction);

            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.AreEqual(2, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("Attack", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual("Quit", ((List<BaseActionTemplate>)actionResult.ExtraData)[1].Name.UniqueId);

            //attack again
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);

            //and again
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);

            //and quits
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[1]);
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(38, level.Player.Experience);
            Assert.AreEqual(14, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").Experience);
            Assert.AreEqual(1, level.Player.Inventory.Count);
        }

        [Test]
        public void PlayTestLevel3_PlayerWins()
        {
            PerformActionResult actionResult;
            PerformActionResult movementResult;
            var level = TestHelper.GetToTheFight(out movementResult,3);

            var startAttack =
                ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.FirstOrDefault(
                    a => a.Name.UniqueId == "InitiateHostilities");

            actionResult = level.Player.PerformAction(startAttack);

            Assert.AreEqual(1, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("PrepareForBattle", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual(level.Player, ((List<BaseActionTemplate>)actionResult.ExtraData)[0].ActiveCharacter);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveAttackWeapon);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveDefenseWeapon);
            //The fighter character has to lose so no defense
            level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveDefenseWeapon = null;
            //simulated the UI for selecting weapons
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(1, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.Count());

            //select the knife for the attack
            var inventoryActionResult = level.Player.PerformAction(
                ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First());
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.IsNotNull(level.Player.ActiveAttackWeapon);
            //select the knife for the defense weapon
            inventoryActionResult = level.Player.PerformAction(
                ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First(p => p.Name.UniqueId == "SelectDefenseWeapon"));
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.IsNotNull(level.Player.ActiveDefenseWeapon);

            //give the character a health boost
            ((InventoryExtraData)actionResult.ExtraData).FinishingAction.TargetCharacter.Health = 5;

            //attack the character
            actionResult = level.Player.PerformAction(((InventoryExtraData)actionResult.ExtraData).FinishingAction);

            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.AreEqual(2, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("Attack", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual("Quit", ((List<BaseActionTemplate>)actionResult.ExtraData)[1].Name.UniqueId);

            while (level.Characters.First(c => c.ObjectTypeId == "NPCFoe").Health > 1)
            {
                //attack again
                actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
                Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            }

            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(1, level.Characters.Count);
            Assert.AreEqual(2, level.Player.Inventory.Count);

            var walk = level.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            //player walks south
            walk.Direction = Direction.South;
            walk.CurrentMap = level.CurrentMap;

            movementResult = level.Player.PerformAction(walk);

            Assert.IsNotNull(movementResult);

            Assert.AreEqual(ActionResultType.LevelCompleted, movementResult.ResultType);
            Assert.IsNull(movementResult.ExtraData);


        }


    }
}
