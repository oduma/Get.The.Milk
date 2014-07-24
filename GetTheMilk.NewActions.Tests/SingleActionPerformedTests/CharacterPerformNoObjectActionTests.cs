using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharacterPerformNoObjectActionTests
    {
        private Character _character = new Character { ObjectTypeId = "NPCFriendly" };
        private ObjectActionsFactory _factory = ObjectActionsFactory.GetFactory();

        [SetUp]
        public void SetUp()
        {

            var objAction = _factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
        }

        [Test]
        public void CharacterPerform()
        {
            _character.AddAvailableAction(new NoObjectActionTemplate
            {
                PerformerType=typeof(NoObjectActionTemplatePerformer),
                Name = new Verb
                {
                    UniqueId = "CloseInventory",
                        Past = "closed inventory",
                        Present = "close inventory"
                    }
            });
            var noObjectAction = _character.CreateNewInstanceOfAction<NoObjectActionTemplate>("CloseInventory");
            Assert.IsNotNull(noObjectAction);
            Assert.AreEqual(typeof(NoObjectActionTemplate), noObjectAction.GetType());
            noObjectAction.ActiveCharacter = _character;
            Assert.AreEqual(ActionResultType.Ok, _character.PerformAction(noObjectAction).ResultType);
        }

        [Test]
        public void CharacterCannotPerformNoPerformAvailable()
        {
            _character.AddAvailableAction(new NoObjectActionTemplate
            {
                Name = new Verb
                {
                    UniqueId = "NoPerformerAction",
                        Past = "no past",
                        Present = "no present"
                    }
            });
            var noObjectAction = _character.CreateNewInstanceOfAction<NoObjectActionTemplate>("NoPerformerAction");
            Assert.IsNotNull(noObjectAction);
            Assert.AreEqual(typeof(NoObjectActionTemplate), noObjectAction.GetType());
            noObjectAction.ActiveCharacter = _character;
            Assert.AreEqual(ActionResultType.CannotPerformThisAction, _character.PerformAction(noObjectAction).ResultType);
        }

    }
}
