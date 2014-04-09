using Castle.Core.Internal;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Settings;
using GetTheMilk.Utils;

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

        }


        public void LoadInteractionsWithPlayer(ICharacter targetCharacter)
        {
            if (!InteractionRules.ContainsKey(targetCharacter.Name.Main)
                && targetCharacter.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
            {
                InteractionRules.Add(targetCharacter.Name.Main,
                                                      targetCharacter.InteractionRules[
                                                          GenericInteractionRulesKeys.PlayerResponses]);
                InteractionRules[targetCharacter.Name.Main].ForEach(ar=> { ar.Action.TargetCharacter = this;
                                                                             ar.Reaction.ActiveCharacter = this;
                });
            }

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
