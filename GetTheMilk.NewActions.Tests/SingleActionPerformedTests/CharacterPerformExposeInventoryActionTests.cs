using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharacterPerformExposeInventoryActionTests
    {
        private Player _player = new Player();
        private Character _character = new Character { ObjectTypeId = "NPCFriendly" };
        private Level _level;
        private ObjectActionsFactory _factory = ObjectActionsFactory.GetFactory();

        [SetUp]
        public void SetUp()
        {

            var objAction = _factory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            objAction = _factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _level = TestHelper.GenerateALevel();

        }

        [Test]
        public void PlayerExposeOwnInventory()
        {
            _player.Inventory.Add(new Tool
                                      {
                                          Name = new Noun {Main = "test tool", Narrator = "test tool"},
                                          AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                                          AllowsTemplateAction = TestHelper.AllowsEverything
                                      });

            Assert.AreEqual(1, _player.Inventory.Count());
            var exposeInventoryActionn = _player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            exposeInventoryActionn.TargetCharacter = _player;
            exposeInventoryActionn.SelfInventory = true;
            var result = _player.PerformAction(exposeInventoryActionn);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ExtraData);
            Assert.IsNotNull(((InventoryExtraData)result.ExtraData).Contents);
            Assert.AreEqual(1,((InventoryExtraData)result.ExtraData).Contents.Count());
            Assert.True(((InventoryExtraData)result.ExtraData).Contents.Any());
        }

        [Test]
        public void ACharacterExposesToThePlayerCharacterOwnInventory()
        {
            _character.Name = new Noun { Main = "testChar", Narrator = "test char" };
            _character.AddAvailableAction(new ExposeInventoryActionTemplate
                                        {
                                            FinishingAction=ExposeInventoryFinishingAction.CloseInventory,
                                            PerformerType=typeof(ExposeInventoryActionTemplatePerformer),
                                            Name= new Verb{UniqueId="ExposeSelfInventory",Past="exposed inventory",Present="expose inventory"}
                                        });
            _character.Inventory = new Inventory {MaximumCapacity = 10,InventoryType=InventoryType.CharacterInventory};
            var objAction = _factory.CreateObjectAction("Tool");
            _character.Inventory.Add(new Tool
                                         {
                                             Name = new Noun {Main = "test tool", Narrator = "test tool"},
                                             AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction,
                                             AllowsTemplateAction = objAction.AllowsTemplateAction
                                         });
            var exposeInventoryActionn = _character.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeSelfInventory");
            exposeInventoryActionn.ActiveCharacter = _character;
            exposeInventoryActionn.TargetCharacter = _player;
            exposeInventoryActionn.SelfInventory = false;
            
            var result = _character.PerformAction(exposeInventoryActionn);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ExtraData);
            Assert.IsNotNull(((InventoryExtraData)result.ExtraData).Contents);
            Assert.AreEqual(1, ((InventoryExtraData)result.ExtraData).Contents.Count());
            Assert.AreEqual(2, ((InventoryExtraData)result.ExtraData).Contents[0].PossibleUsses.Count());
            Assert.AreEqual("Buy", ((InventoryExtraData)result.ExtraData).Contents[0].PossibleUsses[0].Name.UniqueId);
        }
    }
}
