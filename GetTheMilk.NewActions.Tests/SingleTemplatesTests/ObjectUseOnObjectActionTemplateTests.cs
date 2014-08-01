using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ObjectUseOnObjectActionTemplateTests
    {
        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Defuse", Past = "defused", Present = "defuse" },
                ChanceOfSuccess = ChanceOfSuccess.Small,
                PercentOfHealthFailurePenalty = 20,
                DestroyTargetObject = true
            }; 
            var actual = defaultActionTemplate.Clone() as ObjectUseOnObjectActionTemplate;
            defaultActionTemplate.ActiveCharacter = new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual(defaultActionTemplate.ToString(), actual.ToString());
        }

    }
}
