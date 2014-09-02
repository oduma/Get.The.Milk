using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.Factories;
using GetTheMilk.GameLevels;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharacterPerformTwoCharactersActionTests
    {
        private Character _activeCharacter;
        private Character _targetCharacter;
        private Character _targetFoeCharacter;

        [SetUp]
        public void SetUp()
        {
            _activeCharacter = new Character
                                                 {
                                                     ObjectTypeId = "NPCFriendly",
                                                     Name =
                                                         new Noun
                                                             {Main = "Active Character", Narrator = "active character"}
                                                 };

            _targetCharacter = new Character { ObjectTypeId = "NPCFriendly", Name = new Noun { Main = "abc", Narrator = "abc" } };
            _targetFoeCharacter = new Character
            {
                ObjectTypeId = "NPCFoe",
                Name =
                    new Noun { Main = "Target For Character", Narrator = "target foe character" }
            };

            var objAction = ObjectActionsFactory.CreateObjectAction("NPCFriendly");
            _activeCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _activeCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            _targetCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _targetCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            objAction = ObjectActionsFactory.CreateObjectAction("NPCFoe");

            _targetFoeCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _targetFoeCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
        }

        [Test]
        public void CharacterPerformsFriendlyAction()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(CommunicateActionPerformer),
                Name = new Verb
                {
                    Past = "talked",
                        Present = "talk",
                        UniqueId="Talk"
                    },
                Message = "Hello World!"
            });
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Talk");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetCharacter;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
        }

        [Test]
        public void CharacterPerformsHostilitiesInitiationAction()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(InitiateHostilitiesActionPerformer),
                Name = new Verb
                {
                    UniqueId = "InitiateHostilities",
                        Past = "attacked",
                        Present = "attack"
                    }
            });
            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("InitiateHostilities");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetFoeCharacter;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
        }

        [Test]
        public void CharacterAttacksAnotherCharacterWithoutAnAttackWeapon()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(AttackActionPerformer),
                Name = new Verb
                {
                    UniqueId = "Attack",
                        Past = "attacked",
                        Present = "attack"
                    }
            });
            _activeCharacter.Health = GameSettings.GetInstance().FullDefaultHealth;
            _targetFoeCharacter.Health = GameSettings.GetInstance().FullDefaultHealth;

            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Attack");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetFoeCharacter;
            var initialActiveHealth = _activeCharacter.Health;
            var initialTargetHealth = _targetFoeCharacter.Health;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.AreEqual(9,_activeCharacter.Health);
            Assert.AreEqual(9, _targetFoeCharacter.Health);
        }

        [Test]
        public void CharacterAttacksAnotherCharacterAndLoses()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(AttackActionPerformer),
                Name = new Verb
                {
                    UniqueId = "Attack",
                        Past = "attacked",
                        Present = "attack"
                    }
            });
            _activeCharacter.Health = 1;
            _targetFoeCharacter.Health = GameSettings.GetInstance().FullDefaultHealth;
            _targetFoeCharacter.Experience = 2;
            _activeCharacter.Experience = 2;

            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Attack");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetFoeCharacter;
            var initialTargetHealth = _targetFoeCharacter.Health;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.AreEqual(initialTargetHealth - 3, _targetFoeCharacter.Health);
            Assert.IsNotNull(twoCharsAction.ActiveCharacter);
            Assert.GreaterOrEqual(0, twoCharsAction.ActiveCharacter.Health);
        }


        [Test]
        public void CharacterAsksToQuitTheBattle()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(TwoCharactersActionTemplatePerformer),
                Name = new Verb
                {
                    UniqueId = "Quit",
                        Past = "quited",
                        Present = "quit"
                    }
            });

            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Quit");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetFoeCharacter;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);

        }

        [Test]
        public void CharacterAcceptsAQuitRequest()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(AcceptQuitActionPerformer),
                Name = new Verb
                {
                    UniqueId = "AcceptQuit",
                        Past = "accepted quit",
                        Present = "accept quit"
                    }
            });
            _activeCharacter.Health = GameSettings.GetInstance().FullDefaultHealth;
            _targetFoeCharacter.Health = GameSettings.GetInstance().FullDefaultHealth;
            _targetFoeCharacter.Experience = 20;
            _activeCharacter.Experience = 20;

            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("AcceptQuit");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetFoeCharacter;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.AreEqual(25,_activeCharacter.Experience);
            Assert.AreEqual(15, _targetFoeCharacter.Experience);

        }

        [Test]
        public void CharacterChangeBlockingStatusOfAnother()
        {
            _activeCharacter.AddAvailableAction(new TwoCharactersActionTemplate
            {
                PerformerType=typeof(AllowPassActionPerformer),
                Name = new Verb
                {
                    UniqueId = "AllowPass",
                        Past = "allowed pass",
                        Present = "allow pass"
                    }
            });
            _activeCharacter.BlockMovement = true;

            var twoCharsAction = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("AllowPass");
            Assert.IsNotNull(twoCharsAction);
            Assert.AreEqual(typeof(TwoCharactersActionTemplate), twoCharsAction.GetType());
            twoCharsAction.ActiveCharacter = _activeCharacter;
            twoCharsAction.TargetCharacter = _targetFoeCharacter;
            var result = _activeCharacter.PerformAction(twoCharsAction);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.False(_activeCharacter.BlockMovement);
            
        }
    }
}
