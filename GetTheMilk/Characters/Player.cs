using System;
using System.Collections.Generic;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
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

        public Player():this((new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>(
                                GameSettings.GetInstance().InteractiveMode))
        {
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
            if (level.Player == null)
            {
                level.Player = this;
                CellNumber = level.StartingCell;
            }
            return (new EnterLevel { Direction = Direction.None, TargetCell = CellNumber,ActiveCharacter=this, CurrentMap=level.CurrentMap }).Perform();
        }

        public void SetPlayerName(string name)
        {
            Name = name != null
                ? new Noun { Main = name, Narrator = GameSettings.GetInstance().DefaultNarratorAddressingForPlayer }
            : new Noun { Main = "Payer 1", Narrator = GameSettings.GetInstance().DefaultNarratorAddressingForPlayer };
        }

    }
}
