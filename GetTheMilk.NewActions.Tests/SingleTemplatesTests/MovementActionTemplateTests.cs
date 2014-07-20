using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class MovementActionTemplateTests
    {
        [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new MovementActionTemplate();

            Assert.AreEqual("walk", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new MovementActionTemplate
            {
                PerformerType=typeof(RunActionPerformer),
                Name=new Verb{UniqueId="Run",Past="ran", Present="run"},
                DefaultDistance=3
            };

            Assert.AreEqual("run", defaultActionTemplate.ToString());

        }
        [Test]
        public void SerializeEmptyDefaultActionTemplate()
        {
            var defaultActionTemplate = new MovementActionTemplate();
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"TargetCell\":0,\"Direction\":0,\"DefaultDistance\":1,\"CurrentMap\":null,\"Category\":\"MovementActionTemplate\",\"Name\":{\"Identifier\":\"Walk\",\"Present\":\"walk\",\"Past\":\"walked\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new MovementActionTemplate
            {
                PerformerType=typeof(TeleportActionPerformer),
                Name= new Verb{UniqueId="EnterLevel",Past="entered level",Present="enter level"},
                DefaultDistance=0,
                TargetCell=100
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"TargetCell\":100,\"Direction\":0,\"DefaultDistance\":0,\"CurrentMap\":null,\"Category\":\"MovementActionTemplate\",\"Name\":{\"Identifier\":\"EnterLevel\",\"Present\":\"enter level\",\"Past\":\"entered level\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new MovementActionTemplate();
            var result =
                JsonConvert.DeserializeObject<MovementActionTemplate>(
                    "{\"TargetCell\":0,\"Direction\":0,\"DefaultDistance\":1,\"CurrentMap\":null,\"Name\":{\"Identifier\":\"Walk\",\"Present\":\"walk\",\"Past\":\"walked\",\"Category\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
            Assert.AreEqual(expected.DefaultDistance,result.DefaultDistance);

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new MovementActionTemplate
            {
                PerformerType=typeof(TeleportActionPerformer),
                Name = new Verb { UniqueId = "EnterLevel", Past = "entered level", Present = "enter level" },
                DefaultDistance = 0,
                TargetCell = 100
            };
            var result =
                JsonConvert.DeserializeObject<MovementActionTemplate>(
                    "{\"TargetCell\":100,\"Direction\":0,\"DefaultDistance\":0,\"CurrentMap\":null,\"Name\":{\"Identifier\":\"EnterLevel\",\"Present\":\"enter level\",\"Past\":\"entered level\",\"Category\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
            Assert.AreEqual(expected.TargetCell, result.TargetCell);

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new MovementActionTemplate
            {
                PerformerType=typeof(TeleportActionPerformer),
                Name = new Verb { UniqueId = "EnterLevel", Past = "entered level", Present = "enter level" },
                DefaultDistance = 0,
                TargetCell = 100
            };

            var actual = defaultActionTemplate.Clone() as MovementActionTemplate;
            defaultActionTemplate.TargetCell = 200;
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual(defaultActionTemplate.ToString(),actual.ToString());
Assert.AreEqual(0,actual.TargetCell);
        }
    }
}
