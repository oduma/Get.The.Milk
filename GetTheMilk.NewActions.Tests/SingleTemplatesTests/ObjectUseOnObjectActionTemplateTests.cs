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
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate();

            Assert.AreEqual("ObjectUseOnObjectActionTemplate No Target Object Assigned using No Active Object Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Open", Past = "opened", Present = "open" },
                DestroyActiveObject=true,
                DestroyTargetObject=true
            };

            Assert.AreEqual("open No Target Object Assigned using No Active Object Assigned", defaultActionTemplate.ToString());

        }
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
