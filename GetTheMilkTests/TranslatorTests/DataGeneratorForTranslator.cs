using System.Collections;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    public class DataGeneratorForTranslator
    {
        private static readonly Level Level = Level.Create(0);

        public static IEnumerable TestCasesForTranslatorCommunicate
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
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Communicate { ActiveCharacter = active, TargetCharacter = Level.Characters.First(), Message = "Yes, of course" } }).Returns("As John the Shop Keeper left you heard You saying 'Yes, of course'.");
            }
        }

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
                            {ResultType = ActionResultType.Ok, ForAction = new Walk {Direction = Direction.South,ActiveCharacter=active}}).Returns("You walked South.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new Run { Direction = Direction.West,ActiveCharacter=active } }).Returns("You ran West.");
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Ok, ForAction = new EnterLevel { LevelNo=1,CurrentMap=new Map{Parent=new Level{Number=1}},ActiveCharacter=active } }).Returns("You entered level 1.");

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
                Level.Player = null;
                active.EnterLevel(Level);
                active.CellNumber = 20;

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.OriginNotOnTheMap,
                            ForAction = new Walk
                                        {
                                            Direction = Direction.North,
                                            ActiveCharacter =
                                                active
                                        }
                        }).Returns("Error You cannot walk.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.OriginNotOnTheMap,
                            ForAction = new Run {Direction = Direction.West,ActiveCharacter=active}
                            }
        ).Returns("Error You cannot run.");

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
                        new ActionResult { ResultType = ActionResultType.OutOfTheMap, ForAction = new Walk { Direction = Direction.South, ActiveCharacter=active} }).Returns("You tried to walk South but the world has limits.");

                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.OutOfTheMap, ForAction = new Run { Direction = Direction.West,ActiveCharacter=active } }).Returns("You tried to run West but the world has limits.");

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
                                ForAction = new Walk {Direction = Direction.South,ActiveCharacter=active},
                                ExtraData =
                                    new MovementActionExtraData
                                        {
                                            ObjectsBlocking =Level.Inventory
                                        }
                            }).Returns("You tried to walk South but the Red Key, red door, wall and wall are on the way.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new Run { Direction = Direction.West,ActiveCharacter=active },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking = Level.Inventory
                                }
                        }).Returns("You tried to run West but the Red Key, red door, wall and wall are on the way.");
                yield return
                    new TestCaseData(
                        new ActionResult { ResultType = ActionResultType.Blocked, ForAction = new EnterLevel { LevelNo = 2 ,ActiveCharacter=active,CurrentMap=Level.CurrentMap,TargetCell=Level.StartingCell},
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsBlocking = Level.Inventory
                                } }).Returns("You tried to enter level 0 but is impossible, blocked by the Red Key, red door, wall and wall.");

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
                            ForAction = new Walk { Direction = Direction.South ,ActiveCharacter=active},
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    CharactersBlocking = Level.Characters
                                }
                                
                        }).Returns("You tried to walk South but John the Shop Keeper and the Baddie are on the way.");

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new Run { Direction = Direction.West ,ActiveCharacter=active},
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    CharactersBlocking = Level.Characters
                                }
                        }).Returns("You tried to run West but John the Shop Keeper and the Baddie are on the way.");
                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ResultType = ActionResultType.Blocked,
                            ForAction = new EnterLevel { LevelNo = 2,ActiveCharacter=active,CurrentMap=Level.CurrentMap,TargetCell=Level.StartingCell },
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    CharactersBlocking = Level.Characters
                                }
                        }).Returns("You tried to enter level 0 but is impossible, blocked by John the Shop Keeper and the Baddie.");

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


        public static IEnumerable TestCasesForTranslatorMovementExtraData
        {
            get
            {
                var active = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                active.AllowsAction = objAction.AllowsAction;
                active.AllowsIndirectAction = objAction.AllowsIndirectAction;
                Level.Player = null;
                active.EnterLevel(Level);

                yield return
                    new TestCaseData(
                        new ActionResult
                        {
                            ExtraData =
                                new MovementActionExtraData
                                {
                                    ObjectsInRange = Level.CurrentMap.Cells[1].AllObjects.Union(Level.CurrentMap.Cells[3].AllObjects)
                                }
                        },
                        active, Level).Returns("Looking East The wall is solid stone, unpassable for sure..\r\nLooking South the Red Key of Kirna and you wonder how did you knew what it was..");
            }
        }

        public static IEnumerable TestCasesForActionTranslator
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
                        new Keep
                        {
                            TargetObject =
                                Level.Inventory.FirstOrDefault(o => o.ObjectCategory == ObjectCategory.Tool),
                            ActiveCharacter = active
                        }
                        ).Returns("keep the Red Key");
                yield return
                    new TestCaseData(
                        new Kick
                        {
                            TargetObject =
                                Level.Inventory.FirstOrDefault(o => o.ObjectCategory == ObjectCategory.Tool),
                            ActiveCharacter = active
                        }).Returns("kick the Red Key");
                yield return
                    new TestCaseData(
                        new Meet
                        {
                            TargetCharacter =
                                Level.Characters.FirstOrDefault(),
                            ActiveCharacter = active
                        }).Returns("meet John the Shop Keeper");
                yield return
                    new TestCaseData(
                        new InitiateHostilities
                        {
                            TargetCharacter =
                                Level.Characters.FirstOrDefault(),
                            ActiveCharacter = active
                        }).Returns("initiate hostilities John the Shop Keeper");
                yield return
                    new TestCaseData(
                        new Open
                        {
                            TargetObject =
                                Level.Inventory.FirstOrDefault(o => o.ObjectCategory != ObjectCategory.Tool),
                            ActiveObject = Level.Inventory.FirstOrDefault(o => o.ObjectCategory == ObjectCategory.Tool),
                            ActiveCharacter=active
                        }).Returns("open wall using the Red Key");



            }
            
        }

    }
}
