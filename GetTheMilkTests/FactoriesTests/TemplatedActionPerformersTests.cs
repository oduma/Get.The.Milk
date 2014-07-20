using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
