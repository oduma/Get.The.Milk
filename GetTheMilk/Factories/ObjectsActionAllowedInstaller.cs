using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.BaseCommon;
using Sciendo.Common.Logging;

namespace GetTheMilk.Factories
{
    public class ObjectActionsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn<IActionAllowed>().WithService.Base().LifestyleSingleton()
                    );
            }
            
        }
    }
}
