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
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate();

            Assert.AreEqual("ObjectTransferActionTemplate Target Object Not Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                PerformerType=typeof( BuyActionPerformer),
                Name=new Verb{UniqueId="Buy",Past="bought", Present="buy"}
            };

            Assert.AreEqual("buy Target Object Not Assigned", defaultActionTemplate.ToString());

        }

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

