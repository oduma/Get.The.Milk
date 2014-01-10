using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.UI
{
    public interface IInteractivity
    {
        IPositionableObject[] SelectAnObject(IPositionableObject[] availableObjects);

        ICharacter SelectACharacter(ICharacter[] availableCharacters);

        GameAction SelectAnAction(GameAction[] availableObjects, ICharacter targetCharacter);

        GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData);

        void SelectWeapons(List<Weapon> attackWeapons, List<IPositionableObject> rightHandObjects,
                           List<Weapon> defenseWeapons, List<IPositionableObject> leftHandObjects);

        GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData);

        void EnterLevel(IPlayer player, ILevel level);
    }
}
