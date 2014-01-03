using Castle.MicroKernel.Registration;
using Sciendo.Common.IOC;

namespace GetTheMilk.Factories
{
    public class ObjectsFactory
    {
        private readonly ComponentResolver _componentResolver;

        public ObjectsFactory(IWindsorInstaller installer)
        {
            _componentResolver = new ComponentResolver();
            _componentResolver.RegisterAll(installer);
        }

        public T CreateObject<T>(string name)
        {
            return _componentResolver.Resolve<T>(name);
        }

        public T[] CreateObjects<T>()
        {
            return _componentResolver.ResolveAll<T>();
        }
    }
}
