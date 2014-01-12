using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class CommunicationActionsTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCasesCom")]
        public string TalkTests(CommunicateAction action, Character active, Character passive)
        {
            return active.TryPerformAction(action,passive).ExtraData as string;
        }

        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCasesComD")]
        public void DialogTests(CommunicateAction action, Character active, Character passive)
        {
            while(action!=null)
            {
                active.TryPerformAction(action,passive);
                if (active is Player)
                    Assert.True(action.Message.StartsWith("Interactive"));
                else
                    Assert.True(action.Message.StartsWith("Random Response"));
                action = passive.TryContinueInteraction(action, active) as CommunicateAction;
                var temp = passive;
                passive = active;
                active = temp;
            }
        }

        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCasesComInt")]
        public string DialogFollowedByGiveTests(CommunicateAction action, Character active, Character passive)
        {
            if(active.StartInteraction(action, passive).ResultType==ActionResultType.Ok)
            {
                return active.RightHandObject.Objects[0].Name.Main;
            }
            return string.Empty;

        }

    }
}
