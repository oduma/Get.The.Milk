using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
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
            var defuser = new Tool {AllowsTemplateAction = AllowsToDefuse};
            var defusee = new Tool {AllowsIndirectTemplateAction = allowsToBeDefused};
            var active = new Player();
            active.Experience = 10;
            active.Health = 10;
            var defuseAction = new ObjectUseOnObjectActionTemplate
                                   {
                                       Name=new Verb{PerformerId="Defuse"},
                                       ChanceOfSuccess = ChanceOfSuccess.Big,
                                       DestroyActiveObject = true,
                                       DestroyTargetObject = true,
                                       PercentOfHealthFailurePenalty = 50,
                                       ActiveCharacter = active,
                                       ActiveObject = defuser,
                                       TargetObject = defusee
                                   };
            var defuseResult = active.PerformAction(defuseAction);
            Assert.AreNotEqual(ActionResultType.CannotPerformThisAction,defuseResult.ResultType);
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

        private bool allowsToBeDefused(BaseActionTemplate arg1, IPositionable arg2)
        {
            return true;
        }

        private bool AllowsToDefuse(BaseActionTemplate arg)
        {
            return true;
        }
    }
}
