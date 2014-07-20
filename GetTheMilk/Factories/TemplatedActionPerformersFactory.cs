using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using Sciendo.Common.IOC;

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

        //public IActionTemplatePerformer CreateActionPerformer<T>(string actionType) where T:DefaultActionTemplate
        //{
        //    var all = _componentResolver.ResolveAll<IActionTemplatePerformer>();
        //    return all.First(o => o.Identifier == actionType && o.TemplateActionType == typeof(T));
        //}


        public T[] GetAllActionPerformers<T>() where T:IActionTemplatePerformer
        {
            var cmps = _componentResolver.ResolveAll<T>();
            return cmps.ToArray();
        }
    }
}
