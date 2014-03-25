using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.Actions.BaseActions;
using Sciendo.Common.Logging;

namespace GetTheMilk.Factories
{
    public class ActionsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn<GameAction>().WithService.Base().LifestyleSingleton()
                    );
            }

        }
    }
}
