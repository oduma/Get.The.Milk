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
