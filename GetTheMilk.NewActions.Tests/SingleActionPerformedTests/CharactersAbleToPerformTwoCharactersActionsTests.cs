using System.Linq;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersAbleToPerformTwoCharactersActionsTests
    {
        private Character _activeCharacter;
        private Character _targetCharacter;
        private ObjectActionsFactory _factory = ObjectActionsFactory.GetFactory();

        [SetUp]
        public void SetUp()
        {

        _activeCharacter = new Character { ObjectTypeId = "NPCFriendly" };
        _targetCharacter = new Character { ObjectTypeId = "NPCFriendly" };
            var objAction = _factory.CreateObjectAction("NPCFriendly");
            _activeCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _activeCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _targetCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _targetCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

        }


        [Test]
        public void CharacterNotAbleToPerformDefaultAction()
        {
            var noObjectActionTemplate = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNull(noObjectActionTemplate);
        }

        [Test]
        public void CharacterNotAbleToPerformNoActiveCharacter()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                        Past = "talked",
                        Present = "talk",
                        UniqueId="Talk1"
                    }
            });

            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            Assert.False(_activeCharacter.CanPerformAction(twoCharsAction));

        }
        [Test]
        public void CharacterNotAbleToPerformNoTargetCharacter()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                    Past = "talked",
                        Present = "talk",
                        UniqueId="Talk1"
                    }
            });
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.TargetCharacter = _targetCharacter;
            Assert.False(_activeCharacter.CanPerformAction(twoCharsAction));
        }

        [Test]
        public void CharacterNotAbleToPerformActiveCharacterNotAllow()
        {

            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                    Past = "talked",
                        Present = "talk",
                        UniqueId="Talk1"
                    }
            });
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetCharacter;
            Assert.False(_activeCharacter.CanPerformAction(twoCharsAction));


        }
        [Test]
        public void CharacterNotAbleToPerformTargetCharacterNotAllow()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                    Past = "talked",
                        Present = "talk",
                        UniqueId="Talk1"
                    }
            });
            _targetCharacter.AllowsIndirectTemplateAction = TestHelper.AllowsIndirectNothing;
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetCharacter;
            Assert.False(_activeCharacter.CanPerformAction(twoCharsAction));
        }

        [Test]
        public void CharacterNotAbleToPerformCommunicateNoMessage()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                    Past = "talked",
                        Present = "talk",
                        UniqueId="Talk1"
                    }
            });
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetCharacter;
            Assert.False(_activeCharacter.CanPerformAction(twoCharsAction));
        }

        [Test]
        public void CharacterAbleToPerform()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                    Past = "talked",
                        Present = "talk",
                        UniqueId="Talk1"
                    },
                Message="Hello World!"
            });
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk1");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetCharacter;
            
            Assert.True(_activeCharacter.CanPerformAction(twoCharsAction));

        }
    }
}
