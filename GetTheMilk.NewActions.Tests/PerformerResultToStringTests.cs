using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests
{
    [TestFixture]
    public class PerformerResultToStringTests
    {
        #region NoObjectActionTemplates
        [Test]
        public void NotOkWithEmptyNoObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new NoObjectActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to NoObjectActionTemplate but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyNoObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new NoObjectActionTemplate {
                Name = new BaseCommon.Verb { UniqueId = "Laugh", Past = "laughed", Present = "laugh" }, ActiveCharacter = new Player() } };
            Assert.AreEqual("you tried to laugh but couldn't.", actual.ToString());
        }
        [Test]
        public void OkWithEmptyNoObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.Ok, ForAction = new NoObjectActionTemplate() };
            Assert.AreEqual("No Active Character Assigned NoObjectActionTemplate.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyNoObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new NoObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Laugh", Past = "laughed", Present = "laugh" },
                    ActiveCharacter = new Player()
                }
            };
            Assert.AreEqual("you laughed.", actual.ToString());
        }
        #endregion

        #region OneObjectActionTemplates
        [Test]
        public void NotOkWithEmptyOneObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new OneObjectActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to OneObjectActionTemplate No Target Object but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithIncompleteNotEmptyOneObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Kick", Past = "kicked", Present = "kick" },
                    ActiveCharacter = new Player()
                }
            };
            Assert.AreEqual("you tried to kick No Target Object but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithIncompleteNotEmptyOneObjectActionTemplateWithActiveObject()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Explode", Past = "exploded", Present = "explode" },
                    ActiveObject = new Tool { Name = new Noun { Main ="Bomb", Narrator="the bomb"} }
                }
            };
            Assert.AreEqual("the bomb tried to explode No Target Object but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyOneObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Kick", Past = "kicked", Present = "kick" },
                    ActiveCharacter = new Player(),
                    TargetObject = new NonCharacterObject { Name = new Noun { Main="Tyre", Narrator="the tyre"} }
                }
            };
            Assert.AreEqual("you tried to kick the tyre but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyOneObjectActionTemplateWithActiveObject()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Explode", Past = "exploded", Present = "explode" },
                    ActiveObject = new Tool { Name = new Noun { Main = "Bomb", Narrator = "the bomb" } },
                    TargetObject = new Tool { Name = new Noun { Main="SkrewDriver", Narrator ="the skrew driver"} }
                    
                }
            };
            Assert.AreEqual("the bomb tried to explode the skrew driver but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyOneObjectActionTemplateWithActiveObject2()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Explode", Past = "exploded", Present = "explode" },
                    ActiveObject = new Tool { Name = new Noun { Main = "Bomb", Narrator = "the bomb" } },
                    TargetObject = new Tool { Name = new Noun { Main = "SkrewDriver", Narrator = "the skrew driver" } },
                    TargetCharacter= new Player()

                }
            };
            Assert.AreEqual("the bomb tried to explode the skrew driver and you but couldn't.", actual.ToString());
        }
        [Test]
        public void OkWithEmptyOneObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.Ok, ForAction = new OneObjectActionTemplate() };
            Assert.AreEqual("No Active Character Assigned OneObjectActionTemplate No Target Object.", actual.ToString());
        }
        [Test]
        public void OkWithIncompleteNotEmptyOneObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Kick", Past = "kicked", Present = "kick" },
                    ActiveCharacter = new Player()
                }
            };
            Assert.AreEqual("you kicked No Target Object.", actual.ToString());
        }
        [Test]
        public void OkWithIncompleteNotEmptyOneObjectActionTemplateWithActiveObject()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Explode", Past = "exploded", Present = "explode" },
                    ActiveObject = new Tool { Name = new Noun { Main = "Bomb", Narrator = "the bomb" } }
                }
            };
            Assert.AreEqual("the bomb exploded No Target Object.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyOneObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Kick", Past = "kicked", Present = "kick" },
                    ActiveCharacter = new Player(),
                    TargetObject = new NonCharacterObject { Name = new Noun { Main = "Tyre", Narrator = "the tyre" } }
                }
            };
            Assert.AreEqual("you kicked the tyre.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyOneObjectActionTemplateWithActiveObject()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Explode", Past = "exploded", Present = "explode" },
                    ActiveObject = new Tool { Name = new Noun { Main = "Bomb", Narrator = "the bomb" } },
                    TargetObject = new Tool { Name = new Noun { Main = "SkrewDriver", Narrator = "the skrew driver" } }

                }
            };
            Assert.AreEqual("the bomb exploded the skrew driver.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyOneObjectActionTemplateWithActiveObject2()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new OneObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Explode", Past = "exploded", Present = "explode" },
                    ActiveObject = new Tool { Name = new Noun { Main = "Bomb", Narrator = "the bomb" } },
                    TargetObject = new Tool { Name = new Noun { Main = "SkrewDriver", Narrator = "the skrew driver" } },
                    TargetCharacter = new Player()

                }
            };
            Assert.AreEqual("the bomb exploded the skrew driver and you.", actual.ToString());
        }

        #endregion

        #region ObjectUseOnObjectActionTemplates
        [Test]
        public void NotOkWithEmptyObjectUseOnObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new ObjectUseOnObjectActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to ObjectUseOnObjectActionTemplate No Target Object Assigned using No Active Object Assigned but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithIncompleteNotEmptyObjectUseOnObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new ObjectUseOnObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Hammer", Past = "hammered", Present = "hammer" },
                    ActiveCharacter = new Player()
                }
            };
            Assert.AreEqual("you tried to hammer No Target Object Assigned using No Active Object Assigned but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyObjectUseOnObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new ObjectUseOnObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Hammer", Past = "hammered", Present = "hammer" },
                    ActiveCharacter = new Player(),
                    TargetObject = new NonCharacterObject { Name = new Noun { Main = "Nail", Narrator = "the nail" } },
                    ActiveObject = new Tool { Name = new Noun { Main="BigHammer",Narrator="the Big Hammer"} }
                }
            };
            Assert.AreEqual("you tried to hammer the nail using the Big Hammer but couldn't.", actual.ToString());
        }
        [Test]
        public void OkWithEmptyObjectUseOnObjectActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.Ok, ForAction = new ObjectUseOnObjectActionTemplate() };
            Assert.AreEqual("No Active Character Assigned ObjectUseOnObjectActionTemplate No Target Object Assigned using No Active Object Assigned.", actual.ToString());
        }
        [Test]
        public void OkWithIncompleteNotEmptyObjectUseOnObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new ObjectUseOnObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Hammer", Past = "hammered", Present = "hammer" },
                    ActiveCharacter = new Player()
                }
            };
            Assert.AreEqual("you hammered No Target Object Assigned using No Active Object Assigned.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyObjectUseOnObjectActionTemplateWithActiveCharacter()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new ObjectUseOnObjectActionTemplate
                {
                    Name = new BaseCommon.Verb { UniqueId = "Hammer", Past = "hammered", Present = "hammer" },
                    ActiveCharacter = new Player(),
                    TargetObject = new NonCharacterObject { Name = new Noun { Main = "Nail", Narrator = "the nail" } },
                    ActiveObject = new Tool { Name = new Noun { Main = "BigHammer", Narrator = "the Big Hammer" } }
                }
            };
            Assert.AreEqual("you hammered the nail using the Big Hammer.", actual.ToString());
        }
        #endregion


        #region ExposeInventoryActionTemplates
        [Test]
        public void NotOkWithEmptyExposeInventoryActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new ExposeInventoryActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to expose Inventory to No Target Character Assigned but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyExposeInventoryActionTemplateWithSelfInventory()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new ExposeInventoryActionTemplate
                {
                    ActiveCharacter = new Player(),
                    SelfInventory=true
                }
            };
            Assert.AreEqual("you tried to expose Inventory but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithCompleteNotEmptyExposeInventoryActionTemplateWithSelfInventoryPrepareForBattle()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new ExposeInventoryActionTemplate
                {
                    ActiveCharacter = new Player(),
                    SelfInventory = true,
                    FinishActionUniqueId="Attack"
                }
            };
            Assert.AreEqual("you tried to prepare for Battle but couldn't.", actual.ToString());
        }
        [Test]
        public void NotOkWithIncompleteNotEmptyExposeInventoryActionTemplateWithoutSelfInventory()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new ExposeInventoryActionTemplate
                {
                    ActiveCharacter = new Player(),
                    SelfInventory = false,
                    TargetCharacter = new Character { Name = new Noun { Main="Joe", Narrator="ugly Joe"} }
                }
            };
            Assert.AreEqual("you tried to expose Inventory to ugly Joe but couldn't.", actual.ToString());
        }

        [Test]
        public void OkWithEmptyExposeInventoryActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.Ok, ForAction = new ExposeInventoryActionTemplate() };
            Assert.AreEqual("No Active Character Assigned exposed Inventory to No Target Character Assigned.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyExposeInventoryActionTemplateWithSelfInventory()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new ExposeInventoryActionTemplate
                {
                    ActiveCharacter = new Player(),
                    SelfInventory = true
                }
            };
            Assert.AreEqual("you exposed Inventory.", actual.ToString());
        }
        [Test]
        public void OkWithCompleteNotEmptyExposeInventoryActionTemplateWithSelfInventoryPrepareForBattle()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new ExposeInventoryActionTemplate
                {
                    ActiveCharacter = new Player(),
                    SelfInventory = true,
                    FinishActionUniqueId = "Attack"
                }
            };
            Assert.AreEqual("you prepared for Battle.", actual.ToString());
        }

        [Test]
        public void OkWithIncompleteNotEmptyExposeInventoryActionTemplateWithoutSelfInventory()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new ExposeInventoryActionTemplate
                {
                    ActiveCharacter = new Player(),
                    SelfInventory = false,
                    TargetCharacter = new Character { Name = new Noun { Main = "Joe", Narrator = "ugly Joe" } }
                }
            };
            Assert.AreEqual("you exposed Inventory to ugly Joe.", actual.ToString());
        }

        #endregion


        #region MovementActionTemplates
        [Test]
        public void NotOkWithDefaultMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new MovementActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to walk None but couldn't.", actual.ToString());
        }

        [Test]
        public void OriginNotOnTheMapWithDefaultMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.OriginNotOnTheMap, ForAction = new MovementActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to walk None but couldn't. (OriginNotOnTheMap)", actual.ToString());
        }
        [Test]
        public void OutOfTheMapWithDefaultMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.OutOfTheMap, ForAction = new MovementActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to walk None but couldn't. (OutOfTheMap)", actual.ToString());
        }
        [Test]
        public void BlockedWithDefaultMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.Blocked, ForAction = new MovementActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to walk None but couldn't. (Blocked)", actual.ToString());
        }

        [Test]
        public void NotOkWithEnterLevelMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "EnterLevel", Past = "entered level", Present = "enter level" },
                    ActiveCharacter = new Player(),
                    Direction = Navigation.Direction.None,
                    PerformerType = typeof(TeleportActionPerformer)
                }
            };
            Assert.AreEqual("you tried to enter level but couldn't.", actual.ToString());
        }

        [Test]
        public void OriginNotOnTheMapWithEnterLevelMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.OriginNotOnTheMap,
                ForAction = new MovementActionTemplate { 
                    Name = new Verb { UniqueId="EnterLevel",Past="entered level", Present="enter level"},
                    ActiveCharacter= new Player(),Direction=Navigation.Direction.None,
                PerformerType=typeof(TeleportActionPerformer)}
            };
            Assert.AreEqual("you tried to enter level but couldn't. (OriginNotOnTheMap)", actual.ToString());
        }

        [Test]
        public void OutOfTheMapWithEnterLevelMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.OutOfTheMap,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "EnterLevel", Past = "entered level", Present = "enter level" },
                    ActiveCharacter = new Player(),
                    Direction = Navigation.Direction.None,
                    PerformerType = typeof(TeleportActionPerformer)
                }
            };
            Assert.AreEqual("you tried to enter level but couldn't. (OutOfTheMap)", actual.ToString());
        }
        [Test]
        public void BlockedWithEnterLevelMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Blocked,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "EnterLevel", Past = "entered level", Present = "enter level" },
                    ActiveCharacter = new Player(),
                    Direction = Navigation.Direction.None,
                    PerformerType = typeof(TeleportActionPerformer)
                }
            };
            Assert.AreEqual("you tried to enter level but couldn't. (Blocked)", actual.ToString());
        }

        [Test]
        public void BlockedWithNonDefaultMovementActionTemplateWithBlockingObjects()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Blocked,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "Run", Past = "ran", Present = "run" },
                    ActiveCharacter = new Player(),
                    Direction = Navigation.Direction.East,
                    DefaultDistance = 3,
                    PerformerType = typeof(RunActionPerformer)
                },
                ExtraData = new MovementActionTemplateExtraData 
                { 
                    ObjectsBlocking = new Tool[] 
                    { 
                        new Tool 
                        { 
                            Name = new Noun { Main = "Stopper", Narrator = "the stopper" } 
                        }, 
                        new Tool 
                        { 
                            Name = new Noun { Main = "DoorStop", Narrator = "the door stop" } 
                        }, 
                        new Tool 
                        { 
                            Name = new Noun { Main = "PaperWeight", Narrator = "the paper weight" } 
                        } 
                    } 
                }
            };
            Assert.AreEqual("you tried to run East but couldn't. Blocked by the stopper, the door stop and the paper weight.", actual.ToString());
        }

        [Test]
        public void BlockedWithNonDefaultMovementActionTemplateWithBlockingCharacters()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Blocked,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "Run", Past = "ran", Present = "run" },
                    ActiveCharacter = new Player(),
                    Direction = Navigation.Direction.East,
                    DefaultDistance = 3,
                    PerformerType = typeof(RunActionPerformer)
                },
                ExtraData = new MovementActionTemplateExtraData
                {
                    CharactersBlocking = new Character[] 
                    { 
                        new Character 
                        { 
                            Name = new Noun { Main = "Bouncer", Narrator = "the bouncer" } 
                        }, 
                        new Character 
                        { 
                            Name = new Noun { Main = "PoliceMan", Narrator = "the policeman" } 
                        }, 
                        new Character 
                        { 
                            Name = new Noun { Main = "Beggar", Narrator = "the beggar" } 
                        } 
                    }
                }
            };
            Assert.AreEqual("you tried to run East but couldn't. Blocked by the bouncer, the policeman and the beggar.", actual.ToString());
        }
        [Test]
        public void BlockedWithNonDefaultMovementActionTemplateWithBlockingObjectsAndCharacters()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Blocked,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "Run", Past = "ran", Present = "run" },
                    ActiveCharacter = new Player(),
                    Direction = Navigation.Direction.East,
                    DefaultDistance = 3,
                    PerformerType = typeof(RunActionPerformer)
                },
                ExtraData = new MovementActionTemplateExtraData
                {
                    CharactersBlocking = new Character[] 
                    { 
                        new Character 
                        { 
                            Name = new Noun { Main = "Bouncer", Narrator = "the bouncer" } 
                        }, 
                        new Character 
                        { 
                            Name = new Noun { Main = "PoliceMan", Narrator = "the policeman" } 
                        }, 
                        new Character 
                        { 
                            Name = new Noun { Main = "Beggar", Narrator = "the beggar" } 
                        } 
                    },
                    ObjectsBlocking = new Tool[] 
                    { 
                        new Tool 
                        { 
                            Name = new Noun { Main = "Stopper", Narrator = "the stopper" } 
                        }, 
                        new Tool 
                        { 
                            Name = new Noun { Main = "DoorStop", Narrator = "the door stop" } 
                        }, 
                        new Tool 
                        { 
                            Name = new Noun { Main = "PaperWeight", Narrator = "the paper weight" } 
                        } 
                    } 
                }
            };
            Assert.AreEqual("you tried to run East but couldn't. Blocked by the stopper, the door stop, the paper weight, the bouncer, the policeman and the beggar.", actual.ToString());
        }

        [Test]
        public void OkWithDefaultMovementActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { ResultType = Actions.BaseActions.ActionResultType.Ok, ForAction = new MovementActionTemplate() };
            Assert.AreEqual("No Active Character Assigned walked None.", actual.ToString());
        }
        [Test]
        public void OkWithEnterLevelActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult 
            { 
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new MovementActionTemplate { Name = new Verb { UniqueId="EnterLevel", Past="entered level", Present="enter level"},PerformerType=typeof(TeleportActionPerformer), Direction=Navigation.Direction.None } 
            };
            Assert.AreEqual("No Active Character Assigned entered level.", actual.ToString());
        }

        [Test]
        public void OkWithNonDefaultActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new MovementActionTemplate
                {
                    Name = new Verb { UniqueId = "Run", Past = "ran", Present = "run" },
                    PerformerType = typeof(RunActionPerformer),
                    Direction = Navigation.Direction.East,
                    ActiveCharacter = new Player()
                }
            };
            Assert.AreEqual("you ran East.", actual.ToString());
        }

        #endregion

        
        #region TwoCharactersActionTemplates
        [Test]
        public void NotOkWithEmptyTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult { 
                ResultType = Actions.BaseActions.ActionResultType.NotOk, ForAction = new TwoCharactersActionTemplate() };
            Assert.AreEqual("No Active Character Assigned tried to TwoCharactersActionTemplate Target Character Not Assigned but couldn't.", actual.ToString());
        }

        [Test]
        public void OkWithEmptyTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new TwoCharactersActionTemplate()
            };
            Assert.AreEqual("No Active Character Assigned TwoCharactersActionTemplate Target Character Not Assigned.", actual.ToString());
        }

        [Test]
        public void NotOkWithCommunicateTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new TwoCharactersActionTemplate
                {
                    Name = new Verb { UniqueId = "SayYes", Past = "said", Present = "say" },
                    Message = "Hello!",
                    ActiveCharacter = new Player(),
                    TargetCharacter = new Character { Name = new Noun { Main = "World", Narrator = "world" } }
                }
            };
            Assert.AreEqual("you tried to say Hello! to world but couldn't.", actual.ToString());
        }

        [Test]
        public void OkWithCommunicateTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new TwoCharactersActionTemplate
                {
                    Name = new Verb { UniqueId = "SayYes", Past = "said", Present = "say" },
                    Message = "Hello!",
                    ActiveCharacter = new Player(),
                    TargetCharacter = new Character { Name = new Noun { Main = "World", Narrator = "world" } }
                }
            };
            Assert.AreEqual("you said Hello! to world.", actual.ToString());
        }

        [Test]
        public void NotOkWithQuitTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new TwoCharactersActionTemplate
                {
                    Name = new Verb { UniqueId = "Quit", Past = "quited", Present = "quit" },
                    ActiveCharacter = new Player(),
                    TargetCharacter = new Character { Name = new Noun { Main = "Smoker", Narrator = "the smoker" } }
                }
            };
            Assert.AreEqual("you tried to quit the smoker but couldn't.", actual.ToString());
        }

        [Test]
        public void OkWithQuitTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new TwoCharactersActionTemplate
                {
                    Name = new Verb { UniqueId = "Quit", Past = "quited", Present = "quit" },
                    ActiveCharacter = new Player(),
                    TargetCharacter = new Character { Name = new Noun { Main = "Smoker", Narrator = "the smoker" } }
                }
            };
            Assert.AreEqual("you quited the smoker.", actual.ToString());
        }

        [Test]
        public void NotOkWithAttackTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.NotOk,
                ForAction = new TwoCharactersActionTemplate
                {
                    Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" },
                    ActiveCharacter = new Player { ActiveAttackWeapon = new Weapon { Name = new Noun { Main = "Fork", Narrator = "the fork" } } },
                    TargetCharacter = new Character { Name = new Noun { Main = "Villager", Narrator = "the villager" } },
                    PerformerType= typeof(AttackActionPerformer)
                }
            };
            Assert.AreEqual("you tried to attack the villager using the fork but couldn't.", actual.ToString());
        }
        [Test]
        public void OkWithAttackTwoCharactersActionTemplate()
        {
            PerformActionResult actual = new PerformActionResult
            {
                ResultType = Actions.BaseActions.ActionResultType.Ok,
                ForAction = new TwoCharactersActionTemplate
                {
                    Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" },
                    ActiveCharacter = new Player { ActiveAttackWeapon = new Weapon { Name = new Noun { Main = "Fork", Narrator = "the fork" } } },
                    TargetCharacter = new Character { Name = new Noun { Main = "Villager", Narrator = "the villager" } },
                    PerformerType = typeof(AttackActionPerformer)
                }
            };
            Assert.AreEqual("you attacked the villager using the fork.", actual.ToString());
        }
        #endregion


    }
}
