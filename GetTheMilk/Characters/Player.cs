using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.Settings;

namespace GetTheMilk.Characters
{
    public class Player : Character, IPlayer
    {
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
            ObjectTypeId = "Player";

            SetPlayerName(gameSettings.DefaultPlayerName);

            Range = gameSettings.DefaultRange;
            foreach(var action in GameSettings.GetInstance().StandardPlayerActions)
                AddAvailableAction(action);
        }

        public PerformActionResult EnterLevel(Level level)
        {
            if (level.Player == null)
            {
                level.Player = this;
                CellNumber = level.StartingCell;
            }
            var newAction = CreateNewInstanceOfAction<MovementActionTemplate>("EnterLevel");
            newAction.ActiveCharacter = this;
            newAction.TargetCell = CellNumber;
            newAction.CurrentMap = level.CurrentMap;
            return PerformAction(newAction);
        }

        public void SetPlayerName(string name)
        {
            Name = name != null
                ? new Noun { Main = name, Narrator = GameSettings.GetInstance().DefaultNarratorAddressingForPlayer }
            : new Noun { Main = "Payer 1", Narrator = GameSettings.GetInstance().DefaultNarratorAddressingForPlayer };
        }

        protected override bool IsNonStandardActionTemplate(BaseActionTemplate baseActionTemplate)
        {
            return
                (!(GameSettings.GetInstance().AllCharactersActions.Any(
                    a =>
                    a.Name.UniqueId == baseActionTemplate.Name.UniqueId)) &&
                 !(GameSettings.GetInstance().StandardPlayerActions.Any(
                     a =>
                     a.Name.UniqueId == baseActionTemplate.Name.UniqueId)));

        }
    }
}
