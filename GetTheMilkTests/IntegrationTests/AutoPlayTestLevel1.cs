using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.IntegrationTests
{
    [TestFixture]
    public class AutoPlayTestLevel1
    {
        [Test]
        public void PlayTestLevel1_PlayerLoses()
        {
            var level = Level.Create(1);

            Assert.IsNotNull(level);
            Assert.AreEqual(0, level.Number);
            Assert.AreEqual(4, level.Inventory.Count);
            Assert.AreEqual(2, level.Characters.Count);

            //create a mock for the ui interactions

            //create a new player
            var player = new Player();
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsAction = objAction.AllowsAction;
            player.AllowsIndirectAction = objAction.AllowsIndirectAction;


            player.SetPlayerName("Me");
            player.Walet.CurrentCapacity = 20;

            level.Player = null;
            //The player enters level 1
            var enterResult = player.EnterLevel(level);
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(ActionResultType.Ok, enterResult.ResultType);

            //The player walks to the east
            var walk = (new Walk());
            walk.Direction = Direction.East;
            walk.ActiveCharacter = level.Player;
            walk.CurrentMap = level.CurrentMap;

            var movementResult = walk.Perform();
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //The player walks to the south
            walk = (new Walk());
            walk.Direction = Direction.South;
            walk.ActiveCharacter = level.Player;
            walk.CurrentMap = level.CurrentMap;
            movementResult = walk.Perform();

            Assert.AreEqual(3, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange.Count());

            //the player has found something
            //pick it up and keep it

            var actionResult = ((MovementActionExtraData) movementResult.ExtraData).AvailableActions[0].Perform();

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(1,level.Player.Inventory.Count);
            Assert.AreEqual(player.Name, level.Player.Inventory[0].StorageContainer.Owner.Name);
            Assert.AreEqual(3, level.Inventory.Count);

            //the user checks his inventory

            var exposeInventory = new ExposeInventory
                                  {
                                      ActiveCharacter = level.Player,
                                      TargetCharacter = level.Player,
                                      AllowedNextActionTypes =
                                          new InventorySubActionType[]
                                          {
                                              new InventorySubActionType
                                              {
                                                  ActionType =
                                                      ActionType
                                                      .CloseInventory,
                                                  FinishInventoryExposure =
                                                      true
                                              }
                                          }
                                  };
            actionResult = exposeInventory.Perform();

            Assert.AreEqual(1,((ExposeInventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(1, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses.Length);
            Assert.True(((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].FinishTheInteractionOnExecution);
            Assert.AreEqual(ActionType.CloseInventory,((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].ActionType);

            actionResult = ((ExposeInventoryExtraData) actionResult.ExtraData).PossibleUses[0].Perform();
            Assert.AreEqual(ActionResultType.Ok,actionResult.ResultType);
            //the user runs to the east
            var run = new Run();
            run.Direction = Direction.East;
            run.CurrentMap = level.CurrentMap;
            run.ActiveCharacter = level.Player;
            movementResult = run.Perform();
            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //the player uses the key to open the door
            actionResult = ((MovementActionExtraData) movementResult.ExtraData).AvailableActions[0].Perform();

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(2, level.Inventory.Count);
            Assert.AreEqual(0, player.Inventory.Count);

            //the userruns to the east
            movementResult = run.Perform();

            Assert.AreEqual(ActionResultType.OutOfTheMap, movementResult.ResultType);
            Assert.AreEqual(5, player.CellNumber);
            Assert.AreEqual(2, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());

            ////the user decides to meet the shop keeper

            var meet =
                ((MovementActionExtraData) movementResult.ExtraData).AvailableActions.FirstOrDefault(
                    a => a.ActionType == ActionType.Meet && a.TargetCharacter.ObjectTypeId == "NPCFriendly");
            actionResult = meet.Perform();

            Assert.AreEqual(ActionType.Communicate, actionResult.ForAction.ActionType);
            Assert.AreEqual("How are you? Beautifull day out there better buy something!", ((Communicate)actionResult.ForAction).Message);

            Assert.AreEqual(2, ((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[1].ActionType);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<GameAction>)actionResult.ExtraData)[0].TargetCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<GameAction>)actionResult.ExtraData)[1].TargetCharacter);

            //the player choses to continue interaction with shopkeeper
            actionResult = ((List<GameAction>) actionResult.ExtraData)[0].Perform();

            Assert.AreEqual(2, ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[0].Name.Main,
                ((ExposeInventoryExtraData) actionResult.ExtraData).Contents.ToList()[0].Name.Main);
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[1].Name.Main,
                ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.ToList()[1].Name.Main);
            Assert.AreEqual(1, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses.Length);
            Assert.AreEqual(ActionType.Buy, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].ActionType);

            //the player buys the knife from the shopkeeper
            ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].TargetObject = ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.ToList()[0];

            var transferresult = ((ExposeInventoryExtraData) actionResult.ExtraData).PossibleUses[0].Perform();
            Assert.AreEqual(1,player.Inventory.Count());
            Assert.AreEqual(1, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory.Count());
            Assert.AreEqual(15, player.Walet.CurrentCapacity);
            Assert.AreEqual(105, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Walet.CurrentCapacity);
            Assert.AreEqual(player.Name,player.Inventory[0].StorageContainer.Owner.Name);

            //the player tries to run south

            run.Direction = Direction.South;
            movementResult = run.Perform();

            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);

            //the player attacks the fighter character

            InitiateHostilities startAttack =
                ((MovementActionExtraData) movementResult.ExtraData).AvailableActions.FirstOrDefault(
                    a => a.ActionType == ActionType.InitiateHostilities) as InitiateHostilities;

            startAttack.ActiveCharacter = level.Player;
            startAttack.TargetCharacter = level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe");
            actionResult = startAttack.Perform();

            Assert.AreEqual(1,((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.ExposeInventory, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(level.Player, ((List<GameAction>)actionResult.ExtraData)[0].ActiveCharacter);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveAttackWeapon);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveDefenseWeapon);

            //simulated the UI for selecting weapons
            actionResult = ((List<GameAction>) actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(1, ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(3, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses.Length);

            //select the knife for the attack
                ((ExposeInventoryExtraData) actionResult.ExtraData).PossibleUses[0].TargetObject =
                    level.Player.Inventory.FirstOrDefault(w => w.ObjectCategory == ObjectCategory.Weapon);
            var inventoryActionResult =
                ((ExposeInventoryExtraData) actionResult.ExtraData).PossibleUses[0].Perform();
            Assert.AreEqual(ActionResultType.Ok,inventoryActionResult.ResultType);
            Assert.IsNotNull(level.Player.ActiveAttackWeapon);

            //attack the character
            actionResult = ((ExposeInventoryExtraData) actionResult.ExtraData).PossibleUses[2].Perform();

            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.AreEqual(2,((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.Attack, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(ActionType.Quit, ((List<GameAction>)actionResult.ExtraData)[1].ActionType);
            
            //attack again
            actionResult = ((List<GameAction>) actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);

            //and again
            actionResult = ((List<GameAction>)actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);

            //and again
            actionResult = ((List<GameAction>)actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(ActionResultType.Lost, actionResult.ResultType);
        }

        [Test]
        public void PlayTestLevel1_PlayerQuits()
        {
            var level = Level.Create(1);

            Assert.IsNotNull(level);
            Assert.AreEqual(0, level.Number);
            Assert.AreEqual(4, level.Inventory.Count);
            Assert.AreEqual(2, level.Characters.Count);

            //create a mock for the ui interactions

            //var stubedUI = new StubTheInteractivity();
            //create a new player
            var player = new Player();
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            player.AllowsAction = objAction.AllowsAction;
            player.AllowsIndirectAction = objAction.AllowsIndirectAction;


            player.SetPlayerName("Me");
            player.Walet.CurrentCapacity = 20;
            player.Experience = 10;

            level.Player = null;
            //The player enters level 1
            var enterResult = player.EnterLevel(level);
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(ActionResultType.Ok, enterResult.ResultType);

            //The player walks to the east
            var walk = (new Walk());
            walk.Direction = Direction.East;
            walk.ActiveCharacter = level.Player;
            walk.CurrentMap = level.CurrentMap;

            var movementResult = walk.Perform();
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //The player walks to the south
            walk = (new Walk());
            walk.Direction = Direction.South;
            walk.ActiveCharacter = level.Player;
            walk.CurrentMap = level.CurrentMap;
            movementResult = walk.Perform();

            Assert.AreEqual(3, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange.Count());

            //the player has found something
            //pick it up and keep it

            var actionResult = ((MovementActionExtraData)movementResult.ExtraData).AvailableActions[0].Perform();

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(1, level.Player.Inventory.Count);
            Assert.AreEqual(player.Name, level.Player.Inventory[0].StorageContainer.Owner.Name);
            Assert.AreEqual(3, level.Inventory.Count);

            //the user runs to the east
            var run = new Run();
            run.Direction = Direction.East;
            run.CurrentMap = level.CurrentMap;
            run.ActiveCharacter = level.Player;
            movementResult = run.Perform();
            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //the player uses the key to open the door
            actionResult = ((MovementActionExtraData)movementResult.ExtraData).AvailableActions[0].Perform();

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(2, level.Inventory.Count);
            Assert.AreEqual(0, player.Inventory.Count);

            //the userruns to the east
            movementResult = run.Perform();

            Assert.AreEqual(ActionResultType.OutOfTheMap, movementResult.ResultType);
            Assert.AreEqual(5, player.CellNumber);
            Assert.AreEqual(2, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());

            ////the user decides to meet the shop keeper

            var meet =
                ((MovementActionExtraData)movementResult.ExtraData).AvailableActions.FirstOrDefault(
                    a => a.ActionType == ActionType.Meet && a.TargetCharacter.ObjectTypeId == "NPCFriendly");
            actionResult = meet.Perform();

            Assert.AreEqual(ActionType.Communicate, actionResult.ForAction.ActionType);
            Assert.AreEqual("How are you? Beautifull day out there better buy something!", ((Communicate)actionResult.ForAction).Message);

            Assert.AreEqual(2, ((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(ActionType.Communicate, ((List<GameAction>)actionResult.ExtraData)[1].ActionType);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<GameAction>)actionResult.ExtraData)[0].TargetCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"), ((List<GameAction>)actionResult.ExtraData)[1].TargetCharacter);

            //the player choses to continue interaction with shopkeeper
            actionResult = ((List<GameAction>)actionResult.ExtraData)[0].Perform();

            Assert.AreEqual(2, ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[0].Name.Main,
                ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.ToList()[0].Name.Main);
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[1].Name.Main,
                ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.ToList()[1].Name.Main);
            Assert.AreEqual(1, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses.Length);
            Assert.AreEqual(ActionType.Buy, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].ActionType);

            //the player buys the knife from the shopkeeper
            ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].TargetObject = ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.ToList()[0];

            var transferresult = ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].Perform();
            Assert.AreEqual(1, player.Inventory.Count());
            Assert.AreEqual(1, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory.Count());
            Assert.AreEqual(15, player.Walet.CurrentCapacity);
            Assert.AreEqual(105, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Walet.CurrentCapacity);
            Assert.AreEqual(player.Name, player.Inventory[0].StorageContainer.Owner.Name);

            //the player tries to run south

            run.Direction = Direction.South;
            movementResult = run.Perform();

            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);

            //the player attacks the fighter character

            InitiateHostilities startAttack =
                ((MovementActionExtraData)movementResult.ExtraData).AvailableActions.FirstOrDefault(
                    a => a.ActionType == ActionType.InitiateHostilities) as InitiateHostilities;

            startAttack.ActiveCharacter = level.Player;
            startAttack.TargetCharacter = level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe");
            actionResult = startAttack.Perform();

            Assert.AreEqual(1, ((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.ExposeInventory, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(level.Player, ((List<GameAction>)actionResult.ExtraData)[0].ActiveCharacter);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveAttackWeapon);
            Assert.IsNotNull(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").ActiveDefenseWeapon);

            //simulated the UI for selecting weapons
            actionResult = ((List<GameAction>)actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(1, ((ExposeInventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(3, ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses.Length);

            //select the knife for the attack
            ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].TargetObject =
                level.Player.Inventory.FirstOrDefault(w => w.ObjectCategory == ObjectCategory.Weapon);
            var inventoryActionResult =
                ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[0].Perform();
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.IsNotNull(level.Player.ActiveAttackWeapon);

            //attack the character
            actionResult = ((ExposeInventoryExtraData)actionResult.ExtraData).PossibleUses[2].Perform();

            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);
            Assert.AreEqual(2, ((List<GameAction>)actionResult.ExtraData).Count);
            Assert.AreEqual(ActionType.Attack, ((List<GameAction>)actionResult.ExtraData)[0].ActionType);
            Assert.AreEqual(ActionType.Quit, ((List<GameAction>)actionResult.ExtraData)[1].ActionType);

            //attack again
            actionResult = ((List<GameAction>)actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);

            //and again
            actionResult = ((List<GameAction>)actionResult.ExtraData)[0].Perform();
            Assert.AreEqual(ActionResultType.Ok, inventoryActionResult.ResultType);

            //and quits
            actionResult = ((List<GameAction>)actionResult.ExtraData)[1].Perform();
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(8,player.Experience);
            Assert.AreEqual(2,level.Characters.FirstOrDefault(c=>c.ObjectTypeId=="NPCFoe").Experience);
            Assert.AreEqual(0,player.Inventory.Count);
            Assert.AreEqual(2, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").Inventory.Count);
            Assert.AreEqual(0,player.Walet.CurrentCapacity);
            Assert.AreEqual(415, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe").Walet.CurrentCapacity);
        }

    }
}
