using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using System.Linq;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class ExposeInentoryActionTests
    {
        [Test]
        public void ExposeOwnInventory_JustShow()
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

        [Test]
        public void ExposeAnotherCharacterInventory()
        {
            Level _level = Level.Create(1);

            var active = new Player();
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            active.AllowsAction = objAction.AllowsAction;
            active.AllowsIndirectAction = objAction.AllowsIndirectAction;

            ExposeInventory exposeInventory = new ExposeInventory();
            exposeInventory.IncludeWallet = false;
            exposeInventory.ActiveCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            exposeInventory.TargetCharacter = active;
            var result = exposeInventory.Perform();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ExtraData);
            Assert.IsNotNull(((ExposeInventoryExtraData)result.ExtraData).Contents);
            Assert.AreEqual(2, ((ExposeInventoryExtraData)result.ExtraData).Contents.Count());
            Assert.AreEqual(2, ((ExposeInventoryExtraData)result.ExtraData).Contents[0].PossibleUsses.Count());
            Assert.AreEqual(ActionType.Buy, ((ExposeInventoryExtraData)result.ExtraData).Contents[0].PossibleUsses[0].ActionType);
            Assert.AreEqual(ActionType.TakeFrom, ((ExposeInventoryExtraData)result.ExtraData).Contents[0].PossibleUsses[1].ActionType);
            Assert.AreEqual(1, ((ExposeInventoryExtraData)result.ExtraData).Contents[1].PossibleUsses.Count());
            
        }
        [Test]
        public void ExposeOwnInventory_ToUseAgainstAnother()
        {
            Level _level = Level.Create(1);

            var active = new Player();
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            active.AllowsAction = objAction.AllowsAction;
            active.AllowsIndirectAction = objAction.AllowsIndirectAction;

            active.Inventory.Add(_level.Characters[0].Inventory[0]);

            Assert.AreEqual(1, active.Inventory.Count());

            ExposeInventory exposeInventory = new ExposeInventory();
            exposeInventory.IncludeWallet = false;
            exposeInventory.TargetCharacter = _level.Characters[1];
            exposeInventory.ActiveCharacter = active;
            exposeInventory.SelfInventory = true;
            var result = exposeInventory.Perform();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ExtraData);
            Assert.IsNotNull(((ExposeInventoryExtraData)result.ExtraData).Contents);
            Assert.AreEqual(1, ((ExposeInventoryExtraData)result.ExtraData).Contents.Count());
            Assert.AreEqual(ActionType.SelectAttackWeapon, ((ExposeInventoryExtraData)result.ExtraData).Contents[0].PossibleUsses[0].ActionType);
            Assert.AreEqual(ActionType.SelectDefenseWeapon, ((ExposeInventoryExtraData)result.ExtraData).Contents[0].PossibleUsses[1].ActionType);
        }

    }
}
