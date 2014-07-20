using System;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using NUnit.Framework;

namespace Get.The.Milk.NewActionsModel.Tests
{
    [TestFixture]
    public class GameActionPerformersTests
    {

        [Test]
        public void GetATemplatedActionPerformerFromMainAssembly()
        {
            var actionPerformer = TemplatedActionPerformersFactory<DefaultActionTemplate>.GetFactory().CreateActionPerformer(ActionType.Default);
            Assert.IsNotNull(actionPerformer);
            Assert.AreEqual(typeof(DefaultActionTemplate), actionPerformer.TemplateActionType);
            Assert.AreEqual(ActionType.Default, actionPerformer.Identifier);
            var actionPerformer1 = TemplatedActionPerformersFactory<OneObjectActionTemplate>.GetFactory().CreateActionPerformer(ActionType.Default);
            Assert.IsNotNull(actionPerformer1);
            Assert.AreEqual(typeof(OneObjectActionTemplate), actionPerformer1.TemplateActionType);
            Assert.AreEqual(ActionType.Default, actionPerformer1.Identifier);
        }

        [Test]
        public void TemplateActionPerformersSeriliazedDeserilaizedWithCharacters()
        {
            var character = new Character { ObjectTypeId = "NPCFriendly" };
            var packages = character.Save();
            Character actualCharacter = Character.Load<Character>(packages);
            Assert.IsNotNull(actualCharacter);
            Assert.IsNotNull(actualCharacter.DefaultActionTemplatePerformers);
            Assert.AreEqual(1, actualCharacter.DefaultActionTemplatePerformers.Length);
            Assert.IsNotNull(actualCharacter.OneObjectActionTemplatePerformers);
            Assert.AreEqual(1, actualCharacter.OneObjectActionTemplatePerformers.Length);
        }

        [Test]
        public void ActionPerformersSeriliazedDeserilaizedWithCharacters()
        {
            var character = new Character {ObjectTypeId = "NPCFriendly"};
            var packages = character.Save();
            Character actualCharacter = Character.Load<Character>(packages);
            Assert.IsNotNull(actualCharacter);
            Assert.IsNotNull(actualCharacter.GameActionPerformers);
            Assert.AreEqual(1,actualCharacter.GameActionPerformers.Length);
        }
    }
}
