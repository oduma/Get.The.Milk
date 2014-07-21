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

            Assert.AreEqual("OneObjectActionTemplate-Empty One Object Action", defaultActionTemplate.ToString());
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
        public void SerializeEmptyDefaultActionTemplate()
        {
            var defaultActionTemplate = new OneObjectActionTemplate();
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"Category\":\"OneObjectActionTemplate\",\"Name\":{\"Identifier\":\"Empty One Object Action\",\"Present\":null,\"Past\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}", result);
        }

        [Test]
        public void SerializeActionTemplate()
        {
            var defaultActionTemplate = new OneObjectActionTemplate
            {
                CurrentPerformer = new SelectAttackWeaponActionPerformer(),
                Name = new Verb
                {
                        UniqueId = "SelectAttackWeapon",
                        Past = "selected attack weapon",
                        Present = "select attack weapon"
                    }
            };
            var result = JsonConvert.SerializeObject(defaultActionTemplate);

            Assert.AreEqual("{\"Name\":{\"UniqueId\":\"SelectAttackWeapon\",\"Present\":\"select attack weapon\",\"Past\":\"selected attack weapon\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.SelectAttackWeaponActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}", result);


        }

        [Test]
        public void DeSerializeEmptyActionTemplate()
        {
            var expected = new OneObjectActionTemplate();
            var result =
                JsonConvert.DeserializeObject<OneObjectActionTemplate>(
                    "{\"Name\":{\"Identifier\":\"Empty One Object Action\",\"Present\":null,\"Past\":null,\"Category\":null},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}");

            Assert.AreEqual(expected.ToString(), result.ToString());

        }
        [Test]
        public void DeSerializeActionTemplate()
        {
            var expected = new OneObjectActionTemplate
            {
                CurrentPerformer = new SelectAttackWeaponActionPerformer(),
                Name = new Verb
                {
                    UniqueId = "SelectAttackWeapon",
                        Past = "selected attack weapon",
                        Present = "select attack weapon"
                    }
            };
            var result =
                JsonConvert.DeserializeObject<OneObjectActionTemplate>(
                    "{\"Name\":{\"UniqueId\":\"SelectAttackWeapon\",\"Present\":\"select attack weapon\",\"Past\":\"selected attack weapon\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.SelectAttackWeaponActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}");

            Assert.AreEqual(expected.ToString(), result.ToString());

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
