using System.Collections.Generic;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Fight;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI;
using Newtonsoft.Json;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface ICharacter:IPositionable
    {
        int Health { get; set; }
        int Experience { get; set; }
        int Range { get; set; }
        [JsonIgnore]
        CharacterCollection StorageContainer { get; set; }
        IInteractivity Interactivity { get;}
        [JsonIgnore]
        Inventory Inventory { get; set; }
        Walet Walet { get; set; }
        [JsonIgnore]
        SortedList<string, ActionReaction[]> InteractionRules { get; set; }
        void PrepareForBattle();
        Hit PrepareDefenseHit();
        Hit PrepareAttackHit();
        Weapon ActiveAttackWeapon { get; set; }
        Weapon ActiveDefenseWeapon { get; set; }
        GameAction ChooseAction(GameAction[] actions, ICharacter targetCharacter);
        GameAction ChooseFromAnotherInventory(ExposeInventoryExtraData extraData);
        ActionResult StartInteraction(GameAction startingAction);
        GameAction TryContinueInteraction(GameAction incomingAction, ICharacter targetCharacter);
        ActionResult PileageCharacter(ICharacter targetCharacter, ActionResultType actionResultType);
        CharacterSavedPackages Save();

    }
}