using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ObjectUseOnObjectActionTemplateTests
    {
        [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate();

            Assert.AreEqual("ObjectUseOnObjectActionTemplate-Default No Target Object Assigned using No Active Object Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType = typeof(ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Open", Past = "opened", Present = "open" },
                DestroyActiveObject=true,
                DestroyTargetObject=true
            };

            Assert.AreEqual("open No Target Object Assigned using No Active Object Assigned", defaultActionTemplate.ToString());

        }
        [Test]
        public void SerializeEmptyDefaultActionTemplate()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate();
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"DestroyActiveObject\":false,\"DestroyTargetObject\":false,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"Category\":\"ObjectUseOnObjectActionTemplate\",\"Name\":{\"Identifier\":\"Default\",\"Present\":null,\"Past\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType = typeof(ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Defuse", Past = "defused", Present = "defuse" },
                ChanceOfSuccess=ChanceOfSuccess.Small,
                PercentOfHealthFailurePenalty=20,
                DestroyTargetObject=true
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"DestroyActiveObject\":false,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":25,\"PercentOfHealthFailurePenalty\":20,\"Category\":\"ObjectUseOnObjectActionTemplate\",\"Name\":{\"Identifier\":\"Defuse\",\"Present\":\"defuse\",\"Past\":\"defused\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new ObjectUseOnObjectActionTemplate();
            var result =
                JsonConvert.DeserializeObject<ObjectUseOnObjectActionTemplate>(
                    "{\"DestroyActiveObject\":false,\"DestroyTargetObject\":false,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"Name\":{\"Identifier\":\"Default\",\"Present\":null,\"Past\":null,\"Category\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
            Assert.False(result.DestroyActiveObject);
            Assert.False(result.DestroyTargetObject);

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new ObjectUseOnObjectActionTemplate
            {
                PerformerType = typeof(ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Defuse", Past = "defused", Present = "defuse" },
                ChanceOfSuccess = ChanceOfSuccess.Small,
                PercentOfHealthFailurePenalty = 20,
                DestroyTargetObject = true
            };
            var result =
                JsonConvert.DeserializeObject<ObjectUseOnObjectActionTemplate>(
                    "{\"DestroyActiveObject\":false,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":25,\"PercentOfHealthFailurePenalty\":20,\"Name\":{\"Identifier\":\"Defuse\",\"Present\":\"defuse\",\"Past\":\"defused\",\"Category\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
            Assert.AreEqual(expected.PercentOfHealthFailurePenalty, result.PercentOfHealthFailurePenalty);
            Assert.AreEqual(expected.ChanceOfSuccess, result.ChanceOfSuccess);
            Assert.AreEqual(expected.DestroyTargetObject, result.DestroyTargetObject);

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType = typeof(ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Defuse", Past = "defused", Present = "defuse" },
                ChanceOfSuccess = ChanceOfSuccess.Small,
                PercentOfHealthFailurePenalty = 20,
                DestroyTargetObject = true
            }; 
            var actual = defaultActionTemplate.Clone() as ObjectUseOnObjectActionTemplate;
            defaultActionTemplate.ActiveCharacter = new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual(defaultActionTemplate.ToString(), actual.ToString());
        }

    }
}
