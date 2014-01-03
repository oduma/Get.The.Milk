using System.Collections.Generic;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface ICharacter:IPositionableObject
    {
        IInteractivity Interactivity { get;}
        int Health { get; set; }
        int Experience { get; set; }
        Inventory ToolInventory { get; }
        Inventory WeaponInventory { get; }
        Walet Walet { get; }
        Personality Personality { get; }
        int Range { get; }
        Inventory RightHandObject { get; }
        Inventory LeftHandObject { get; }
        ActionResult TryPerformAction(GameAction action, ICharacter passive);
        ActionResult TryPerformAction(GameAction action, params IPositionableObject[] targetObjects);
        ActionResult TryPerformAction(ObjectTransferAction action, ICharacter targetCharacter);
        ActionResult TryPerformAction(FightAction action, ICharacter targetCharacter);
        ActionResult TryPerformAction(TwoCharactersAction action, ICharacter targetCharacter);
        ActionResult TryPerformAction(CommunicateAction action, ICharacter targetCharacter);
        ActionResult TryPerformAction(ExposeInventory action, ICharacter targetCharacter);
        ActionResult TryPerformAction(TakeMoneyFrom action, ICharacter targetCharacter);

        ActionResult TryPerformObjectOnObjectAction(ObjectUseOnObjectAction action,
                                                    IPositionableObject passiveTargetObject);
        bool TryAnySuitableInventories(IPositionableObject targetObject);

        ActionResult TryPerformMove(MovementAction movement, Map currentMap,
                                      IEnumerable<IPositionableObject> allLevelObjects,
                                      IEnumerable<IPositionableObject> allLevelCharacters);

        void PrepareForBattle();
        IPositionableObject ChooseTool();
        GameAction ChooseAction(GameAction[] actions, ICharacter targetCharacter);
        GameAction ChooseFromAnotherInventory(ExposeInventoryExtraData extraData);
        ActionResult StartInteraction(GameAction startingAction, ICharacter targetCharacter);
        GameAction TryContinueInteraction(GameAction incomingAction, ICharacter targetCharacter);
        ActionResult PileageCharacter(ICharacter targetCharacter, ActionResultType actionResultType);
        Hit PrepareDefenseHit();
        Hit PrepareAttackHit();
    }
}