using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class OneCharacterOneObjectActionTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases1C1O")]
        public Inventory OneCharacterOnOneObjectAction(GameAction action)
        {
            if (action.Perform().ResultType==ActionResultType.NotOk)
                return action.TargetObject.StorageContainer;

            if (action.TargetObject.StorageContainer.InventoryType == InventoryType.CharacterInventory)
            {
                Assert.Contains(action.TargetObject, action.ActiveCharacter.Inventory);
            }
            return action.TargetObject.StorageContainer;
        }
    }
}
