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
                Level _level = Level.Create(0);

                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var targetObject = _level.CurrentMap.Cells[1].AllObjects.FirstOrDefault(o => o.Name.Main == "Wall");
                //Kick a wall (nothing happens)
                yield return
                    new TestCaseData(new Kick { ActiveCharacter = active, TargetObject = targetObject })
                        .Returns(_level.Inventory);

                //Pick a key, key in hand
                active = new Player();
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                yield return
                    new TestCaseData(new Keep { ActiveCharacter = active, TargetCharacter = active, TargetObject=
                                     _level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key")}).Returns(active.Inventory);

                _level = Level.Create(0);

                active = new Player();
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory = new Inventory { InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 0 };
                yield return
                    new TestCaseData(new Keep
                    {
                        ActiveCharacter = active,
                        TargetCharacter = active,
                        TargetObject =
                            _level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key")
                    }).Returns(_level.Inventory);
            }
        }

        public static IEnumerable TestCases2C1O
        {
            get
            {
                Level _level = Level.Create(0);

                //Give a key to a character that doesn't want it (nothing happens)
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var passive = _level.CurrentMap.Cells[8].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFoe");
                var action = new GiveTo();
                action.ActiveCharacter = active;
                action.TargetCharacter = passive;
                action.TargetObject = _level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key");
                active.Inventory.Add(_level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                yield return
                    new TestCaseData(action).Returns(
                        active.Inventory);

                //Give a key that the active character doesn have it. Nothing happens
                _level = Level.Create(0);

                active = new Player();


                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                passive = _level.CurrentMap.Cells[2].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");

                action = new GiveTo();
                action.ActiveCharacter = active;
                action.TargetCharacter = passive;
                action.TargetObject = _level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key");
                yield return
                    new TestCaseData(action).Returns(
                        _level.Inventory);

                //Give a key to a character that wants it (key changes owners)
                _level = Level.Create(0);

                active = new Player();


                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                passive = _level.CurrentMap.Cells[2].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
                action = new GiveTo();
                action.TargetObject = _level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key");
                active.Inventory.Add(_level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                action.ActiveCharacter = active;
                action.TargetCharacter = passive;
                yield return
                    new TestCaseData(action).Returns(
                        active.Inventory);

            }
        }

        public static IEnumerable TestCases2CC
        {
            get
            {
                Level _level = Level.Create(0);

                //enough money
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var passive = _level.CurrentMap.Cells[2].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");

                yield return new TestCaseData(active, passive, 100, TransactionType.Payed).Returns(
                    active.Walet.CurrentCapacity);

                _level = Level.Create(0);

                //not enough money
                active = new Player();

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                passive = _level.CurrentMap.Cells[2].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");

                yield return new TestCaseData(passive, active, 300, TransactionType.Payed).Returns(
                    100);

            }
        }

        public static IEnumerable TestCases2C1OC
        {
            get
            {
                Level _level = Level.Create(0);

                //Try to buy a key from a character not enough money (nothing happens)
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Walet.CurrentCapacity = 1;
                var passive = _level.CurrentMap.Cells[2].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");

                var buyAction = new Buy{ActiveCharacter=active,TargetCharacter=passive,TargetObject= passive.Inventory[0]};

                yield return
                    new TestCaseData(buyAction).Returns(
                        1);

                //Try to buy a key no room in your inventory

                active = new Player();

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.MaximumCapacity = 0;
                active.Walet.CurrentCapacity = 50;
                
                yield return
                    new TestCaseData(buyAction).Returns(
                        1);
                //Succesfully buy a small tool
                active = new Player();

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.MaximumCapacity = 10;
                active.Walet.CurrentCapacity = 200;

                buyAction = new Buy
                            {
                                TargetObject = passive.Inventory.FirstOrDefault(o => o.Name.Main == "Can Opener"),
                                ActiveCharacter = active,
                                TargetCharacter = passive
                            };
                yield return
                    new TestCaseData(buyAction).Returns(
                        196);

            }
        }

        public static IEnumerable TestCases2C1OC1
        {
            get
            {
                Level _level = Level.Create(0);

                //Try to buy a key from a character not enough money (nothing happens)
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Walet.CurrentCapacity = 1;
                var passive = _level.CurrentMap.Cells[2].AllCharacters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");

                //Try to sell a key to a character not enough money (nothing happens)

                var sellAction = new Sell { ActiveCharacter = passive, TargetCharacter = active, TargetObject = passive.Inventory[0] };
                yield return
                    new TestCaseData(sellAction).Returns(
                        100);

            }
        }


        public static IEnumerable TestCases1C2O
        {
            get
            {
                Level _level = Level.Create(0);

                //Try to open a door with wrong key
                //var active = new Player(new StubedTextBasedInteractivity());
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                var passive = _level.CurrentMap.Cells[4].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Door");
                var blueKey = new Tool
                                  {
                                      Name = new Noun { Main = "Blue Key", Narrator = "the Blue Key" },
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
                Open action = new Open {ActiveCharacter = active, ActiveObject = blueKey, TargetObject = passive};
                yield return
                    new TestCaseData(action).Returns(passive);

                //try to open a door with the right key
                //active = new Player(new StubedTextBasedInteractivity());
                active = new Player();

                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                action = new Open { ActiveCharacter = active, ActiveObject = _level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key"), TargetObject = passive };
                yield return
                    new TestCaseData(action).Returns(null);


            }
        }

        public static IEnumerable TestCasesM
        {
            get
            {
                Level _level = Level.Create(0);

                //walk off the margin of the map
                var active = new Player();

                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;

                active.Inventory.Add(_level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                var walkNorth = new Walk();
                walkNorth.Direction = Direction.North;
                walkNorth.ActiveCharacter = active;
                walkNorth.CurrentMap = _level.CurrentMap;
                yield return
                    new TestCaseData(walkNorth).
                        Returns(ActionResultType.OutOfTheMap);

                //run off the margin of the map
                var runNorth = new Run();
                runNorth.Direction = Direction.North;
                runNorth.ActiveCharacter = active;
                runNorth.CurrentMap = _level.CurrentMap;
                yield return
                    new TestCaseData(runNorth).
                        Returns(ActionResultType.OutOfTheMap);



                //move blocking object

                var runEast = new Run();
                runEast.Direction = Direction.East;
                runEast.ActiveCharacter = active;
                runEast.CurrentMap = _level.CurrentMap;
                yield return
                    new TestCaseData(runEast).
                        Returns(ActionResultType.Blocked);

                //move blocking character
                _level.Characters[0].CellNumber = 6;
                var runSouth = new Run();
                runSouth.Direction = Direction.South;
                runSouth.ActiveCharacter = active;
                runSouth.CurrentMap = _level.CurrentMap;
                yield return
                    new TestCaseData(runSouth).
                        Returns(ActionResultType.Blocked);

                //walk Ok
                _level = Level.Create(0);
                active.CellNumber = 0;
                var walkSouth = new Walk();
                walkSouth.Direction = Direction.South;
                walkSouth.ActiveCharacter = active;
                walkSouth.CurrentMap = _level.CurrentMap;
                yield return
                    new TestCaseData(walkSouth).
                        Returns(ActionResultType.Ok);

            }
        }
    }
}
