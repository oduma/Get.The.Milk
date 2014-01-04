﻿using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Navigation;
using NUnit.Framework;

namespace GetTheMilkTests.IntegrationTests
{
    [TestFixture]
    public class AutoPlayTestLevel1
    {
        [Test]
        public void PlayTestLevel1()
        {
            var level = (new LevelsFactory()).CreateLevel(1);

            Assert.IsNotNull(level);
            Assert.AreEqual(1, level.Number);
            Assert.AreEqual(4, level.PositionableObjects.Objects.Count);
            Assert.AreEqual(2, level.Characters.Objects.Count);

            //create a mock for the ui interactions

           // var mockTheUi = (new Mock<IInteractivity>());
            var stubedUI = new StubTheInteractivity();
            //create a new player
            var player = new Player(stubedUI);
            player.Walet.CurrentCapacity = 20;


            //The player enters level 1
            level.EnterLevel(player,1,1);
            Assert.AreEqual(1,player.MapNumber);
            Assert.AreEqual(1,player.CellNumber);

            //The player walks to the east
            var walk = (new Walk());
            walk.Direction = Direction.East;
            var movementResult = player.TryPerformMove(walk,
                                  level.Maps.FirstOrDefault(m => m.Number == player.MapNumber),
                                  level.PositionableObjects.Objects, level.Characters.Objects);
            Assert.AreEqual(1,player.MapNumber);
            Assert.AreEqual(1,player.CellNumber);
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //The player walks to the south
            walk = (new Walk());
            walk.Direction = Direction.South;
            movementResult = player.TryPerformMove(walk,
                                  level.Maps.FirstOrDefault(m => m.Number == player.MapNumber),
                                  level.PositionableObjects.Objects, level.Characters.Objects);
            Assert.AreEqual(1, player.MapNumber);
            Assert.AreEqual(4, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(0, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange.Count());
            
            //the player has found something
            //pick it up and keep it

            var pick = new Keep();
            //simulate the UI
            var actionResult = player.TryPerformAction(pick, stubedUI.SelectAnObject(((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange));
            Assert.AreEqual(ActionResultType.Ok,actionResult.ResultType);
            Assert.AreEqual(player.Name, ((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange[0].StorageContainer.Owner.Name);
            Assert.AreEqual(3,level.PositionableObjects.Objects.Count);

            //the user runs to the east
            var run = new Run();
            run.Direction = Direction.East;
            movementResult = player.TryPerformMove(run,
                      level.Maps.FirstOrDefault(m => m.Number == player.MapNumber),
                      level.PositionableObjects.Objects, level.Characters.Objects);
            Assert.AreEqual(ActionResultType.BlockedByObject,movementResult.ResultType);
            Assert.AreEqual(1, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //the player uses the key to open the door
            var openDoor = new Open();

            actionResult = player.TryPerformObjectOnObjectAction(openDoor,
                                                                 ref ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking[0]);
            Assert.AreEqual(ActionResultType.Ok,actionResult.ResultType);
            Assert.AreEqual(2,level.PositionableObjects.Objects.Count);
            Assert.AreEqual(0,player.ToolInventory.Objects.Count);

            //the userruns to the east
            movementResult = player.TryPerformMove(run, level.Maps.FirstOrDefault(m => m.Number == player.MapNumber),
                                                   level.PositionableObjects.Objects, level.Characters.Objects);
            Assert.AreEqual(ActionResultType.OutOfTheMap,movementResult.ResultType);
            Assert.AreEqual(1, player.MapNumber);
            Assert.AreEqual(6, player.CellNumber);
            Assert.AreEqual(2, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange.Count());

            //the user decides to meet the shop keeper

            var meet = new Meet();

            var interactionResult = player.StartInteraction(meet, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange[0] as ICharacter);

            //the player buys the knife from the shopkeeper

            var transferresult= player.TryPerformAction(
                player.ChooseFromAnotherInventory(interactionResult.ExtraData as ExposeInventoryExtraData)
                as ObjectTransferAction, ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange[0] as ICharacter);
            Assert.AreEqual(1,player.RightHandObject.Objects.Count);
            Assert.AreEqual(0, ((ICharacter)((MovementActionExtraData)movementResult.ExtraData).CharactersInRange[0]).WeaponInventory.Objects.Count);
            Assert.AreEqual(10,player.Walet.CurrentCapacity);
            Assert.AreEqual(210, ((ICharacter)((MovementActionExtraData)movementResult.ExtraData).CharactersInRange[0]).Walet.CurrentCapacity);
            Assert.AreEqual(player.Name,player.RightHandObject.Objects[0].StorageContainer.Owner.Name);

            //the player tries to run south

            run.Direction = Direction.South;
            movementResult = player.TryPerformMove(run, level.Maps[0], level.PositionableObjects.Objects, level.Characters.Objects);

            Assert.AreEqual(ActionResultType.BlockedByCharacter,movementResult.ResultType);

            //the player attacks the fighter character

            //simulated the UI for selecting an option
            var attack = new Attack ();

            interactionResult = player.StartInteraction(attack, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking[0] as ICharacter);

            Assert.AreNotEqual(ActionResultType.NotOk,interactionResult.ResultType);
            if(interactionResult.ResultType==ActionResultType.Win)
            {
                GameAction[] gameActions =
                    stubedUI.BuildActions(interactionResult.ExtraData as ExposeInventoryExtraData);
                foreach(var gameAction in gameActions)
                {
                    player.TryPerformAction(gameAction, ((MovementActionExtraData)movementResult.ExtraData).ObjectsBlocking[0] as ICharacter);
                }
                Assert.AreEqual(1,level.Characters.Objects.Count);
                Assert.AreEqual(1,player.Experience);
                Assert.AreEqual(1,player.LeftHandObject.Objects.Count);
                Assert.AreEqual(1,player.RightHandObject.Objects.Count);
                Assert.AreEqual(200,player.Walet.CurrentCapacity);
            }

        }
    }
}