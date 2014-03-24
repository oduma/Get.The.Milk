using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class ActionsFactoryTests
    {
        [Test]
        public void GetAllMainActionsFromFactory()
        {
            var actions = ActionsFactory.GetFactory().GetActions();
            Assert.IsNotNull(actions);
            Assert.Greater(actions.Length,1);

        }
    }
}
