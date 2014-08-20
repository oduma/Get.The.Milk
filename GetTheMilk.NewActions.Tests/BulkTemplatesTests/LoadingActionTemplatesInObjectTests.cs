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
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                                    Past = "exploded",
                                    Present = "explode"
                                }
                    }
                );
            var saveResult = tool.Save();
            Assert.AreEqual("{\"Explode\":{\"Name\":{\"UniqueId\":\"Explode\",\"Present\":\"explode\",\"Past\":\"exploded\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}", saveResult.ActionTemplates);
            Assert.AreEqual("{\"BuyPrice\":0,\"SellPrice\":0,\"ObjectTypeId\":\"Tool\",\"Name\":{\"Main\":\"TestTool\",\"Narrator\":\"test tool\",\"Description\":null},\"CellNumber\":0,\"BlockMovement\":false,\"ObjectCategory\":1}", saveResult.Core);
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
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
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
                        "{\"BuyPrice\":0,\"SellPrice\":0,\"ObjectTypeId\":\"Tool\",\"Name\":{\"Main\":\"TestTool\",\"Narrator\":\"test tool\",\"Description\":null},\"CellNumber\":0,\"BlockMovement\":false,\"ObjectCategory\":1}",
                    ActionTemplates = "{\"Explode\":{\"Name\":{\"UniqueId\":\"Explode\",\"Present\":\"explode\",\"Past\":\"exploded\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}"
                });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions["Explode"].ToString(), actual.AllActions["Explode"].ToString());
            Assert.AreEqual(typeof(OneObjectActionTemplate), actual.AllActions["Explode"].GetType());
        }
    }
}
