using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.Factories;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class ObjectssAbleToPerformObjectResponseActionsTests
    {
        private Character _character;

        [SetUp]
        public void SetUp()
        {
            _character = new Character { ObjectTypeId = "NPCFriendly" };


            var objAction = ObjectActionsFactory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

        }

        [Test]
        public void CharacterNotAbleToPerformObjectResponseAction()
        {
            var objectResponseActionTemplate = new ObjectResponseActionTemplate { Name = new Verb { UniqueId = "Crack", Past = "cracked", Present = "crack" }, PerformerType = typeof(ObjectResponseActionTemplatePerformer) };
            _character.AddAvailableAction(objectResponseActionTemplate);
            var useAction = _character.CreateNewInstanceOfAction<ObjectResponseActionTemplate>("Crack");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectResponseActionTemplate), useAction.GetType());
            Assert.False(_character.CanPerformAction(useAction));

        }

        [Test]
        public void ObjectNotAbleToPerformActiveObjectNotAllow()
        {
            var activeObject = new NonCharacterObject { Name = new Noun { Main = "Dawn", Narrator = "the dawn" }, AllowsTemplateAction = TestHelper.AllowsNothing };
            activeObject.AddAvailableAction(new ObjectResponseActionTemplate { Name = new Verb { UniqueId = "Crack", Past = "cracked", Present = "crack" }, PerformerType = typeof(ObjectResponseActionTemplatePerformer) });
            var useAction = activeObject.CreateNewInstanceOfAction<ObjectResponseActionTemplate>("Crack");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectResponseActionTemplate), useAction.GetType());

            Assert.False(activeObject.CanPerformAction(useAction));

        }
        [Test]
        public void ObjectAbleToPerform()
        {
            var activeObject = new NonCharacterObject { Name = new Noun { Main = "Dawn", Narrator = "the dawn" }, AllowsTemplateAction = TestHelper.AllowsEverything };
            activeObject.AddAvailableAction(new ObjectResponseActionTemplate { Name = new Verb { UniqueId = "Crack", Past = "cracked", Present = "crack" }, PerformerType = typeof(ObjectResponseActionTemplatePerformer) });
            var useAction = activeObject.CreateNewInstanceOfAction<ObjectResponseActionTemplate>("Crack");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectResponseActionTemplate), useAction.GetType());

            Assert.True(activeObject.CanPerformAction(useAction));
        }
    }
}
