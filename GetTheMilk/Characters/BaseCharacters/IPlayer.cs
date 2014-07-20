using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Levels;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface IPlayer:ICharacter
    {

        PerformActionResult EnterLevel(Level level);

        void SetPlayerName(string name);

    }
}
