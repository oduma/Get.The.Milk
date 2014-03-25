using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Factories;
using GetTheMilk.Navigation;
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

        [Test]
        public void CreateNewInstancesOfTheSameActionTemplate()
        {
            var actions = ActionsFactory.GetFactory().GetActions();
            Assert.IsNotNull(actions);
            var actionTemplate = actions.FirstOrDefault(a => a.ActionType == ActionType.Run);
            var run1= ActionsFactory.GetFactory().CreateNewActionInstance(actionTemplate.ActionType);
            var run2 = ActionsFactory.GetFactory().CreateNewActionInstance(actionTemplate.ActionType);

            ((Run)run1).Direction=Direction.North;

            Assert.AreEqual(Direction.North, ((Run)run1).Direction);
            Assert.AreEqual(Direction.None, ((Run)run2).Direction);

            ((Run) run2).Direction = Direction.Top;

            Assert.AreEqual(Direction.North, ((Run)run1).Direction);
            Assert.AreEqual(Direction.Top, ((Run)run2).Direction);

        }
    }
}
