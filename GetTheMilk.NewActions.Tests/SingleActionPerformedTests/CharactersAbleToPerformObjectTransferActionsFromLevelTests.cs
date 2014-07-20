using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersAbleToPerformObjectTransferActionsFromLevelTests
    {
        private Character _character;
        private Level _level;
        private Player _player = new Player();

        [SetUp]
        public void SetUp()
        {
            _character = new Character { ObjectTypeId = "NPCFriendly" };

            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
            
            objAction = factory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

        }

        [Test]
        public void CharacterNotAbleToTransferNoDefaultAction()
        {
            var objectTransferActionTemplate = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNull(objectTransferActionTemplate);
        }

        [Test]
        public void PlayerNotAbleToTransferNoDefaultAction()
        {
            var objectTransferActionTemplate = _player.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNull(objectTransferActionTemplate);
        }

        [Test]
        public void CharacterNotAbleToTransferNoObject()
        {
            var targetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Tool);
            _character.LoadInteractions(targetObject,targetObject.Name.Main);
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.ActiveCharacter = _character;
            Assert.False(_character.CanPerformAction(objectTransferAction));

        }
        [Test]
        public void CharacterNotAbleToTransferNoActiveCharacter()
        {
            var targetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Tool);
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = targetObject;
            Assert.False(_character.CanPerformAction(objectTransferAction));


        }
        [Test]
        public void CharacterNotAbleToTransferObjectNotAllowed()
        {
            var targetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Tool);
            var anotherTargetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Decor);
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetObject = anotherTargetObject;
            Assert.False(_character.CanPerformAction(objectTransferAction));
        }

        [Test]
        public void CharacterNotAbleToTransferNoRoom()
        {
            var targetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Tool);
            _character.Inventory = new Inventory {MaximumCapacity = 0, InventoryType = InventoryType.CharacterInventory};
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = targetObject;
            objectTransferAction.ActiveCharacter = _character;
            Assert.False(_character.CanPerformAction(objectTransferAction));
        }
        [Test]
        public void CharacterAbleToTransferObjectFromLevel()
        {
            var targetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Tool);
            _character.Inventory = new Inventory { MaximumCapacity = 1, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetObject, targetObject.Name.Main);
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = targetObject;
            objectTransferAction.ActiveCharacter = _character;
            Assert.True(_character.CanPerformAction(objectTransferAction));

        }

    }
}
