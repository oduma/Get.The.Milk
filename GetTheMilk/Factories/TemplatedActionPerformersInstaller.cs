using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using Sciendo.Common.Logging;

namespace GetTheMilk.Factories
{
    public class TemplatedActionPerformersInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IExposeInventoryActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IMovementActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IOneObjectActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IObjectUseOnObjectActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <IObjectTransferActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <ITwoCharactersActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );
                container.Register(
                    Classes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory)).BasedOn
                        <INoObjectActionTemplatePerformer>().WithService.Base().LifestyleSingleton()
                    );

            }

        }
    }
}
