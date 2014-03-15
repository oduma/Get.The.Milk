using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
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

        void SelectWeapons(IEnumerable<Weapon> weapons, ref Weapon activeAttackWeapon, ref Weapon activeDefenseWeapon);

        GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData);
    }
}
