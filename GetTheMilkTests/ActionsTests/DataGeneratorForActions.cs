using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetTheMilk;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    public class DataGeneratorForActions
    {
        
        public static IEnumerable TestCases1C1O
        {
            get
            {
                //Kick a wall (nothing happens)
                yield return
                    new TestCaseData(CreateACharacter("Me"), (new ObjectsFactory(new LevelInstaller())).CreateObject <OneObjectAction>("Kick"), (new ObjectsFactory(new LevelInstaller())).CreateObject<IPositionableObject>("Wall")).Returns(null);
                //Kick a window (nothing happens)
                yield return
                    new TestCaseData(CreateACharacter("Me"), (new ObjectsFactory(new LevelInstaller())).CreateObject <OneObjectAction>("Kick"), (new ObjectsFactory(new LevelInstaller())).CreateObject<IPositionableObject>("Window")).Returns(null);
                //Pick a key, key in hand
                var meCharacter = CreateACharacter("Me");
                yield return
                    new TestCaseData(meCharacter, (new ObjectsFactory(new LevelInstaller())).CreateObject <OneObjectAction>("Pick"), (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey")).Returns(meCharacter.RightHandObject);
                //Put the key in the tool inventory
                var character = CreateACharacter("Me");
                yield return
                    new TestCaseData(character, (new ObjectsFactory(new LevelInstaller())).CreateObject <OneObjectAction>("Keep"), CreateToolInHand(character, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"))).Returns(
                        character.ToolInventory);
                //try to put a key in a full tool inventory
                character = CreateACharacter("Me", true);
                yield return
                    new TestCaseData(character, (new ObjectsFactory(new LevelInstaller())).CreateObject <OneObjectAction>("Keep"), CreateToolInHand(character, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"))).Returns(character.RightHandObject);
            }
        }

        public static IEnumerable TestCases2C1O
        {
            get
            {
                //Give a key to a character that doesn't want it (nothing happens)
                var active = CreateACharacter("Me");
                var passive = CreateACharacter("Key Master");
                var action = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("GiveTo");
                action.UseableObject = CreateToolInHand(active,new ObjectsFactory(new LevelInstaller()).CreateObject<ITransactionalObject>("BlueKey"));
                yield return
                    new TestCaseData(active, passive,action).Returns(
                        active.RightHandObject);

                //Give a key to a character that wants it (key changes owners)
                active = CreateACharacter("Me");
                passive = CreateACharacter("Keyless Child");
                action = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("GiveTo");
                action.UseableObject = CreateToolInHand(active,
                                                        (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>(
                                                            "BlueKey"));
                yield return
                    new TestCaseData(active, passive,action ).Returns(
                        passive.RightHandObject);

            }
        }

        public static IEnumerable TestCases2CC
        {
            get
            {
                //not enough money
                var active = CreateACharacter("Me");
                var passive = CreateACharacter("Shop Keeper");

                yield return new TestCaseData(active, passive, 100, TransactionType.Debit).Returns(
                        active.Walet.CurrentCapacity);

                yield return new TestCaseData(active, passive, 10, TransactionType.Debit).Returns(
                    40);

            }
        }

        public static IEnumerable TestCases2C1OC
        {
            get
            {
                //Try to buy a key from a character not enough money (nothing happens)
                var active = CreateACharacter("Me");
                var passive = CreateACharacter("Shop Keeper");

                var buyAction = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("Buy");
                buyAction.UseableObject = CreateToolInHand(passive,
                                                        (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>(
                                                            "BlueKey"));
                yield return
                    new TestCaseData(active, passive,buyAction ).Returns(
                        50);

                //Try to sell a key to a character not enough money (nothing happens)
                active = CreateACharacter("Me");
                passive = CreateACharacter("Keyless Child");
                
                var sellAction = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("Sell");
                sellAction.UseableObject = CreateToolInHand(active,
                                        (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>(
                                            "BlueKey"));

                yield return
                    new TestCaseData(active, passive, sellAction).Returns(
                        50);

                //Try to buy a key no room in your inventory
                active = CreateACharacter("Me",true);
                passive = CreateACharacter("Shop Keeper");

                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                buyAction = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("Buy");
                buyAction.UseableObject = CreateToolInHand(passive,
                                        (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>(
                                            "SkrewDriver"));
                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        50);
                //Succesfully buy a skrew driver
                active = CreateACharacter("Me");
                passive = CreateACharacter("Shop Keeper");

                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                buyAction = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("Buy");
                buyAction.UseableObject = CreateToolInHand(passive,
                                        (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>(
                                            "SkrewDriver"));
                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        45);

                //Succesfully sell a skrew driver
                active = CreateACharacter("Me");
                passive = CreateACharacter("Keyless Child");
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                sellAction = (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectTransferAction>("Sell");
                sellAction.UseableObject = CreateToolInHand(active,
                                        (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>(
                                            "SkrewDriver"));

                yield return
                    new TestCaseData(active, passive, sellAction).Returns(
                        51);


            }
        }

        public static IEnumerable TestCases1C2O
        {
            get
            {
                //Try to open a door with wrong key
                var active = CreateACharacter("Me");
                var passive = (new ObjectsFactory(new LevelInstaller())).CreateObject<IPositionableObject>("RedDoor");
                yield return
                    new TestCaseData(active,
                                     CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey")),
                                     passive, (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectUseOnObjectAction>("Open")).Returns(passive);
                //try to open a door with the right key
                active = CreateACharacter("Me");
                passive = (new ObjectsFactory(new LevelInstaller())).CreateObject<IPositionableObject>("RedDoor");
                yield return
                    new TestCaseData(active,
                                     CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("RedKey")),
                                     passive, (new ObjectsFactory(new LevelInstaller())).CreateObject<ObjectUseOnObjectAction>("Open")).Returns(null);


            }
        }

        public static IEnumerable TestCasesM
        {
            get
            {
                //walk off the margin of the map
                var map = CreateAMap();
                var active = CreateACharacter("Me");
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                var walkNorth = (new ObjectsFactory(new LevelInstaller())).CreateObject<MovementAction>("Walk");
                walkNorth.Direction = Direction.North;
                yield return
                    new TestCaseData(active,
                                     walkNorth, map,null,null).
                        Returns(ActionResultType.OutOfTheMap);

                //run off the margin of the map
                active = CreateACharacter("Me");
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                var runNorth = (new ObjectsFactory(new LevelInstaller())).CreateObject<MovementAction>("Run");
                runNorth.Direction = Direction.North;
                yield return
                    new TestCaseData(active,
                                     runNorth, map,null,null).
                        Returns(ActionResultType.OutOfTheMap);



                //move blocking object

                active = CreateACharacter("Me");
                var blockingObject = (new ObjectsFactory(new LevelInstaller()).CreateObject<IPositionableObject>("RedDoor"));
                blockingObject.MapNumber = 1;
                blockingObject.CellNumber = 3;
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                var runEast = (new ObjectsFactory(new LevelInstaller())).CreateObject<MovementAction>("Run");
                runEast.Direction = Direction.East;
                yield return
                    new TestCaseData(active,
                                     runEast, map, new List<IPositionableObject> {blockingObject},null).
                        Returns(ActionResultType.BlockedByObject);

                //move blocking character

                active = CreateACharacter("Me");
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));
                var blockingCharacter = CreateACharacter("Keyless Child");
                
                var runSouth = (new ObjectsFactory(new LevelInstaller())).CreateObject<MovementAction>("Run");
                runSouth.Direction = Direction.South;
                yield return
                    new TestCaseData(active,
                                     runSouth, map, null, new List<Character>{blockingCharacter}).
                        Returns(ActionResultType.BlockedByCharacter);

                //walk Ok

                active = CreateACharacter("Me");
                CreateToolInHand(active, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("BlueKey"));

                var walkSouth = (new ObjectsFactory(new LevelInstaller())).CreateObject<MovementAction>("Walk");
                walkSouth.Direction = Direction.South;
                yield return
                    new TestCaseData(active,
                                     walkSouth, map, null, new List<Character> { blockingCharacter }).
                        Returns(ActionResultType.Ok);

            }
        }

        public static IEnumerable TestCasesCom
        {
            get
            {
                //cannot talk
                var me = CreateACharacter("Me");
                var keyMaster = CreateACharacter("Key Master");
                yield return new TestCaseData(new CommunicateAction {Message = "Hello!"}, me, keyMaster).Returns("");

                //talk no response
                var keylessChild = CreateACharacter("Keyless Child");
                yield return
                    new TestCaseData(new CommunicateAction {Message = "Hello!"}, me, keylessChild).Returns("Hello!");
            }
        }

        public static IEnumerable TestCasesComD
        {
            get
            {
                //cannot talk
                var me = CreateACharacter("Me");
                var keylessChild = CreateACharacter("Keyless Child");
                yield return new TestCaseData(new CommunicateAction { Message = "Interactive" }, me, keylessChild).Returns(null);
            }
        }

        public static IEnumerable TestCasesComInt
        {
            get
            {
                //ask and receive
                var me = CreateACharacter("Me");
                var keyMaster = CreateACharacter("Key Master");
                CreateToolInHand(keyMaster, (new ObjectsFactory(new LevelInstaller())).CreateObject<ITransactionalObject>("RedKey"));
                keyMaster.Personality.InteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific,GetKMInteractionRules(keyMaster));

                yield return new TestCaseData(new CommunicateAction { Message = "Give me the red key." }, me, keyMaster).Returns("RedKey");
            }
        }

        private static Map CreateAMap()
        {
            //1 2   3
            //4 5   6
            //7 8   9
            return new Map
                       {
                           Number = 1,
                           Cells =
                               new Cell[9]
                                   {
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 0,
                                               NorthCell = 0,
                                               Number = 1,
                                               SouthCell = 4,
                                               TopCell = 0,
                                               EastCell = 2
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 1,
                                               NorthCell = 0,
                                               Number = 2,
                                               SouthCell = 5,
                                               TopCell = 0,
                                               EastCell = 3
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 2,
                                               NorthCell = 0,
                                               Number = 3,
                                               SouthCell = 6,
                                               TopCell = 0,
                                               EastCell = 0
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 0,
                                               NorthCell = 1,
                                               Number = 4,
                                               SouthCell = 7,
                                               TopCell = 0,
                                               EastCell = 5
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 4,
                                               NorthCell = 2,
                                               Number = 5,
                                               SouthCell = 8,
                                               TopCell = 0,
                                               EastCell = 6
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 5,
                                               NorthCell = 3,
                                               Number = 6,
                                               SouthCell = 9,
                                               TopCell = 0,
                                               EastCell = 0
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 0,
                                               NorthCell = 4,
                                               Number = 7,
                                               SouthCell = 0,
                                               TopCell = 0,
                                               EastCell = 8
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 7,
                                               NorthCell = 5,
                                               Number = 8,
                                               SouthCell = 0,
                                               TopCell = 0,
                                               EastCell = 9
                                           },
                                       new Cell
                                           {
                                               BottomCell = 0,
                                               WestCell = 8,
                                               NorthCell = 6,
                                               Number = 9,
                                               SouthCell = 0,
                                               TopCell = 0,
                                               EastCell = 0
                                           }
                                   }
                       };
        }

        private static Tool CreateToolInHand(Character character, IPositionableObject toolObject)
        {
            character.TryPerformAction((new ObjectsFactory(new LevelInstaller())).CreateObject <OneObjectAction>("Pick"),toolObject);
            return ((Tool) toolObject);
        }

        private static Character CreateACharacter(string name, bool fullToolInventory=false, int currencyAvailable=50)
        {
            if(name=="Me")
            {
                var me = (new ObjectsFactory(new LevelInstaller())).CreateObject<IPlayer>("Me");
                me.ToolInventory.MaximumCapacity = (fullToolInventory) ? 0 : 2;
                me.WeaponInventory.MaximumCapacity = 2;
                me.Walet.MaxCapacity = 200;
                me.Walet.CurrentCapacity = currencyAvailable;
                me.RightHandObject.Objects.Clear();
                me.LeftHandObject.Objects.Clear();
                me.CellNumber = 1;
                me.Personality.InteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific, GetMeInteractionRules());
                return me as Character;
            }
            if (name == "Key Master")
            {
                var kmCharacter = (new ObjectsFactory(new LevelInstaller())).CreateObject<Character>("Key Master");
                kmCharacter.ToolInventory.MaximumCapacity = (fullToolInventory) ? 0 : 2;
                kmCharacter.WeaponInventory.MaximumCapacity = 2;
                kmCharacter.RightHandObject.Objects.Clear();
                kmCharacter.LeftHandObject.Objects.Clear();
                return kmCharacter;
            }
            if (name == "Keyless Child")
            {
                var kcCharacter = (new ObjectsFactory(new LevelInstaller())).CreateObject<Character>("Keyless Child");
                kcCharacter.ToolInventory.MaximumCapacity = (fullToolInventory) ? 0 : 2;
                kcCharacter.WeaponInventory.MaximumCapacity = 2;
                kcCharacter.Walet.MaxCapacity = 200;
                kcCharacter.Walet.CurrentCapacity = 5;
                kcCharacter.RightHandObject.Objects.Clear();
                kcCharacter.LeftHandObject.Objects.Clear();
                kcCharacter.Personality.InteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific,GetKCInteractionRules());
                kcCharacter.MapNumber = 1;
                kcCharacter.CellNumber = 7;
                return kcCharacter;
            }
            if (name == "Shop Keeper")
            {
                var skCharacter = (new ObjectsFactory(new LevelInstaller())).CreateObject<Character>("Shop Keeper");
                skCharacter.ToolInventory.MaximumCapacity = (fullToolInventory) ? 0 : 2;
                skCharacter.WeaponInventory.MaximumCapacity = 2;
                skCharacter.Walet.MaxCapacity=2000;
                skCharacter.Walet.CurrentCapacity= 50;
                skCharacter.RightHandObject.Objects.Clear();
                skCharacter.LeftHandObject.Objects.Clear();
                return skCharacter;
            }

            return null;
        }

        private static ActionReaction[] GetKMInteractionRules(Character kmCharacter)
        {
            return new ActionReaction[]
                       {
                           new ActionReaction
                               {
                                   Action = new CommunicateAction {Message = "Give me the red key."},
                                   Reaction = new GiveTo {UseableObject = kmCharacter.RightHandObject.Objects[0]}
                               }
                       };
        }

        private static ActionReaction[] GetKCInteractionRules()
        {
            return new ActionReaction[]
                       {
                           new ActionReaction
                               {
                                   Action = new CommunicateAction {Message = "Interactive"},
                                   Reaction = new CommunicateAction {Message = "Random Response 1"}
                               },
                           new ActionReaction
                               {
                                   Action = new CommunicateAction {Message = "Interactive 1"},
                                   Reaction = new CommunicateAction {Message = "Random Response 2"}
                               }

                       };
        }

        private static ActionReaction[] GetMeInteractionRules()
        {
            return new ActionReaction[]
                       {
                           new ActionReaction
                               {
                                   Action = new CommunicateAction {Message = "Random Response 1"},
                                   Reaction = new CommunicateAction {Message = "Interactive 2"}
                               }
                       };
        }

        private static GameAction FakeInteractiveSelector(GameAction[] options, ICharacter targetCharacter)
        {
            Assert.True(options.Any());
            return options.First();
        }
    }
}
