﻿using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersAbleToPerformObjectTransferActionsFromCharactersTests
    {
        private Character _character;
        private Level _level;
        private Player _player = new Player();
        private ObjectActionsFactory _factory = ObjectActionsFactory.GetFactory();

        [SetUp]
        public void SetUp()
        {
            _character = new Character { ObjectTypeId = "NPCFriendly" };

            var objAction = _factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            objAction = _factory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

        }


        [Test]
        public void CharacterNotAbleToTransferNoObject()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.ActiveCharacter = _character;
            Assert.False(_character.CanPerformAction(objectTransferAction));


        }
        [Test]
        public void CharacterNotAbleToTransferNoActiveCharacter()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory {MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory};
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            Assert.False(_character.CanPerformAction(objectTransferAction));
        }
        [Test]
        public void CharacterNotAbleToTransferNoTargetCharacter()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            Assert.False(_character.CanPerformAction(objectTransferAction));

        }
        [Test]
        public void CharacterNotAbleToTransferObjectNotAllowed()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectNothing,
                AllowsTemplateAction = objAction.AllowsTemplateAction
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.False(_character.CanPerformAction(objectTransferAction));
        }

        [Test]
        public void CharacterNotAbleToTransferNoRoom()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory
                                            {MaximumCapacity = 0, InventoryType = InventoryType.CharacterInventory};
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.False(_character.CanPerformAction(objectTransferAction));

        }
        [Test]
        public void CharacterAbleToTransferToOtherCharacter()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.True(_character.CanPerformAction(objectTransferAction));
        }
        [Test]
        public void CharacterNotAbleToTransferNoRoomForBuyer()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction,
                SellPrice=10
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.False(_character.CanPerformAction(objectTransferAction));
        }

        [Test]
        public void CharacterNotAbleToTransferNotEnoughMoney()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.Walet = new Walet {MaxCapacity = 2000, CurrentCapacity = 0};
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction,
                SellPrice = 10
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            targetCharacter.Walet.CurrentCapacity = 4;
            Assert.False(_character.CanPerformAction(objectTransferAction));

        }
        [Test]
        public void CharacterAbleToTransferObjectFromOtherCharacter()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.Name.Main);
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.Walet = new Walet { MaxCapacity = 2000, CurrentCapacity = 100 };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction,
                SellPrice = 10
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("GiveTo");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.True(_character.CanPerformAction(objectTransferAction));

        }

    }
}
