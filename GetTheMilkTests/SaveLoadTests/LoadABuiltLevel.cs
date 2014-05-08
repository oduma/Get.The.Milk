using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilkTests.SaveLoadTests
{
    [TestFixture]
    public class LoadABuiltLevel
    {
        [Test]
        public void SaveAndLoadLevelWithoutPlayer()
        {
            var actual = Level.Create(3);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Number);
            Assert.AreEqual("Test Level1", actual.Name.Main);
            Assert.AreEqual("The light side", actual.Name.Narrator);
            Assert.AreEqual(0, actual.StartingCell);
            Assert.AreEqual("Some Story", actual.Story);
            Assert.AreEqual(9, actual.CurrentMap.Cells.Length);
            Assert.IsNotNull(actual.Inventory);
            Assert.AreEqual(InventoryType.LevelInventory, actual.Inventory.InventoryType);
            Assert.AreEqual("Test Level1", actual.Inventory.Owner.Name.Main);
            Assert.AreEqual(4, actual.Inventory.Count);
            Assert.False(actual.Inventory.Any(o => o.StorageContainer.Owner.Name.Main != actual.Name.Main));
            Assert.AreEqual(1, actual.CurrentMap.Cells[3].AllObjects.Count());
            Assert.IsNotNull(actual.Characters);
            Assert.AreEqual(2, actual.Characters.Count);
            Assert.AreEqual(InventoryType.CharacterInventory, actual.Characters[0].Inventory.InventoryType);
            Assert.AreEqual(2, actual.Characters.Count(c => c.StorageContainer.Owner != null));
        }

    }
}
