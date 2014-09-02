using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Common;
using Sciendo.IOC;

namespace GetTheMilk.Factories
{
    public static class ObjectActionsFactory
    {
        public static IEnumerable<string> ListAllRegisterNames(ObjectCategory objectCategory)
        {
            return Container.GetInstance().ResolveAll<IActionAllowed>().Where(c=>c.ObjectCategory==objectCategory).Select(c => c.ObjectTypeId);
        }
        public static IActionAllowed CreateObjectAction(string name)
        {
            return Container.GetInstance().ResolveAll<IActionAllowed>().First(o => o.ObjectTypeId == name);
        }

    }
}
