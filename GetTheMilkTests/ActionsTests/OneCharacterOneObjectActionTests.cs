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
        public Inventory OneCharacterOnOneObjectAction(Character character,OneObjectAction action, NonCharacterObject targetObject)
        {
            if (character.TryPerformAction(action,targetObject).ResultType==ActionResultType.NotOk)
                return targetObject.StorageContainer;

            if(targetObject.StorageContainer.InventoryType==InventoryType.CharacterInventory)
            {
                Assert.Contains(targetObject,character.Inventory.Objects);
            }
            return targetObject.StorageContainer;
        }
    }
}
