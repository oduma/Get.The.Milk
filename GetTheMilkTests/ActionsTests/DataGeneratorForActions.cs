using System.Collections;
using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    public class DataGeneratorForActions
    {

        public static IEnumerable TestCases1C1O
        {
            get
            {
                Level _level = Level.Create(1);

                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                //Kick a wall (nothing happens)
                yield return
                    new TestCaseData(active,
                                     new Kick(),
                                     _level.Objects.Objects.FirstOrDefault(o=>o.Name.Main=="Wall"))
                        .Returns(_level.Objects);

                //Pick a key, key in hand
                active = new Player();
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                yield return
                    new TestCaseData(active,
                                     new Keep(),
                                     _level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key")).Returns(active.Inventory);

                _level = Level.Create(1);

                active = new Player();
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory = new Inventory { InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 0 };
                yield return
                    new TestCaseData(active,
                                     new Keep(),
                                     _level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key")).Returns(_level.Objects);
            }
        }

        public static IEnumerable TestCases2C1O
        {
            get
            {
                Level _level = Level.Create(1);

                //Give a key to a character that doesn't want it (nothing happens)
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var passive = _level.Characters.Characters.FirstOrDefault(c=>c.ObjectTypeId=="NPCFoe");
                var action = new GiveTo();
                action.UseableObject = passive.Inventory.Objects[0];
                active.Inventory.Add(_level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                yield return
                    new TestCaseData(active, passive, action).Returns(
                        passive.Inventory);

                //Give a key to a character that wants it (key changes owners)
                passive = _level.Characters.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
                action= new GiveTo();
                action.UseableObject = passive.Inventory.Objects[0];
                yield return
                    new TestCaseData(passive, active, action).Returns(
                        active.Inventory);

            }
        }

        public static IEnumerable TestCases2CC
        {
            get
            {
                Level _level = Level.Create(1);

                //not enough money
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var passive = _level.Characters.Characters.FirstOrDefault(c=>c.ObjectTypeId=="NPCFriendly");

                yield return new TestCaseData(active, passive, 100, TransactionType.Debit).Returns(
                    active.Walet.CurrentCapacity);

                yield return new TestCaseData(active, passive, 10, TransactionType.Debit).Returns(
                    10);

            }
        }

        public static IEnumerable TestCases2C1OC
        {
            get
            {
                Level _level = Level.Create(1);

                //Try to buy a key from a character not enough money (nothing happens)
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Walet.CurrentCapacity = 1;
                var passive = _level.Characters.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"); 

                var buyAction = new Buy();
                buyAction.UseableObject = passive.Inventory.Objects[0];

                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        1);

                //Try to sell a key to a character not enough money (nothing happens)

                var sellAction = new Sell();
                sellAction.UseableObject = passive.Inventory.Objects[0];
                yield return
                    new TestCaseData(passive, active, sellAction).Returns(
                        200);

                //Try to buy a key no room in your inventory

                active = new Player();

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.MaximumCapacity = 0;
                active.Walet.CurrentCapacity = 50;
                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        50);
                //Succesfully buy a skrew driver
                active= new Player();

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.MaximumCapacity = 10;
                active.Walet.CurrentCapacity = 200;

                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        190);

            }
        }

        public static IEnumerable TestCases1C2O
        {
            get
            {
                Level _level = Level.Create(1);

                //Try to open a door with wrong key
                var active = new Player(new StubedTextBasedInteractivity());

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var passive = _level.Objects.Objects.FirstOrDefault(o=>o.Name.Main=="Red Door");
                var blueKey = new Tool
                                  {
                                      Name = new Noun {Main = "Blue Key", Narrator = "the Blue Key"},
                                      AllowsAction = (a) =>
                                                         {
                                                             return (a.Name.Infinitive == "To Open");
                                                         },
                                      AllowsIndirectAction = (a, o) =>
                                                                 {
                                                                     return
                                                                         false;
                                                                 }
                                  };
                active.Inventory.Add(blueKey);
                yield return
                    new TestCaseData(active,
                                     passive,
                                     new Open()).Returns(passive);

                //try to open a door with the right key
                active = new Player(new StubedTextBasedInteractivity());

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.Add(_level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                yield return
                    new TestCaseData(active,
                                     passive,
                                     new Open()).Returns(null);


            }
        }

        public static IEnumerable TestCasesM
        {
            get
            {
                Level _level = Level.Create(1);

                //walk off the margin of the map
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.Add(_level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                var walkNorth = new Walk();
                walkNorth.Direction = Direction.North;
                yield return
                    new TestCaseData(active,_level,
                                     walkNorth, _level.Maps[0], null, null).
                        Returns(ActionResultType.OutOfTheMap);

                //run off the margin of the map
                var runNorth = new Run();
                runNorth.Direction = Direction.North;
                yield return
                    new TestCaseData(active, _level,
                                     runNorth, _level.Maps[0], null, null).
                        Returns(ActionResultType.OutOfTheMap);



                //move blocking object

                var runEast = new Run();
                runEast.Direction = Direction.East;
                yield return
                    new TestCaseData(active, _level,
                                     runEast, _level.Maps[0], _level.Objects.Objects, null).
                        Returns(ActionResultType.Blocked);

                //move blocking character
                _level.Characters.Characters[0].CellNumber = 7;
                var runSouth = new Run();
                runSouth.Direction = Direction.South;
                yield return
                    new TestCaseData(active, _level,
                                     runSouth, _level.Maps[0], null, _level.Characters.Characters).
                        Returns(ActionResultType.Blocked);

                //walk Ok

                var walkSouth = new Walk();
                walkSouth.Direction = Direction.South;
                yield return
                    new TestCaseData(active, _level,
                                     walkSouth, _level.Maps[0], null, _level.Characters.Characters).
                        Returns(ActionResultType.Ok);

            }
        }
    }
}
