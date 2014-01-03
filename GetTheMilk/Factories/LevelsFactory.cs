using System.Linq;
using GetTheMilk.Levels;
using Sciendo.Common.IOC;

namespace GetTheMilk.Factories
{
    public class LevelsFactory
    {
        private readonly ComponentResolver _componentResolver;

        public LevelsFactory()
        {
            _componentResolver = new ComponentResolver();
            _componentResolver.RegisterAll(new LevelsInstaller());
        }

        public ILevel CreateLevel(int levelNumber)
        {
            return _componentResolver.ResolveAll<ILevel>().First(l => l.Number == levelNumber);
        }

    }
}
