using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class BaseHealthAffectingActionPerformer
    {
        public event EventHandler<EventArgs> GameOverEvent;

        protected virtual bool NotifyDeath(BaseActionTemplate actionTemplate,Action<Character,Character> robTheDead)
        {
            var charactersInvolved = GetCharactersInvolved(actionTemplate);
            if (!charactersInvolved.Any(c => c.Health <= 0))
                return false;
            if (charactersInvolved.Any(c => c.ObjectTypeId == "Player" && c.Health <= 0))
            {
                if (GameOverEvent != null)
                    GameOverEvent(this, new EventArgs());
                return true;
            }
            else
            {
                var characterAlive = charactersInvolved.FirstOrDefault(c => c.Health > 0);
                if (characterAlive == null)
                {
                    DestroyDeadCharacters(charactersInvolved);
                    return true;
                }
                var deadCharacter = charactersInvolved.FirstOrDefault(c => c.Health <= 0);
                characterAlive.Experience += CalculationStrategies.CalculateWinExperience(characterAlive, deadCharacter);
                if(robTheDead!=null)
                    robTheDead(characterAlive, deadCharacter);
                DestroyDeadCharacters(charactersInvolved);
                return true;
            }
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
