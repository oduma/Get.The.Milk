using System.Collections;
using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using GetTheMilk.Test.Level.Level1Characters;
using GetTheMilk.TestLevel;
using GetTheMilkTests.ActionsTests;
using NUnit.Framework;
using RedDoor = GetTheMilk.Test.Level.Level1Objects.RedDoor;
using Wall = GetTheMilk.Test.Level.Level1Objects.Wall;

namespace GetTheMilkTests.TranslatorTests
{
    public class DataGeneratorForTranslator
    {
        public static IEnumerable TestCasesForTranslatorMovement
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult
                            {ResultType = ActionResultType.Ok, ForAction = new Walk {Direction = Direction.South}},
                        Player.GetNewInstance()).Returns("You walked South.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Run { Direction = Direction.West } },
                        Player.GetNewInstance()).Returns("You ran West.");
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new EnterLevel { LevelNo=2 } },
                        Player.GetNewInstance()).Returns("You entered level 2.");

            }
        }
        public static IEnumerable TestCasesForTranslatorMovementError
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OriginNotOnTheMap, ForAction = new Walk { Direction = Direction.South } },
                        Player.GetNewInstance()).Returns("Error You cannot walk.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OriginNotOnTheMap, ForAction = new Run { Direction = Direction.West } },
                        Player.GetNewInstance()).Returns("Error You cannot run.");

            }
        }
        public static IEnumerable TestCasesForTranslatorMovementOutOfTheWorld
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OutOfTheMap, ForAction = new Walk { Direction = Direction.South } },
                        Player.GetNewInstance()).Returns("You tried to walk South but the world has limits.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OutOfTheMap, ForAction = new Run { Direction = Direction.West } },
                        Player.GetNewInstance()).Returns("You tried to run West but the world has limits.");

            }
        }
        public static IEnumerable TestCasesForTranslatorMovementBlockedByObjs
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult
                            {
                                ResultType = ActionResultType.BlockedByObject,
                                ForAction = new Walk {Direction = Direction.South},
                                ExtraData =
                                    new MovementActionExtraData
                                        {
                                            ObjectsBlocking =
                                                new IPositionableObject[] {new RedDoor(), new Wall(), new Window()}
                                        }
                            },
                        Player.GetNewInstance()).Returns("You tried to walk South but is impossible, blocked by a red door, wall and window.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.BlockedByObject,
                            ForAction = new Run { Direction = Direction.West },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =
                                        new IPositionableObject[] { new RedDoor(), new Wall(), new Window() }
                                }
                        },
                        Player.GetNewInstance()).Returns("You tried to run West but is impossible, blocked by a red door, wall and window.");
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.BlockedByObject, ForAction = new EnterLevel { LevelNo = 2 },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =
                                        new IPositionableObject[] { new RedDoor(), new Wall(), new Window() }
                                } },
                        Player.GetNewInstance()).Returns("You tried to enter level 2 but is impossible, blocked by a red door, wall and window.");

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementBlockedByChars
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.BlockedByCharacter,
                            ForAction = new Walk { Direction = Direction.South },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =
                                        new ICharacter[] { new FighterCharacter(), new ShopKeeperCharacter() }
                                }
                        },
                        Player.GetNewInstance()).Returns("You tried to walk South but the Baddie and John the Shop Keeper is on the way.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.BlockedByCharacter,
                            ForAction = new Run { Direction = Direction.West },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =
                                        new ICharacter[] { new FighterCharacter(), new ShopKeeperCharacter() }
                                }
                        },
                        Player.GetNewInstance()).Returns("You tried to run West but the Baddie and John the Shop Keeper is on the way.");
                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.BlockedByCharacter,
                            ForAction = new EnterLevel { LevelNo = 2 },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =
                                        new ICharacter[] { new FighterCharacter(),new ShopKeeperCharacter()   }
                                }
                        },
                        Player.GetNewInstance()).Returns("You tried to enter level 2 but the Baddie and John the Shop Keeper is on the way.");

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementNoActiveCharacter
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Walk { Direction = Direction.South } }).Returns(GameSettings.TranslatorErrorMessage);

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementWrongAction
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Kill () },
                        Player.GetNewInstance()).Returns(GameSettings.TranslatorErrorMessage);
            }
        }

        public static IEnumerable TestCasesForTranslatorMovementExtraData
        {
            get
            {
                var player = Player.GetNewInstance();
                player.CellNumber = 1;
                player.MapNumber = 1;
                var redDoor = new RedDoor();
                redDoor.CellNumber = 2;
                redDoor.MapNumber = 1;
                var wall = new Wall();
                wall.CellNumber = 4;
                wall.MapNumber = 1;

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsInRange = 
                                        new IPositionableObject[] { redDoor,wall }
                                }
                        },
                        player, new TestLevel1()).Returns("Looking East There is a red door in the distance, or is it a wall?.\r\nLooking South You see a wall.");

                //yield return
                //    new TestCaseData(
                //        new ActionResult
                //        {
                //            ResultType = ActionResultType.BlockedByObject,
                //            ForAction = new Run { Direction = Direction.West },
                //            ExtraData =
                //                new MovementActionExtraData
                //                {
                //                    ObjectsBlocking =
                //                        new IPositionableObject[] { new RedDoor(), new Wall(), new Window() }
                //                }
                //        },
                //        Player.GetNewInstance()).Returns("You tried to run West but is impossible, blocked by a red door, wall and window.");
                //yield return
                //    new TestCaseData(
                //        new ActionResult
                //        {
                //            ResultType = ActionResultType.BlockedByObject,
                //            ForAction = new EnterLevel { LevelNo = 2 },
                //            ExtraData =
                //                new MovementActionExtraData
                //                {
                //                    ObjectsBlocking =
                //                        new IPositionableObject[] { new RedDoor(), new Wall(), new Window() }
                //                }
                //        },
                //        Player.GetNewInstance()).Returns("You tried to enter level 2 but is impossible, blocked by a red door, wall and window.");

            }
        }


    }
}
