using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using System.Collections.Generic;
using System;
using Sciendo.IOC;

namespace GetTheMilk.Factories
{
    public static class TemplatedActionPerformersFactory
    {
        public static IActionTemplatePerformer CreateActionPerformer(string performerType)
        {
            return Container.GetInstance().ResolveAll<IActionTemplatePerformer>().First(o=>o.PerformerType==performerType);
        }


        public static IActionTemplatePerformer[] GetAllActionPerformers()
        {
            return Container.GetInstance().ResolveAll<IActionTemplatePerformer>().ToArray();
        }

    }
}
