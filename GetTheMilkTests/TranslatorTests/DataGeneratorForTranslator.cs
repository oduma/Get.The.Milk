﻿using System.Collections;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Settings;
using GetTheMilkTests.ActionsTests;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    public class DataGeneratorForTranslator
    {
        private static readonly Level Level = Level.Create(1);

        public static IEnumerable TestCasesForTranslatorMovement
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult
                            {ResultType = ActionResultType.Ok, ForAction = new Walk {Direction = Direction.South}},
                        active).Returns("You walked South.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Run { Direction = Direction.West } },
                        active).Returns("You ran West.");
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new EnterLevel { LevelNo=2 } },
                        active).Returns("You entered level 2.");

            }
        }
        public static IEnumerable TestCasesForTranslatorMovementError
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OriginNotOnTheMap, ForAction = new Walk { Direction = Direction.South } },
                        active).Returns("Error You cannot walk.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OriginNotOnTheMap, ForAction = new Run { Direction = Direction.West } },
                        active).Returns("Error You cannot run.");

            }
        }
        public static IEnumerable TestCasesForTranslatorMovementOutOfTheWorld
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OutOfTheMap, ForAction = new Walk { Direction = Direction.South } },
                        active).Returns("You tried to walk South but the world has limits.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OutOfTheMap, ForAction = new Run { Direction = Direction.West } },
                        active).Returns("You tried to run West but the world has limits.");

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementBlockedByObjs
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult
                            {
                                ResultType = ActionResultType.Blocked,
                                ForAction = new Walk {Direction = Direction.South},
                                ExtraData =
                                    new MovementActionExtraData
                                        {
                                            ObjectsBlocking =Level.Objects.Objects.ToArray()
                                        }
                            },
                        active).Returns("You tried to walk South but is impossible, blocked by a red door, wall and window.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new Run { Direction = Direction.West },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =Level.Objects.Objects.ToArray()
                                }
                        },
                        active).Returns("You tried to run West but is impossible, blocked by a red door, wall and window.");
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Blocked, ForAction = new EnterLevel { LevelNo = 2 },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking =Level.Objects.Objects.ToArray()
                                } },
                        active).Returns("You tried to enter level 2 but is impossible, blocked by a red door, wall and window.");

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementBlockedByChars
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new Walk { Direction = Direction.South },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    CharactersBlocking = Level.Characters.Characters.ToArray()
                                }
                        },
                        active).Returns("You tried to walk South but the Baddie and John the Shop Keeper is on the way.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new Run { Direction = Direction.West },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    CharactersBlocking = Level.Characters.Characters.ToArray()
                                }
                        },
                        active).Returns("You tried to run West but the Baddie and John the Shop Keeper is on the way.");
                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new EnterLevel { LevelNo = 2 },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    CharactersBlocking = Level.Characters.Characters.ToArray()
                                }
                        },
                        active).Returns("You tried to enter level 2 but the Baddie and John the Shop Keeper is on the way.");

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementNoActiveCharacter
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Walk { Direction = Direction.South } }).Returns(GameSettings.GetInstance().TranslatorErrorMessage);

            }
        }

        public static IEnumerable TestCasesForTranslatorMovementWrongAction
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Kill () },
                        active).Returns(GameSettings.GetInstance().TranslatorErrorMessage);
            }
        }

        public static IEnumerable TestCasesForTranslatorMovementExtraData
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;


                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsInRange = Level.Objects.Objects.ToArray()
                                }
                        },
                        active, Level).Returns("Looking East There is a red door in the distance, or is it a wall?.\r\nLooking South You see a wall.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new Run { Direction = Direction.West },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking = Level.Objects.Objects.ToArray()
                                }
                        },
                        active).Returns("You tried to run West but is impossible, blocked by a red door, wall and window.");
                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new EnterLevel { LevelNo = 2 },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking = Level.Objects.Objects.ToArray()
                                }
                        },
                        active).Returns("You tried to enter level 2 but is impossible, blocked by a red door, wall and window.");

            }
        }


    }
}
