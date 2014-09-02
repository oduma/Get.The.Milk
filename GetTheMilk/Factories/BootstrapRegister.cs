using GetTheMilk.Actions.ActionTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Common;
using Sciendo.IOC;
using System.IO;
using System.Reflection;

namespace GetTheMilk.Factories
{
    public class BootstrapRegister
    {
        public Container RegisterAllComponents()
        {
            var assemblyScanner = new AssemblyScanner();
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, @"Get.The.Milk.L*.dll").Select(fileName => Assembly.LoadFrom(fileName)).ToList();
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
