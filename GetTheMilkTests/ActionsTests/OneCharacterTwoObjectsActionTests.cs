using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class OneCharacterTwoObjectsActionTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases1C2O")]
        public IPositionable TransfromFromMovementPreventingToNot(ObjectUseOnObjectAction action)
        {
            action.Perform();
            return action.TargetObject;
        }

    }
}
