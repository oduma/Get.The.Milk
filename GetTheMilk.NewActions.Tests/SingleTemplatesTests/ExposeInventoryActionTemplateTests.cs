using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ExposeInventoryActionTemplateTests
    {
        [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate();

            Assert.AreEqual("Expose Inventory", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                ActiveCharacter = new Player(),
                SelfInventory=true
            };

            Assert.AreEqual("Expose Inventory", defaultActionTemplate.ToString());

        }
        [Test]
        public void SerializeEmptyDefaultActionTemplate()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate();
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"PerformerType\":null,\"FinishActionUniqueId\":null,\"SelfInventory\":false,\"Name\":null,\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                FinishActionUniqueId = "Attack",
                PerformerType=typeof(ExposeInventoryActionTemplatePerformer)
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"FinishActionUniqueId\":\"Attack\",\"SelfInventory\":false,\"Name\":null,\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new ExposeInventoryActionTemplate();
            var result =
                JsonConvert.DeserializeObject<ExposeInventoryActionTemplate>(
                    "{\"PerformerType\":null,\"FinishActionUniqueId\":null,\"SelfInventory\":false,\"Name\":null,\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new ExposeInventoryActionTemplate
            {
                FinishActionUniqueId = "Attack",
                PerformerType=typeof(ExposeInventoryActionTemplatePerformer)
            };
            var result =
                JsonConvert.DeserializeObject<ExposeInventoryActionTemplate>(
                    "{\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"FinishActionUniqueId\":\"Attack\",\"SelfInventory\":false,\"Name\":null,\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
            Assert.AreEqual(expected.FinishActionUniqueId,result.FinishActionUniqueId);

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                FinishActionUniqueId = "Attack",
                PerformerType=typeof(ExposeInventoryActionTemplatePerformer)
            };

            var actual = defaultActionTemplate.Clone() as ExposeInventoryActionTemplate;
            defaultActionTemplate.ActiveCharacter = new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual("Attack",actual.FinishActionUniqueId);

        }

    }
}
