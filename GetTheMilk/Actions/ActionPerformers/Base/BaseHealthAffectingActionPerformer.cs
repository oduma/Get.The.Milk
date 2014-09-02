using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class BaseHealthAffectingActionPerformer
    {
        public event EventHandler<EventArgs> GameOverEvent;

        protected virtual bool NotifyDeath(BaseActionTemplate actionTemplate,Action<Character,Character> robTheDead)
        {
            var charactersInvolved = GetCharactersInvolved(actionTemplate);
            var characters = charactersInvolved as Character[] ?? charactersInvolved.ToArray();
            if (!characters.Any(c => c.Health <= 0))
                return false;
            if (characters.Any(c => c.ObjectTypeId == "Player" && c.Health <= 0))
            {
                if (GameOverEvent != null)
                    GameOverEvent(this, new EventArgs());
                return true;
            }
            var characterAlive = characters.FirstOrDefault(c => c.Health > 0);
            if (characterAlive == null)
            {
                DestroyDeadCharacters(characters);
                return true;
            }
            var deadCharacter = characters.FirstOrDefault(c => c.Health <= 0);
            characterAlive.Experience += CalculationStrategies.CalculateWinExperience(characterAlive, deadCharacter);
            if(robTheDead!=null)
                robTheDead(characterAlive, deadCharacter);
            DestroyDeadCharacters(characters);
            return true;
        }

        private void DestroyDeadCharacters(IEnumerable<Character> charactersInvolved)
        {
            foreach (var character in charactersInvolved.Where(c => c.Health <= 0))
            {
                if(character.StorageContainer!=null)
                    character.StorageContainer.Remove(character);
            }
        }

        private IEnumerable<Character> GetCharactersInvolved(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter != null)
                yield return actionTemplate.ActiveCharacter;
            if (actionTemplate.TargetCharacter != null)
                yield return actionTemplate.TargetCharacter;
        }

    }
}
