using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ObjectTransferActionTemplateTests
    {
        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                PerformerType = typeof(SellActionPerformer),
                Name = new Verb { UniqueId = "Sell", Past = "sold", Present = "sell" }
            };

            var actual = defaultActionTemplate.Clone() as ObjectTransferActionTemplate;
            defaultActionTemplate.ActiveCharacter= new Player();
            Assert.IsNull(actual.ActiveCharacter);
            Assert.AreEqual(defaultActionTemplate.ToString(),actual.ToString());
        }
    }

    }

