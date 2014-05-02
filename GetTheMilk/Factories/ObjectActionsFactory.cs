using System.Collections.Generic;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;
using Sciendo.Common.IOC;

namespace GetTheMilk.Factories
{
    public class ObjectActionsFactory
    {
        private readonly ComponentResolver _componentResolver;

        private ObjectActionsFactory()
        {
            _componentResolver = new ComponentResolver();
            _componentResolver.RegisterAll(new ObjectActionsInstaller());
        }

        private static readonly ObjectActionsFactory Instance = new ObjectActionsFactory();
        public static ObjectActionsFactory GetFactory()
        {
            return Instance;
        }

        public IEnumerable<string> ListAllRegisterNames(ObjectCategory objectCategory)
        {
            return _componentResolver.ResolveAll<IActionAllowed>().Where(c=>c.ObjectCategory==objectCategory).Select(c => c.ObjectTypeId);
        }
        public IActionAllowed CreateObjectAction(string name)
        {
            return _componentResolver.ResolveAll<IActionAllowed>().First(o => o.ObjectTypeId == name);
        }

    }
}
