using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class OneCharacterTwoObjectsActionTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases1C2O")]
        public IPositionableObject TransfromFromMovementPreventingToNot(Character active, IPositionableObject activeObject, IPositionableObject passiveObject, ObjectUseOnObjectAction action)
        {
            active.TryPerformObjectOnObjectAction(action, passiveObject);
            Assert.IsNull(activeObject);
            return passiveObject;
        }

    }
}
