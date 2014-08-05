using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ObjectResponseActionTemplateTests
    {
        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new ObjectResponseActionTemplate
            {
                PerformerType = typeof(ObjectResponseActionTemplatePerformer)
            };

            var actual = defaultActionTemplate.Clone() as ObjectResponseActionTemplate;
            defaultActionTemplate.ActiveObject = new NonCharacterObject();
            Assert.IsNull(actual.ActiveObject);

        }

    }
}
