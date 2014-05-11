using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilkTests.FactoriesTests
{
    [TestFixture]
    public class ObjectActionsFactoryTests
    {
        [Test]
        public void ObjectActionsFactories_Ok()
        {
            var objectActionsFactory = ObjectActionsFactory.GetFactory();
            var allRegisteredNames = objectActionsFactory.ListAllRegisterNames(ObjectCategory.Tool);
            Assert.IsNotNull(allRegisteredNames);
            Assert.Contains("Skrewdriver",allRegisteredNames.ToList());
        }
    }
}
