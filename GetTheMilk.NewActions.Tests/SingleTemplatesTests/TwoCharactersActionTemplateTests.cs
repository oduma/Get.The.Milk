using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Common;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class TwoCharactersActionTemplateTests
    {
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
