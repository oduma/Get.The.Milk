using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface IPlayer:ICharacter
    {

        void LoadInteractionsWithPlayer(ICharacter targetCharacter);

        ActionResult EnterLevel(Level level);

        void SetPlayerName(string name);

    }
}
