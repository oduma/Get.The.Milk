using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.IntegrationTests
{
    [TestFixture]
    public class AutoPlayTestLevel1
    {
        [Test]
        public void PlayTestLevel1_PlayerLoses()
        {
            PerformActionResult actionResult;
            PerformActionResult movementResult;
            var level = GetToTheFight(out movementResult);

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

        private static Level GetToTheFight(out PerformActionResult movementResult)
        {
            var level = GetTheMilk.Levels.Level.Create(0);

            Assert.IsNotNull(level);
            Assert.AreEqual(0, level.Number);
            Assert.AreEqual(4, level.Inventory.Count);
            Assert.AreEqual(2, level.Characters.Count);

            //create a new player
            var player = new Player();
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
            player.Health = 10;
            player.Experience = 50;

            player.SetPlayerName("Me");
            player.Walet.CurrentCapacity = 20;

            level.Player = null;
            //The player enters level 1
            var enterResult = player.EnterLevel(level);
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(ActionResultType.Ok, enterResult.ResultType);

            //The player walks to the east
            var walk = player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            walk.Direction = Direction.East;
            walk.CurrentMap = level.CurrentMap;

            movementResult = level.Player.PerformAction(walk);
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionTemplateExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //The player walks to the south
            walk.Direction = Direction.South;
            movementResult = level.Player.PerformAction(walk);

            Assert.AreEqual(3, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionTemplateExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(0, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsBlocking.Count());
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsInRange.Count());

            //the player has found something
            //pick it up and keep it

            var actionResult =
                level.Player.PerformAction(
                    ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0]);

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(1, level.Player.Inventory.Count);
            Assert.AreEqual(player.Name, level.Player.Inventory[0].StorageContainer.Owner.Name);
            Assert.AreEqual(3, level.Inventory.Count);

            //the user checks his inventory

            var exposeInventory = level.Player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            actionResult = level.Player.PerformAction(exposeInventory);

            Assert.AreEqual(1, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.IsEmpty(((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses);

            //the user closes his inventory
            var closeInventory = level.Player.CreateNewInstanceOfAction(exposeInventory.FinishingAction.ToString());
            actionResult = level.Player.PerformAction(closeInventory);
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            //the user runs to the east
            var run = level.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Run");
            run.Direction = Direction.East;
            run.CurrentMap = level.CurrentMap;
            movementResult = level.Player.PerformAction(run);
            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //the player uses the key to open the door
            actionResult =
                level.Player.PerformAction(
                    ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0]);

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(2, level.Inventory.Count);
            Assert.AreEqual(0, player.Inventory.Count);

            //the userruns to the east
            movementResult = level.Player.PerformAction(run);

            Assert.AreEqual(ActionResultType.OutOfTheMap, movementResult.ResultType);
            Assert.AreEqual(5, player.CellNumber);
            Assert.AreEqual(2, ((MovementActionTemplateExtraData)movementResult.ExtraData).CharactersInRange.Count());

            ////the user decides to meet the shop keeper

            var meet =
                ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.FirstOrDefault(
                    a => a.Name.UniqueId == "Meet" && a.TargetCharacter.ObjectTypeId == "NPCFriendly");
            actionResult = level.Player.PerformAction(meet);

            Assert.AreEqual("Talk", actionResult.ForAction.Name.UniqueId);
            Assert.AreEqual("How are you? Beautifull day out there better buy something!",
                            ((TwoCharactersActionTemplate)actionResult.ForAction).Message);

            Assert.AreEqual(2, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("Responde", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual("RespondeNO", ((List<BaseActionTemplate>)actionResult.ExtraData)[1].Name.UniqueId);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"),
                            ((List<BaseActionTemplate>)actionResult.ExtraData)[0].TargetCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"),
                            ((List<BaseActionTemplate>)actionResult.ExtraData)[1].TargetCharacter);

            //the player choses to continue interaction with shopkeeper
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);

            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[0].Name.Main,
                ((InventoryExtraData)actionResult.ExtraData).Contents[0].Object.Name.Main);
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.Count());
            Assert.AreEqual("Buy",
                            ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First().Name.UniqueId);
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[1].Name.Main,
                ((InventoryExtraData)actionResult.ExtraData).Contents[1].Object.Name.Main);
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[1].PossibleUsses.Count());
            Assert.AreEqual("Buy",
                            ((InventoryExtraData)actionResult.ExtraData).Contents[1].PossibleUsses.First().Name.UniqueId);

            //the player buys the knife from the shopkeeper

            var transferresult =
                level.Player.PerformAction(((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First());
            Assert.AreEqual(1, player.Inventory.Count());
            Assert.AreEqual(1, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory.Count());
            Assert.AreEqual(15, player.Walet.CurrentCapacity);
            Assert.AreEqual(105, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Walet.CurrentCapacity);
            Assert.AreEqual(player.Name, player.Inventory[0].StorageContainer.Owner.Name);

            //the player tries to run south

            run.Direction = Direction.South;
            movementResult = level.Player.PerformAction(run);

            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            return level;
        }

        [Test]
        public void PlayTestLevel1_PlayerQuits()
        {
            PerformActionResult actionResult;
            PerformActionResult movementResult;
            var level = GetToTheFight(out movementResult);

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
            Assert.AreEqual(12, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").Experience);
            Assert.AreEqual(1, level.Player.Inventory.Count);
        }

        [Test]
        public void PlayTestLevel1_PlayerWins()
        {
            PerformActionResult actionResult;
            PerformActionResult movementResult;
            var level = GetToTheFight(out movementResult);

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
