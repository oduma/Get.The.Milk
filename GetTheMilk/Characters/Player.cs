using System;
using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Settings;
using GetTheMilk.UI;
using GetTheMilk.Utils;

namespace GetTheMilk.Characters
{
    public class Player : Character, IPlayer
    {
        public Player(IInteractivity interactivity)
        {
            var gameSettings = GameSettings.GetInstance();
            Inventory = new Inventory
            {
                InventoryType = InventoryType.CharacterInventory,
                MaximumCapacity = gameSettings.DefaulMaximumCapacity
            };
            BlockMovement = true;
            Experience = GameSettings.GetInstance().MinimumStartingExperience;
            Walet = new Walet
            {
                MaxCapacity = GameSettings.GetInstance().DefaultWalletMaxCapacity,
                CurrentCapacity = gameSettings.MinimumStartingMoney
            };
            Interactivity = interactivity;
            ObjectTypeId = "Player";

            SetPlayerName(gameSettings.DefaultPlayerName);

            Range = gameSettings.DefaultRange;

        }

        public Player()
        {
            var gameSettings = GameSettings.GetInstance();
            Inventory = new Inventory
                            {
                                InventoryType = InventoryType.CharacterInventory,
                                MaximumCapacity = gameSettings.DefaulMaximumCapacity
                            };
            BlockMovement = true;
            Experience = GameSettings.GetInstance().MinimumStartingExperience;
            Walet = new Walet
                        {
                            MaxCapacity = GameSettings.GetInstance().DefaultWalletMaxCapacity,
                            CurrentCapacity = gameSettings.MinimumStartingMoney
                        };
            Interactivity = (new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>(
                                GameSettings.GetInstance().InteractiveMode);
            ObjectTypeId = "Player";

            SetPlayerName(gameSettings.DefaultPlayerName);

            Range = gameSettings.DefaultRange;

        }

        public void LoadInteractionsWithPlayer(ICharacter targetCharacter)
        {
            if (!InteractionRules.ContainsKey(targetCharacter.Name.Main)
                && targetCharacter.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
                InteractionRules.Add(targetCharacter.Name.Main,
                                                      targetCharacter.InteractionRules[
                                                          GenericInteractionRulesKeys.PlayerResponses]);

        }

        public ActionResult EnterLevel(Level level)
        {
            level.Player = this;
            MapNumber = level.StartingMap;
            CellNumber = level.StartingCell;
            return TryPerformMove(new EnterLevel { Direction = Direction.None, TargetCell = level.StartingCell },
                                  level.Maps.FirstOrDefault(m => m.Number == level.StartingMap), level.Objects.Objects,
                                  level.Characters.Characters);
        }

        public void SetPlayerName(string name)
        {
            Name = name != null
                ? new Noun { Main = name, Narrator = GameSettings.GetInstance().DefaultNarratorAddressingForPlayer }
            : new Noun { Main = "Payer 1", Narrator = GameSettings.GetInstance().DefaultNarratorAddressingForPlayer };
        }
    }
}
