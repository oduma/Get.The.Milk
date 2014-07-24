using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class MovementActionTemplateTests
    {
        [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new MovementActionTemplate();

            Assert.AreEqual("walk", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new MovementActionTemplate
            {
                PerformerType=typeof( RunActionPerformer),
                Name=new Verb{UniqueId="Run",Past="ran", Present="run"},
                DefaultDistance=3
            };

            Assert.AreEqual("run", defaultActionTemplate.ToString());

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new MovementActionTemplate
            {
                PerformerType=typeof( TeleportActionPerformer),
                Name = new Verb { UniqueId = "EnterLevel", Past = "entered level", Present = "enter level" },
                DefaultDistance = 0,
                TargetCell = 100
            };

            var actual = defaultActionTemplate.Clone() as MovementActionTemplate;
            defaultActionTemplate.TargetCell = 200;
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual(defaultActionTemplate.ToString(),actual.ToString());
Assert.AreEqual(0,actual.TargetCell);
        }
    }
}
