using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.UI
{
    public interface IInteractivity
    {
        NonCharacterObject[] SelectAnObject(NonCharacterObject[] availableObjects);

        ICharacter SelectACharacter(ICharacter[] availableCharacters);

        GameAction SelectAnAction(GameAction[] availableObjects, ICharacter targetCharacter);

        GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData);

        void SelectWeapons(List<Weapon> attackWeapons, Weapon activeAttackWeapon,
                           List<Weapon> defenseWeapons, Weapon activeDefenseWeapon);

        GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData);
    }
}
