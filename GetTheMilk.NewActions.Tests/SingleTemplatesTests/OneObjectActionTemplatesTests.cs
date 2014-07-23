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
        public void EmptyDefaultActionToString()
        {
            var defaultActionTemplate = new OneObjectActionTemplate();

            Assert.AreEqual("OneObjectActionTemplate", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyActionToString()
        {
            var defaultActionTemplate = new OneObjectActionTemplate
                                            {
                                                ActiveCharacter = new Player(),
                                                TargetObject =
                                                    new Weapon
                                                        {
                                                            Name = new Noun {Main = "Super Weapon", Narrator = "super weapon"}
                                                        },
                                                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                                                Name = new Verb
                                                {
                                                    UniqueId = "SelectAttackWeapon",
                                                            Past = "selected attack weapon",
                                                            Present = "select attack weapon"
                                                        }
                                            };

            Assert.AreEqual("select attack weapon super weapon", defaultActionTemplate.ToString());

        }

        [Test]
        public void CloneActionTemplate()
        {
            var defaultActionTemplate = new OneObjectActionTemplate
            {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                        UniqueId = "SelectAttackWeapon",
                        Past = "selected attack weapon",
                        Present = "select attack weapon"
                    }
            };

            var actual = defaultActionTemplate.Clone();
            defaultActionTemplate.ActiveCharacter=new Player();
            Assert.AreEqual("select attack weapon",actual.ToString());
            Assert.IsNull(actual.ActiveCharacter);
        }
    }
}
