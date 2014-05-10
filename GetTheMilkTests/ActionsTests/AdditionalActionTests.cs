using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class AdditionalActionTests
    {
        [Test]
        public void DefuseTests()
        {
            var defuser = new Tool {AllowsAction = AllowsToDefuse};
            var defusee = new Tool {AllowsIndirectAction = allowsToBeDefused};
            var active = new Player();
            active.Experience = 10;
            active.Health = 10;
            var defuseAction = new Defuse {ActiveCharacter = active, ActiveObject = defuser, TargetObject = defusee};
            var defuseResult = defuseAction.Perform();
            if(defuseResult.ResultType==ActionResultType.Ok)
            {
                Assert.AreEqual(10,active.Health);
                Assert.IsNotNull(defusee);
                Assert.IsNotNull(defuser);
            }
            else
            {
                Assert.AreEqual(5,active.Health);
                Assert.IsNull(defuser);
                Assert.IsNull(defusee);
            }
        }

        private bool allowsToBeDefused(GameAction arg1, IPositionable arg2)
        {
            return true;
        }

        private bool AllowsToDefuse(GameAction arg)
        {
            return true;
        }
    }
}
