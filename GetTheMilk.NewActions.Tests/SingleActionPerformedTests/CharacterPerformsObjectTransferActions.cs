using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharacterPerformsObjectTransferActions
    {
        private Character _character = new Character
                                           {
                                               ObjectTypeId = "NPCFriendly",
                                               Name = new Noun {Main = "test character", Narrator = "test character"}
                                           };
        private Level _level;
        private Player _player = new Player();
        private ObjectActionsFactory _factory = ObjectActionsFactory.GetFactory();

        [SetUp]
        public void SetUp()
        {

            var objAction = _factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            objAction = _factory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

        }

        [Test]
        public void CharacterTransferObjectFromLevel()
        {
            var targetObject = _level.Inventory.First(o => o.ObjectCategory == ObjectCategory.Tool);
            _character.Inventory = new Inventory { MaximumCapacity = 1, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetObject, targetObject.GetType());
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Keep");
            objectTransferAction.TargetObject = targetObject;
            objectTransferAction.ActiveCharacter = _character;
            Assert.AreEqual(ActionResultType.Ok, _character.PerformAction(objectTransferAction).ResultType);
            Assert.AreEqual(1,_character.Inventory.Count);

        }

        [Test]
        public void CharacterTransferObjectToLevel()
        {
            var targetObject = new Tool
                                   {
                                       Name = new Noun {Main = "Test Tool", Narrator = "test tool"},
                                       AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                                       AllowsTemplateAction = TestHelper.AllowsEverything
                                   };
            _level.Characters.First().Inventory.Add(targetObject);
            var initialInventroyCount = _level.Characters.First().Inventory.Count;
            var initialLevelInventoryCount = _level.Inventory.Count;
            var objectTransferActionTemplate = _level.Characters.First().ActionsForExposedContents[ContentActionsKeys.SelfContentActions].FirstOrDefault(
a => a.Name.UniqueId == "Discard");
            var objectTransferAction = objectTransferActionTemplate.Clone();
            objectTransferAction.TargetObject = targetObject;
            objectTransferAction.ActiveCharacter = _level.Characters.First();
            Assert.AreEqual(ActionResultType.Ok, _level.Characters.First().PerformAction(objectTransferAction).ResultType);
            Assert.AreEqual(initialInventroyCount-1, _level.Characters.First().Inventory.Count);
            Assert.AreEqual(initialLevelInventoryCount + 1, _level.Inventory.Count);

        }

        [Test]
        [Ignore]
        public void CharacterTransferToOtherCharacter()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.GetType());
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
            Assert.AreEqual(ActionResultType.Ok,_character.PerformAction(objectTransferAction).ResultType);
            Assert.AreEqual(0, _character.Inventory.Count);
            Assert.AreEqual(1, targetCharacter.Inventory.Count);
        }

        [Test]
        public void CharacterTransferFromOtherCharacter()
        {
            var activeCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            activeCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            activeCharacter.LoadInteractions(_character, _character.GetType());
            activeCharacter.AddAvailableAction(new ObjectTransferActionTemplate
            {
                PerformerType=typeof(TakeFromActionPerformer),
                Name = new Verb
                {
                    UniqueId = "TakeFrom",
                    Past = "took from",
                    Present = "take from"
                }
            });
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction
            });
            var objectTransferAction = activeCharacter.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("TakeFrom");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.TargetCharacter = _character;
            Assert.AreEqual(ActionResultType.Ok, activeCharacter.PerformAction(objectTransferAction).ResultType);
            Assert.AreEqual(0, _character.Inventory.Count);
            Assert.AreEqual(1, activeCharacter.Inventory.Count);
        }


        [Test]
        [Ignore]
        public void CharacterTransferObjectToOtherCharacterAndReceiveMoney()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.GetType());
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
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Sell");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = _character.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.AreEqual(ActionResultType.Ok,_character.PerformAction(objectTransferAction).ResultType);
            Assert.AreEqual(0, _character.Inventory.Count);
            Assert.AreEqual(1, targetCharacter.Inventory.Count);
            Assert.AreEqual(110,_character.Walet.CurrentCapacity);
            Assert.AreEqual(90, targetCharacter.Walet.CurrentCapacity);

        }

        [Test]
        public void CharacterTransferObjectFromOtherCharacterAndGiveMoney()
        {
            var targetCharacter = _level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly");
            targetCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.LoadInteractions(targetCharacter, targetCharacter.GetType());
            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.Walet = new Walet { MaxCapacity = 2000, CurrentCapacity = 100 };
            _character.AddAvailableAction(new ObjectTransferActionTemplate
            {
                PerformerType=typeof(BuyActionPerformer),
                Name = new Verb
                {
                    UniqueId = "Buy",
                    Past = "bought",
                    Present = "buy"
                }
            });
            var objAction = _factory.CreateObjectAction("Tool");
            targetCharacter.Inventory.Add(new Tool
            {
                Name = new Noun { Main = "test tool", Narrator = "test tool" },
                AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                AllowsTemplateAction = objAction.AllowsTemplateAction,
                BuyPrice = 10
            });
            var objectTransferAction = _character.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("Buy");
            Assert.IsNotNull(objectTransferAction);
            Assert.AreEqual(typeof(ObjectTransferActionTemplate), objectTransferAction.GetType());
            objectTransferAction.TargetObject = targetCharacter.Inventory[0];
            objectTransferAction.ActiveCharacter = _character;
            objectTransferAction.TargetCharacter = targetCharacter;
            Assert.AreEqual(ActionResultType.Ok, _character.PerformAction(objectTransferAction).ResultType);
            Assert.AreEqual(1, _character.Inventory.Count);
            Assert.AreEqual(0, targetCharacter.Inventory.Count);
            Assert.AreEqual(90, _character.Walet.CurrentCapacity);
            Assert.AreEqual(110, targetCharacter.Walet.CurrentCapacity);

        }

    }
}
