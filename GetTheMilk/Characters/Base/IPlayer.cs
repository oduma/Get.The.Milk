using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.GameLevels;

namespace GetTheMilk.Characters.Base
{
    public interface IPlayer:ICharacter
    {

        PerformActionResult EnterLevel(Level level);

        void SetPlayerName(string name);

    }
}
