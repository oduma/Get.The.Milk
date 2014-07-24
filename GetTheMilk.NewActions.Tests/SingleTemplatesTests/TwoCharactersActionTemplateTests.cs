using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class TwoCharactersActionTemplateTests
    {
        [Test]
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate();

            Assert.AreEqual("TwoCharactersActionTemplate Target Character Not Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType=typeof( AttackActionPerformer),
                Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" }
            };

            Assert.AreEqual("attack Target Character Not Assigned", defaultActionTemplate.ToString());

            defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType=typeof( CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message="hello"
            };

            Assert.AreEqual("say hello to Target Character Not Assigned", defaultActionTemplate.ToString());



        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType=typeof( CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message = "hello"
            };

            var actual = defaultActionTemplate.Clone() as TwoCharactersActionTemplate;
            defaultActionTemplate.ActiveCharacter = new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual("hello",actual.Message);
            Assert.AreEqual(defaultActionTemplate.ToString(), actual.ToString());
        }
    }
}
