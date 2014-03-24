using System.Linq;
using GetTheMilk.Actions.BaseActions;
using Sciendo.Common.IOC;

namespace GetTheMilk.Factories
{
    public class ActionsFactory
    {
        private readonly ComponentResolver _componentResolver;

        private ActionsFactory()
        {
            _componentResolver = new ComponentResolver();
            _componentResolver.RegisterAll(new ActionsInstaller());
        }

        private static readonly ActionsFactory Instance = new ActionsFactory();
        public static ActionsFactory GetFactory()
        {
            return Instance;
        }

        public GameAction[] GetActions()
        {
            return _componentResolver.ResolveAll<GameAction>();
        }

        public GameAction CreateNewActionInstance(ActionType actionType)
        {
            return _componentResolver.ResolveAll<GameAction>().FirstOrDefault(a => a.ActionType == actionType).CreateNewInstance();
        }
    }
}
