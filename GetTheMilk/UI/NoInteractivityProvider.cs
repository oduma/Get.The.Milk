using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.UI
{
    public class NoInteractivityProvider:IInteractivity
    {
        public IPositionableObject[] SelectAnObject(IPositionableObject[] availableObjects)
        {
            throw new NotImplementedException();
        }

        public ICharacter SelectACharacter(ICharacter[] availableCharacters)
        {
            throw new NotImplementedException();
        }

        public GameAction SelectAnAction(GameAction[] availableActions, ICharacter targetCharacter)
        {
            var optionId = Randomizer.GetRandom(0,availableActions.Length);
            return availableActions[optionId];
        }

        public GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }

        public void SelectWeapons(List<Weapon> attackWeapons, List<IPositionableObject> rightHandObjects, List<Weapon> defenseWeapons, List<IPositionableObject> leftHandObjects)
        {
            var randomSelector = new Random();
            if(attackWeapons.Any())
            {
                var optionId = randomSelector.Next(0, attackWeapons.Count);
                rightHandObjects.Add(attackWeapons[optionId]);
            }
            if(defenseWeapons.Any(w=>w.Name!=rightHandObjects[0].Name))
            {
                var optionId = randomSelector.Next(0, defenseWeapons.Count);
                leftHandObjects.Add(defenseWeapons[optionId]);
                
            }
        }

        public GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            throw new NotImplementedException();
        }
    }
}
