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
    public class OneCharacterOneObjectActionTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases1C1O")]
        public Inventory OneCharacterOnOneObjectAction(Character character,OneObjectAction action, IPositionableObject targetObject)
        {
            if (character.TryPerformAction(action,targetObject).ResultType==ActionResultType.NotOk)
                return targetObject.StorageContainer;

            if(targetObject.StorageContainer.InventoryType==InventoryType.CharacterInventory)
            {
                Assert.Contains(targetObject,character.ToolInventory.Objects);
            }
            if (targetObject.StorageContainer.InventoryType == InventoryType.InHand)
            {
                Assert.AreEqual(targetObject,character.RightHandObject.Objects[0]);
            }
            return targetObject.StorageContainer;
        }
    }
}
