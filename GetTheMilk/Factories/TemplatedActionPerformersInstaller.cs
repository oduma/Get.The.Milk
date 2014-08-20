using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using Sciendo.Common.Logging;
using Castle.DynamicProxy;
using GetTheMilk.Utils;
using Castle.Core;

namespace GetTheMilk.Factories
{
    public class TemplatedActionPerformersInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
        Component.For<IInterceptor>()
        .ImplementedBy<PerformerLoggingInterceptor>()
        .Named("PerformerLoggingInterceptor"));
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
            }

        }

        private void Configurator(ComponentRegistration obj)
        {
            obj.Interceptors(InterceptorReference.ForKey("PerformerLoggingInterceptor"));
        }
    }
}
