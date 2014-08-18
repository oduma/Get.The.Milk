﻿using System.Linq;
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

        //public IEnumerable<string> ListAllRegisterNames<T>() where T:DefaultActionTemplate
        //{
        //    return _componentResolver.ResolveAll<IActionTemplatePerformer>().Where(c => c.TemplateActionType == typeof(T)).Select(c => c.Identifier);
        //}

        public IActionTemplatePerformer CreateActionPerformer<T>(string performerType) where T : IActionTemplatePerformer
        {
            var all = _componentResolver.ResolveAll<T>();

            return all.First(o => o.PerformerType == performerType);
        }


        public T[] GetAllActionPerformers<T>() where T:IActionTemplatePerformer
        {
            var cmps = _componentResolver.ResolveAll<T>();
            return cmps.ToArray();
        }

        public IEnumerable<Type> ListAllActionPerformerTypes()
        {
            var tmp = _componentResolver.ResolveAll().Select(p => p.GetType()).ToList();
            return _componentResolver.ResolveAll<IActionTemplatePerformer>().Select(p => p.GetType());
        }
    }
}
