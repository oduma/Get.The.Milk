using System.Collections;
using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    public class DataGeneratorForActions
    {

        private static readonly Level Level = Level.Create(1);
        public static IEnumerable TestCases1C1O
        {
            get
            {
                //Kick a wall (nothing happens)
                yield return
                    new TestCaseData(new Player(),
                                     new Kick(),
                                     Level.Objects.Objects.FirstOrDefault(o=>o.Name.Main=="Wall"))
                        .Returns(null);
                //Pick a key, key in hand
                var meCharacter = new Player();
                yield return
                    new TestCaseData(meCharacter,
                                     new Keep(),
                                     Level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key")).Returns(meCharacter.Inventory);
                meCharacter = new Player();
                meCharacter.Inventory = new Inventory
                                            {InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 0};
                yield return
                    new TestCaseData(meCharacter,
                                     new Keep(),
                                     Level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key")).Returns(meCharacter.Inventory);
            }
        }

        public static IEnumerable TestCases2C1O
        {
            get
            {
                //Give a key to a character that doesn't want it (nothing happens)
                var active = new Player();
                var passive = Level.Characters.Characters.FirstOrDefault(c=>c.ObjectTypeId=="NPCFoey");
                var action = new GiveTo();
                active.Inventory.Add(Level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                yield return
                    new TestCaseData(active, passive, action).Returns(
                        active.Inventory);

                //Give a key to a character that wants it (key changes owners)
                passive = Level.Characters.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
                action = new GiveTo();
                yield return
                    new TestCaseData(active, passive, action).Returns(
                        passive.Inventory);

            }
        }

        public static IEnumerable TestCases2CC
        {
            get
            {
                //not enough money
                var active = new Player();
                var passive = Level.Characters.Characters.FirstOrDefault(c=>c.ObjectTypeId=="NPCFriendly");

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
                var active = new Player();
                active.Walet.CurrentCapacity = 1;
                var passive = Level.Characters.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"); 

                var buyAction = new Buy();

                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        1);

                //Try to sell a key to a character not enough money (nothing happens)

                var sellAction = new Sell();
                yield return
                    new TestCaseData(passive, active, sellAction).Returns(
                        50);

                //Try to buy a key no room in your inventory

                active.Inventory.MaximumCapacity = 0;

                buyAction = new Buy();
                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        50);
                //Succesfully buy a skrew driver
                active.Inventory.MaximumCapacity = 10;
                active.Walet.CurrentCapacity = 200;

                buyAction = new Buy();
                buyAction.UseableObject = passive.Inventory.Objects.FirstOrDefault();
                yield return
                    new TestCaseData(active, passive, buyAction).Returns(
                        45);

            }
        }

        public static IEnumerable TestCases1C2O
        {
            get
            {
                //Try to open a door with wrong key
                var active = new Player();
                var passive = Level.Objects.Objects.Select(o=>o.Name.Main="Red Door");
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

                active.Inventory.Add(Level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key"));
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
                //walk off the margin of the map
                var active = new Player();
                active.Inventory.Add(Level.Objects.Objects.FirstOrDefault(o => o.Name.Main == "Red Key"));
                var walkNorth = new Walk();
                walkNorth.Direction = Direction.North;
                yield return
                    new TestCaseData(active,Level,
                                     walkNorth, Level.Maps[0], null, null).
                        Returns(ActionResultType.OutOfTheMap);

                //run off the margin of the map
                var runNorth = new Run();
                runNorth.Direction = Direction.North;
                yield return
                    new TestCaseData(active, Level,
                                     runNorth, Level.Maps[0], null, null).
                        Returns(ActionResultType.OutOfTheMap);



                //move blocking object

                var runEast = new Run();
                runEast.Direction = Direction.East;
                yield return
                    new TestCaseData(active, Level,
                                     runEast, Level.Maps[0], Level.Objects.Objects, null).
                        Returns(ActionResultType.Blocked);

                //move blocking character
                Level.Characters.Characters[0].CellNumber = 7;
                var runSouth = new Run();
                runSouth.Direction = Direction.South;
                yield return
                    new TestCaseData(active, Level,
                                     runSouth, Level.Maps[0], null, Level.Characters.Characters).
                        Returns(ActionResultType.Blocked);

                //walk Ok

                var walkSouth = new Walk();
                walkSouth.Direction = Direction.South;
                yield return
                    new TestCaseData(active, Level,
                                     walkSouth, Level.Maps[0], null, Level.Characters.Characters).
                        Returns(ActionResultType.Ok);

            }
        }
    }
}
