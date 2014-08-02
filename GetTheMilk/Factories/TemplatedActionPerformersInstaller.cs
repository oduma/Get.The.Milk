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
                        <IExposeInventoryActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IMovementActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IOneObjectActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IObjectUseOnObjectActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IObjectTransferActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <ITwoCharactersActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <INoObjectActionTemplatePerformer>().WithService.Base().Configure(Configurator).LifestyleSingleton()
                    );

            }

        }

        private void Configurator(ComponentRegistration obj)
        {
            obj.Interceptors(InterceptorReference.ForKey("PerformerLoggingInterceptor"));
        }
    }
}
