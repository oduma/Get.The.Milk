using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using Sciendo.Common.IOC;
using System.Collections.Generic;
using System;

namespace GetTheMilk.Factories
{
    public class TemplatedActionPerformersFactory
    {
        private readonly ComponentResolver _componentResolver;

        private TemplatedActionPerformersFactory()
        {
            _componentResolver = new ComponentResolver();
            _componentResolver.RegisterAll(new TemplatedActionPerformersInstaller());
        }

        private static readonly TemplatedActionPerformersFactory Instance = new TemplatedActionPerformersFactory();
        public static TemplatedActionPerformersFactory GetFactory()
        {
            return Instance;
        }

        public IActionTemplatePerformer CreateActionPerformer(string performerType)
        {
            var all = _componentResolver.ResolveAll<IActionTemplatePerformer>();

            return all.First(o => o.PerformerType == performerType);
        }


        public IActionTemplatePerformer[] GetAllActionPerformers()
        {
            return _componentResolver.ResolveAll<IActionTemplatePerformer>().ToArray();
        }

    }
}
