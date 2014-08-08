using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilkTests.FactoriesTests
{
    [TestFixture]
    public class TemplatedActionPerformersTests
    {
        [Test]
        public void GetAListOfPerformersFromFactory()
        {
            var performers =
                TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers<IMovementActionTemplatePerformer>();
            Assert.IsNotNull(performers);
            Assert.AreEqual(3,performers.Length);
        }
    }
}
