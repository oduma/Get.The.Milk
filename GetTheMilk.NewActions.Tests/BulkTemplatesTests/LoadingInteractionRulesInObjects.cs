using System.Collections.Generic;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.BulkTemplatesTests
{
    [TestFixture]
    public class LoadingInteractionRulesInObjects : BaseTestClass
    {
        [Test]
        public void SaveAnObjectWithModifiedInteractions()
        {
            var tool = new Tool
                           {
                               Name = new Noun {Main = "TestTool", Narrator = "test tool"},
                               ObjectTypeId = "Tool",
                               Interactions =
                                   new SortedList<string, Interaction[]>
                                       {
                                           {
                                               GenericInteractionRulesKeys.AnyCharacter,
                                               new Interaction[]
                                                   {
                                                       new Interaction
                                                           {
                                                               Action =
                                                                   new OneObjectActionTemplate
                                                                       {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Touch",
                                                                                       Past = "touched",
                                                                                       Present = "touch"
                                                                                   }
                                                                       },
                                                               Reaction =
                                                                   new OneObjectActionTemplate
                                                                       {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Laugh",
                                                                                       Past = "laughed",
                                                                                       Present = "laugh"
                                                                                   }
                                                                       }
                                                           }
                                                   }
                                           }
                                       }
                           };
            var saveResult = tool.Save();
            Assert.AreEqual("{\"AnyCharacter\":[{\"Action\":{\"Name\":{\"UniqueId\":\"Touch\",\"Present\":\"touch\",\"Past\":\"touched\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"},\"Reaction\":{\"Name\":{\"UniqueId\":\"Laugh\",\"Present\":\"laugh\",\"Past\":\"laughed\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}]}", saveResult.Interactions);
            Assert.AreEqual("{\"BuyPrice\":0,\"SellPrice\":0,\"ObjectTypeId\":\"Tool\",\"Name\":{\"Main\":\"TestTool\",\"Narrator\":\"test tool\",\"Description\":null},\"CellNumber\":0,\"BlockMovement\":false,\"ObjectCategory\":1}", saveResult.Core);
            Assert.AreEqual("{\"Laugh\":{\"Name\":{\"UniqueId\":\"Laugh\",\"Present\":\"laugh\",\"Past\":\"laughed\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}", saveResult.ActionTemplates);
        }
        [Test]
        public void LoadAnObjectWithModifiedInteractions()
        {
            var expected = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId = "Tool",
                Interactions =
                    new SortedList<string, Interaction[]>
                                       {
                                           {
                                               GenericInteractionRulesKeys.AnyCharacter,
                                               new Interaction[]
                                                   {
                                                       new Interaction
                                                           {
                                                               Action =
                                                                   new OneObjectActionTemplate
                                                                       {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Touch",
                                                                                       Past = "touched",
                                                                                       Present = "touch"
                                                                                   }
                                                                       },
                                                               Reaction =
                                                                   new OneObjectActionTemplate
                                                                       {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Laugh",
                                                                                       Past = "laughed",
                                                                                       Present = "laugh"
                                                                                   }
                                                                       }
                                                           }
                                                   }
                                           }
                                       }
            };
            var actual =
                NonCharacterObject.Load<Tool>(new BasePackage
                                                  {
                                                      ActionTemplates = "{\"Laugh\":{\"Name\":{\"UniqueId\":\"Laugh\",\"Present\":\"laugh\",\"Past\":\"laughed\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}",
                                                      Core =
                                                          "{\"BuyPrice\":0,\"SellPrice\":0,\"ObjectTypeId\":\"Tool\",\"Name\":{\"Main\":\"TestTool\",\"Narrator\":\"test tool\",\"Description\":null},\"CellNumber\":0,\"BlockMovement\":false,\"ObjectCategory\":1}",
                                                      Interactions =
                                                          "{\"AnyCharacter\":[{\"Action\":{\"Name\":{\"UniqueId\":\"Touch\",\"Present\":\"touch\",\"Past\":\"touched\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"},\"Reaction\":{\"Name\":{\"UniqueId\":\"Laugh\",\"Present\":\"laugh\",\"Past\":\"laughed\"},\"Category\":\"OneObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}]}"
                                                  });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Interactions.Count, actual.Interactions.Count);
            Assert.AreEqual(expected.Interactions.Keys[0], actual.Interactions.Keys[0]);
            Assert.AreEqual(expected.Interactions[GenericInteractionRulesKeys.AnyCharacter].Length, actual.Interactions[GenericInteractionRulesKeys.AnyCharacter].Length);
            Assert.AreEqual(expected.Interactions[GenericInteractionRulesKeys.AnyCharacter][0].Action.ToString(), actual.Interactions[GenericInteractionRulesKeys.AnyCharacter][0].Action.ToString());
            Assert.AreEqual(expected.Interactions[GenericInteractionRulesKeys.AnyCharacter][0].Reaction.ToString(), actual.Interactions[GenericInteractionRulesKeys.AnyCharacter][0].Reaction.ToString());

        }
    }
}
