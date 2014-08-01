using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using Newtonsoft.Json;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Utils;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class OneObjectActionTemplatesTests
    {
        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new OneObjectActionTemplate
            {
                PerformerType=typeof( OneObjectActionTemplatePerformer),
                Name = new Verb
                {
                        UniqueId = "SelectAttackWeapon",
                        Past = "selected attack weapon",
                        Present = "select attack weapon"
                    }
            };

            var actual = defaultActionTemplate.Clone();
            defaultActionTemplate.ActiveCharacter=new Player();
            Assert.AreEqual("select attack weapon No Target Object",actual.ToString());
            Assert.IsNull(actual.ActiveCharacter);
        }
    }
}
