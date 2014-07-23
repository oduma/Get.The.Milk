using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
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
    public class CharactersAbleToPerformObjectUseOnObjectActionsTests
    {
        private Character _character;
        private Level _level;

        [SetUp]
        public void SetUp()
        {
            _character = new Character { ObjectTypeId = "NPCFriendly" };
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

            _character.Inventory = new Inventory {MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory};
            _character.Inventory.Add(_level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "Key"));
            _character.Inventory.Add(new Tool
                                         {
                                             Name = new Noun {Main = "BlueKey", Narrator = "blue key"},
                                             ObjectTypeId = "Key"
                                         });

        }
        [Test]
        public void CharacterNotAbleToTransferNoDefaultAction()
        {
            var objectTransferActionTemplate = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNull(objectTransferActionTemplate);
        }


        [Test]
        public void CharacterNotAbleToPerformNoActiveObject()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject,targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            Assert.False(_character.CanPerformAction(useAction));

        }
        [Test]
        public void CharacterNotAbleToPerformNoTargetObject()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            useAction.ActiveObject = _character.Inventory[0];
            Assert.False(_character.CanPerformAction(useAction));

        }
        [Test]
        public void CharacterNotAbleToPerformActiveObjectNotAllow()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());

            useAction.ActiveObject = _character.Inventory[1];
            _character.Inventory[1].AllowsTemplateAction = TestHelper.AllowsNothing;
            useAction.TargetObject = targetObject;
            Assert.False(_character.CanPerformAction(useAction));

        }
        [Test]
        public void CharacterNotAbleToPerformTargetObjectNotAllowed()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            useAction.ActiveObject = _character.Inventory[1];
            _character.Inventory[1].AllowsTemplateAction = TestHelper.AllowsEverything;
            useAction.TargetObject = targetObject;
            Assert.False(_character.CanPerformAction(useAction));
        }
        [Test]
        public void CharacterAbleToPerform()
        {
            var targetObject = _level.Inventory.FirstOrDefault(o => o.ObjectTypeId == "RedDoor");
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Open");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            useAction.ActiveObject = _character.Inventory[0];
            _character.Inventory[1].AllowsTemplateAction = TestHelper.AllowsEverything;
            useAction.TargetObject = targetObject;
            Assert.True(_character.CanPerformAction(useAction));
        }
    }
}
