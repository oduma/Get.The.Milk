using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.Levels;
using Sciendo.Common.Logging;

namespace GetTheMilk.Factories
{
    public class LevelsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory,"*Level*")).BasedOn<ILevel>().WithService.Base().LifestyleSingleton()
                    );
            }
            
        }
    }
}
