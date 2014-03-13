using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.UI.Translators.Common;
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

        public IActionAllowed CreateObjectAction(string name)
        {
            return _componentResolver.ResolveAll<IActionAllowed>().First(o => o.ObjectTypeId == name);
        }

    }
}
