using System.Collections.Generic;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.BulkTemplatesTests
{
    [TestFixture]
    public class LoadingActionTemplatesInObjectTests
    {
        [Test]
        public void LoadObjectsDefaultActionTemplates()
        {
            var tool = new Tool {Name=new Noun{Main="TestTool",Narrator="test tool"}};
            Assert.IsEmpty(tool.AllActions);
        }

        [Test]
        public void SaveAModifiedObject()
        {
            var tool = new Tool
                           {
                               Name = new Noun {Main = "TestTool", Narrator = "test tool"},
                               ObjectTypeId = "Tool"
                           };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    CurrentPerformer = new OneObjectActionTemplatePerformer(),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                                    Past = "exploded",
                                    Present = "explode"
                                }
                    }
                );
            var saveResult = tool.Save();
            Assert.AreEqual("[{\"Category\":\"OneObjectActionTemplate\",\"Name\":{\"Identifier\":\"Explode\",\"Present\":\"explode\",\"Past\":\"exploded\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}]", saveResult.ActionTemplates);
            Assert.AreEqual("{\"BuyPrice\":0,\"SellPrice\":0,\"ObjectTypeId\":\"Tool\",\"Name\":{\"Main\":\"TestTool\",\"Narrator\":\"test tool\"},\"CellNumber\":0,\"BlockMovement\":false,\"ObjectCategory\":1,\"CloseUpMessage\":null}",saveResult.Core);
        }

        [Test]
        public void LoadAModifiedObject()
        {
            var expected = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId="Tool"
            };
            expected.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    CurrentPerformer = new OneObjectActionTemplatePerformer(),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                                    Past = "exploded",
                                    Present = "explode"
                                }
                    }
                );

            var actual =
                Tool.Load<Tool>(new BasePackage
                {
                    Core =
                        "{\"BuyPrice\":0,\"SellPrice\":0,\"ObjectTypeId\":\"Tool\",\"Name\":{\"Main\":\"TestTool\",\"Narrator\":\"test tool\"},\"CellNumber\":0,\"BlockMovement\":false,\"ObjectCategory\":1,\"CloseUpMessage\":null}",
                    ActionTemplates = "[{\"Category\":\"OneObjectActionTemplate\",\"Name\":{\"Identifier\":\"Explode\",\"Present\":\"explode\",\"Past\":\"exploded\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}]"
                });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions["CloseInventory"].ToString(), actual.AllActions["CloseInventory"].ToString());
            Assert.AreEqual(typeof(OneObjectActionTemplate), actual.AllActions["CloseInventory"].GetType());
        }
    }
}
