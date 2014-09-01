using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using NUnit.Framework;
using Sciendo.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests
{
    public class BaseTestClass
    {
        private Container _container;
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            if(_container==null)
            {
                var bootStrapRegister = new BootstrapRegister();
                _container = bootStrapRegister.RegisterAllComponents();
                _container.Add(new AssemblyScanner().From(Assembly.GetAssembly(typeof(BaseTestClass))).BasedOn<IActionTemplatePerformer>().With(LifeStyle.Singleton).ToArray());
            }
        }
    }
}
