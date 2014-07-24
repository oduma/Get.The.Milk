using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Utils;
using NUnit.Framework;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersAbleToPerformInventoryActionsTests
    {
        private Player _player = new Player();
        private Character _character;
        [SetUp]
        public void SetUp()
        {
            _character = new Character { ObjectTypeId = "NPCFriendly" };
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            objAction = factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
        }

        [Test]
        public void PlayerNotAbleNoTargetCharacter()
        {
            var exposeInventoryActionn = _player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            Assert.IsNotNull(exposeInventoryActionn);
            Assert.AreEqual(typeof(ExposeInventoryActionTemplate), exposeInventoryActionn.GetType());
            Assert.False(_player.CanPerformAction(exposeInventoryActionn));

        }

        [Test]
        public void PlayerNotAbleNotSelfInventory()
        {
            var exposeInventoryActionn = _player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            exposeInventoryActionn.TargetCharacter = _player;
            exposeInventoryActionn.SelfInventory = false;
            Assert.IsNotNull(exposeInventoryActionn);
            Assert.AreEqual(typeof(ExposeInventoryActionTemplate), exposeInventoryActionn.GetType());
            Assert.False(_player.CanPerformAction(exposeInventoryActionn));
        }

        [Test]
        public void PlayerAbleSelfInventory()
        {

            var exposeInventoryActionn = _player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            exposeInventoryActionn.TargetCharacter = _player;
            exposeInventoryActionn.SelfInventory = true;
            Assert.IsNotNull(exposeInventoryActionn);
            Assert.AreEqual(typeof(ExposeInventoryActionTemplate), exposeInventoryActionn.GetType());
            Assert.True(_player.CanPerformAction(exposeInventoryActionn));
        }

        [Test]
        public void CharacterNotAbleAsStartingAction()
        {
            var exposeInventoryActionTemplate = _character.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            Assert.IsNull(exposeInventoryActionTemplate);
        }

        [Test]
        public void CharacterAbleToExposeInventoryToUser()
        {
            _character.Name = new Noun {Main = "testChar", Narrator = "test char"};
            _character.Interactions.Add(GenericInteractionRulesKeys.CharacterSpecific, new Interaction[]{                                                                                                        new Interaction
                                                                                                            {
                                                                                                                Action =
                                                                                                                    new TwoCharactersActionTemplate
                                                                                                                        {
                                                                                                                            Message="Yes",
                                                                                                                            FinishTheInteractionOnExecution=true,
                                                                                                                            PerformerType=typeof(CommunicateActionPerformer),
                                                                                                                            Name = new Verb{UniqueId="SayYes",Past="said", Present="say"}
                                                                                                                        },
                                                                                                                Reaction
                                                                                                                    =
                                                                                                                    new ExposeInventoryActionTemplate
                                                                                                                        {
                                                                                                                            FinishActionUniqueId="CloseInventory",
                                                                                                                            PerformerType=typeof(ExposeInventoryActionTemplatePerformer),
                                                                                                                            Name = new Verb{UniqueId="ExposeInventory",Past="exposed", Present="expose"}
                                                                                                                        }
                                                                                                            }});
            var exposeInventoryActionn = _character.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            exposeInventoryActionn.TargetCharacter = _player;
            exposeInventoryActionn.SelfInventory = false;
            Assert.IsNotNull(exposeInventoryActionn);
            Assert.AreEqual(typeof(ExposeInventoryActionTemplate), exposeInventoryActionn.GetType());
            Assert.True(_player.CanPerformAction(exposeInventoryActionn));
        }

    }
}
