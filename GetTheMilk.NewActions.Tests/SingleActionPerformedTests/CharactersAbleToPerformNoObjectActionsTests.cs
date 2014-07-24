using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersAbleToPerformNoObjectActionsTests
    {

        private Character _character;
        private ObjectActionsFactory _factory = ObjectActionsFactory.GetFactory();

        [SetUp]
        public void SetUp()
        {

            _character = new Character { ObjectTypeId = "NPCFriendly" };
            var objAction = _factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
        }
        [Test]
        public void CharacterNotAbleToPerformNoDefaultAction()
        {
            var noObjectActionTemplate = _character.CreateNewInstanceOfAction<NoObjectActionTemplate>("CloseInventory");
            Assert.IsNull(noObjectActionTemplate);
        }

        [Test]
        public void CharacterNotAbleToPerformActiveCharacterNotAllow()
        {
            _character.AddAvailableAction(new NoObjectActionTemplate
            {
                PerformerType=typeof(NoObjectActionTemplatePerformer),
                Name = new Verb
                {
                        Past = "closed inventory",
                        Present = "close inventory",
                        UniqueId="CloseInventory"
                    }
            });
            _character.AllowsTemplateAction = TestHelper.AllowsNothing;
            var noObjectAction = _character.CreateNewInstanceOfAction<NoObjectActionTemplate>("CloseInventory");
            Assert.IsNotNull(noObjectAction);
            Assert.AreEqual(typeof(NoObjectActionTemplate), noObjectAction.GetType());
            Assert.False(_character.CanPerformAction(noObjectAction));
        }
        [Test]
        public void CharacterAbleToPerform()
        {
            _character.AddAvailableAction(new NoObjectActionTemplate
            {
                PerformerType=typeof(NoObjectActionTemplatePerformer),
                Name = new Verb
                {
                        Past = "closed inventory",
                        Present = "close inventory",
                        UniqueId="CloseInventory"
                    }
            });
            var noObjectAction = _character.CreateNewInstanceOfAction<NoObjectActionTemplate>("CloseInventory");
            Assert.IsNotNull(noObjectAction);
            Assert.AreEqual(typeof(NoObjectActionTemplate), noObjectAction.GetType());
            Assert.True(_character.CanPerformAction(noObjectAction));
        }

    }
}
