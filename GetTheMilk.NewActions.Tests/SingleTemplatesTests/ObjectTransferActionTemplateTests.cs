using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ObjectTransferActionTemplateTests
    {
                [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate();

            Assert.AreEqual("ObjectTransferActionTemplate Target Object Not Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                CurrentPerformer=new BuyActionPerformer(),
                Name=new Verb{UniqueId="Buy",Past="bought", Present="buy"}
            };

            Assert.AreEqual("buy Target Object Not Assigned", defaultActionTemplate.ToString());

        }
        [Test]
        public void SerializeEmptyDefaultActionTemplate()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate();
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"PerformerType\":null,\"Name\":null,\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                CurrentPerformer = new SellActionPerformer(),
                Name = new Verb { UniqueId = "Sell", Past = "sold", Present = "sell" }
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.SellActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Sell\",\"Present\":\"sell\",\"Past\":\"sold\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new ObjectTransferActionTemplate();
            var result =
                JsonConvert.DeserializeObject<ObjectTransferActionTemplate>(
                    "{\"PerformerType\":null,\"Name\":null,\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new ObjectTransferActionTemplate
            {
                CurrentPerformer = new SellActionPerformer(),
                Name = new Verb { UniqueId = "Sell", Past = "sold", Present = "sell" }
            };
            var result =
                JsonConvert.DeserializeObject<ObjectTransferActionTemplate>(
                    "{\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.SellActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Sell\",\"Present\":\"sell\",\"Past\":\"sold\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                CurrentPerformer = new SellActionPerformer(),
                Name = new Verb { UniqueId = "Sell", Past = "sold", Present = "sell" }
            };

            var actual = defaultActionTemplate.Clone() as ObjectTransferActionTemplate;
            defaultActionTemplate.ActiveCharacter= new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual(defaultActionTemplate.ToString(),actual.ToString());
        }
    }

    }

