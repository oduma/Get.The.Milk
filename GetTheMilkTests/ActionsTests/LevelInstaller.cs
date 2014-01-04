using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using Sciendo.Common.Logging;


namespace GetTheMilkTests.ActionsTests
{
    public class LevelInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            LoggingManager.Debug("Installing all components.");

            using (LoggingManager.LogSciendoPerformance())
            {
                container.Register(
                    Component.For<IPositionableObject>().ImplementedBy<Wall>().Named("Wall"),
                    Component.For<IPositionableObject>().ImplementedBy<RedDoor>().Named("RedDoor"),
                    Component.For<ITransactionalObject>().ImplementedBy<RedKey>().Named("Red Key"),
                    Component.For<ITransactionalObject>().ImplementedBy<BlueKey>().Named("BlueKey"),
                    Component.For<ITransactionalObject>().ImplementedBy<ScrewDriver>().Named("SkrewDriver"),
                    Component.For<IPositionableObject>().ImplementedBy<Window>().Named("Window"),
                    Component.For<ICharacter>().ImplementedBy<KeyMaster>().Named("Key Master"),
                    Component.For<ICharacter>().ImplementedBy<KeylessChild>().Named("Keyless Child"),
                    Component.For<ICharacter>().ImplementedBy<ShopKeeper>().Named("Shop Keeper"),
                    Component.For<OneObjectAction>().ImplementedBy<Pick>().Named("Pick"),
                    Component.For<OneObjectAction>().ImplementedBy<Kick>().Named("Kick"),
                    Component.For<OneObjectAction>().ImplementedBy<Keep>().Named("Keep"),
                    Component.For<ObjectTransferAction>().ImplementedBy<GiveTo>().Named("GiveTo"),
                    Component.For<ObjectTransferAction>().ImplementedBy<Buy>().Named("Buy"),
                    Component.For<ObjectTransferAction>().ImplementedBy<Sell>().Named("Sell"),
                    Component.For<ObjectUseOnObjectAction>().ImplementedBy<Open>().Named("Open"),
                    Component.For<MovementAction>().ImplementedBy<Walk>().Named("Walk"),
                    Component.For<MovementAction>().ImplementedBy<Run>().Named("Run"),
                    Component.For<TwoCharactersAction>().ImplementedBy<Meet>().Named("Meet")
                    );
            }
        }
    }
}
