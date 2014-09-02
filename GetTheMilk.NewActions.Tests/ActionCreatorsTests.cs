using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests
{
    [TestFixture]
    public class ActionCreatorsTests:BaseTestClass
    {
        [Test]
        public void ObjectActionsFactories_Ok()
        {
            var allRegisteredNames = ObjectActionsFactory.ListAllRegisterNames(ObjectCategory.Tool);
            Assert.IsNotNull(allRegisteredNames);
            Assert.Contains("CanOpener", allRegisteredNames.ToList());
        }

        [Test]
        public void GetAListOfPerformersFromFactory()
        {
            var performers =
                TemplatedActionPerformersFactory.GetAllActionPerformers();
            Assert.IsNotNull(performers);
            Assert.AreEqual(25, performers.Length);
        }

        [Test]
        public void ActionsFactoryCreatesSeparateActions()
        {

            var action1 = ActionsFactory.CreateAction(CategorysCatalog.OneObjectCategory);
            var action2 = ActionsFactory.CreateAction(CategorysCatalog.OneObjectCategory);
            Assert.IsNotNull(action1);
            Assert.IsNotNull(action2);
            action1.Name = new Verb { UniqueId = "abc" };
            Assert.AreEqual("abc", action1.Name.UniqueId);
            Assert.IsNull(action2.Name);
        }
    }
}
