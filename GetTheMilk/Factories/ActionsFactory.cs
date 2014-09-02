using GetTheMilk.Actions.ActionTemplates;
using Sciendo.IOC;
using System;
using System.Linq;

namespace GetTheMilk.Factories
{
    public static class ActionsFactory
    {
        public static BaseActionTemplate CreateAction(string actionCategory)
        {
            var action = Container.GetInstance().ResolveAll<BaseActionTemplate>().FirstOrDefault(a=>a.Category==actionCategory);
            if (action == null)
                throw new NotImplementedException();
            return action.Clone();
        }
    }
}
