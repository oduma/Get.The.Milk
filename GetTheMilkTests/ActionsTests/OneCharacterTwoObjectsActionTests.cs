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
        public IPositionable TransfromFromMovementPreventingToNot(Character active, NonCharacterObject passiveObject, ObjectUseOnObjectAction action)
        {
            active.TryPerformObjectOnObjectAction(action, ref passiveObject);
            return passiveObject;
        }

    }
}
