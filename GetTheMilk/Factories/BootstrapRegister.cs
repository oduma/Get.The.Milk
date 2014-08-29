using GetTheMilk.Actions.ActionTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetTheMilk.BaseCommon;
using GetTheMilk.Actions.ActionPerformers.Base;
using Sciendo.IOC;
using System.IO;
using System.Reflection;

namespace GetTheMilk.Factories
{
    public class BootstrapRegister
    {
        public Container RegisterAllComponents()
        {
            AssemblyScanner assemblyScanner = new AssemblyScanner();
            List<Assembly> assemblies = new List<Assembly>();
            foreach(var fileName in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory,@"Get.The.Milk.L*.dll"))
            {
                assemblies.Add(Assembly.LoadFrom(fileName));
            }
            assemblies.Add(Assembly.GetExecutingAssembly());
            Container.GetInstance()
                .Add(assemblyScanner.From(assemblies.ToArray()).BasedOn<BaseActionTemplate>().With(LifeStyle.Singleton).ToArray());
            Container.GetInstance()
                .Add(assemblyScanner.From(assemblies.ToArray()).BasedOn<IActionAllowed>().With(LifeStyle.Singleton).ToArray());
            Container.GetInstance()
                .Add(assemblyScanner.From(assemblies.ToArray()).BasedOn<IActionTemplatePerformer>().With(LifeStyle.Singleton).ToArray());
            return Container.GetInstance();
        }
    }
}
