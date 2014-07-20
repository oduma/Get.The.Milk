using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

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

            Assert.AreEqual("{\"FinishActionType\":null,\"FinishActionCategory\":null,\"SelfInventory\":false,\"Category\":\"ExposeInventoryActionTemplate\",\"Name\":{\"Identifier\":\"ExposeInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                FinishActionType = "Attack",
                FinishActionCategory=typeof(TwoCharactersActionTemplate)
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"FinishActionType\":\"Attack\",\"FinishActionCategory\":\"GetTheMilk.Actions.ActionTemplates.TwoCharactersActionTemplate, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"SelfInventory\":false,\"Category\":\"ExposeInventoryActionTemplate\",\"Name\":{\"Identifier\":\"ExposeInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new ExposeInventoryActionTemplate();
            var result =
                JsonConvert.DeserializeObject<ExposeInventoryActionTemplate>(
                    "{\"FinishActionType\":null,\"FinishActionCategory\":null,\"SelfInventory\":false,\"Name\":{\"Identifier\":\"ExposeInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\",\"Category\":null},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new ExposeInventoryActionTemplate
            {
                FinishActionType = "Attack",
                FinishActionCategory = typeof(TwoCharactersActionTemplate)
            };
            var result =
                JsonConvert.DeserializeObject<ExposeInventoryActionTemplate>(
                    "{\"FinishActionType\":\"Attack\",\"FinishActionCategory\":\"GetTheMilk.Actions.ActionTemplates.TwoCharactersActionTemplate, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"SelfInventory\":false,\"Name\":{\"Identifier\":\"SelectAttackWeapon\",\"Present\":\"select attack weapon\",\"Past\":\"selected attack weapon\",\"Category\":null},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());
            Assert.AreEqual(expected.FinishActionCategory,result.FinishActionCategory);
            Assert.AreEqual(expected.FinishActionType,result.FinishActionType);

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                FinishActionType = "Attack",
                FinishActionCategory = typeof(TwoCharactersActionTemplate)
            };

            var actual = defaultActionTemplate.Clone() as ExposeInventoryActionTemplate;
            defaultActionTemplate.ActiveCharacter = new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual("Attack",actual.FinishActionType);
            Assert.AreEqual((typeof(TwoCharactersActionTemplate)),actual.FinishActionCategory);

        }

    }
}
