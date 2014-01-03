using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.UI;
using Sciendo.Common.Logging;

namespace GetTheMilk.Factories
{
    public class InteractivityProvidersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
                    Component.For<IInteractivity>().ImplementedBy<TextBasedInteractivityProvider>().Named("TextBased").LifestyleSingleton(),
                    Component.For<IInteractivity>().ImplementedBy<NoInteractivityProvider>().Named("No").LifestyleSingleton(),
                    Component.For<IInteractivity>().ImplementedBy<GraphicBasedInteractivityProvider>().Named(
                        "GraphicBased").LifestyleSingleton()
                    );
            }

        }
    }
}
