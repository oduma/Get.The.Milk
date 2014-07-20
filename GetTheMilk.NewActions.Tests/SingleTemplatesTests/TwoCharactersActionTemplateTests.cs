using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class TwoCharactersActionTemplateTests
    {
        [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate();

            Assert.AreEqual("TwoCharactersActionTemplate-Default Target Character Not Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(AttackActionPerformer),
                Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" }
            };

            Assert.AreEqual("attack Target Character Not Assigned", defaultActionTemplate.ToString());

            defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message="hello"
            };

            Assert.AreEqual("say hello to Target Character Not Assigned", defaultActionTemplate.ToString());



        }
        [Test]
        public void SerializeEmptyDefaultActionTemplate()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate();
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Default\",\"Present\":null,\"Past\":null},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message = "hello"
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"Message\":\"hello\",\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Communicate\",\"Present\":\"say\",\"Past\":\"said\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new TwoCharactersActionTemplate();
            var result =
                JsonConvert.DeserializeObject<TwoCharactersActionTemplate>(
                    "{\"Message\":null,\"Name\":{\"Identifier\":\"Default\",\"Present\":null,\"Past\":null,\"Category\":null},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message = "hello"
            };
            var result =
                JsonConvert.DeserializeObject<TwoCharactersActionTemplate>(
                    "{\"Message\":\"hello\",\"Name\":{\"Identifier\":\"Communicate\",\"Present\":\"say\",\"Past\":\"said\",\"Category\":null},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message = "hello"
            };

            var actual = defaultActionTemplate.Clone() as TwoCharactersActionTemplate;
            defaultActionTemplate.ActiveCharacter = new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual("hello",actual.Message);
            Assert.AreEqual(defaultActionTemplate.ToString(), actual.ToString());
        }
    }
}
