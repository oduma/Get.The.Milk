using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharacterPerformObjectUseOnObjectActionTests
    {
        private Character _character = new Character { ObjectTypeId = "NPCFriendly" };
        private Level _level;

        [SetUp]
        public void SetUp()
        {
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.Inventory.Add(_level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "Key"));
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "BlueKey", Narrator = "blue key" },
                ObjectTypeId = "Key"
            });
            _character.Health = 100;
        }

        [Test]
        public void CharacterPerformDestructiveActionNoSelfDamage()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            useAction.ActiveCharacter = _character;
            useAction.ActiveObject = _character.Inventory[0];
            _character.Inventory[1].AllowsTemplateAction = TestHelper.AllowsEverything;
            useAction.TargetObject = targetObject;
            Assert.AreEqual(ActionResultType.Ok, _character.PerformAction(useAction).ResultType);
            Assert.AreEqual(1,_character.Inventory.Count);
            Assert.AreEqual(2,_level.Inventory.Count);
            Assert.AreEqual(100,_character.Health);
        }

        [Test]
        public void CharacterPerformDestructiveActionSelfDamage()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            useAction.ActiveCharacter = _character;
            useAction.ActiveObject = _character.Inventory[0];
            _character.Inventory[1].AllowsTemplateAction = TestHelper.AllowsEverything;
            useAction.TargetObject = targetObject;
            useAction.PercentOfHealthFailurePenalty = 50;
            useAction.ChanceOfSuccess = ChanceOfSuccess.None;
            Assert.AreEqual(ActionResultType.NotOk, _character.PerformAction(useAction).ResultType);
            Assert.AreEqual(2, _character.Inventory.Count);
            Assert.AreEqual(3, _level.Inventory.Count);
            Assert.AreEqual(50, _character.Health);
        }
    }
}
