using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class ExposeInentoryActionTests
    {
        [Test]
        public void ExposeOwnInventory()
        {
            Level _level = Level.Create(1);

            var active = new Player();
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            active.AllowsAction = objAction.AllowsAction;
            active.AllowsIndirectAction = objAction.AllowsIndirectAction;

            active.Inventory.Add(_level.CurrentMap.Cells[3].AllObjects.FirstOrDefault(o => o.ObjectCategory == ObjectCategory.Tool));

            Assert.AreEqual(1, active.Inventory.Count());

            ExposeInventory exposeInventory = new ExposeInventory();
            exposeInventory.IncludeWallet = false;
            exposeInventory.TargetCharacter = active;
            exposeInventory.ActiveCharacter = active;
            var result = exposeInventory.Perform();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ExtraData);
            Assert.IsNotNull(((ExposeInventoryExtraData)result.ExtraData).Contents);
            Assert.AreEqual(1,((ExposeInventoryExtraData)result.ExtraData).Contents.Count());
            Assert.True(((ExposeInventoryExtraData)result.ExtraData).Contents.Any());
        }
    }
}
