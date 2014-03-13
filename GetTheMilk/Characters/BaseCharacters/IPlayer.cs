using GetTheMilk.Actions;
using GetTheMilk.Levels;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface IPlayer:ICharacter
    {

        void LoadInteractionsWithPlayer(ICharacter targetCharacter);

        ActionResult EnterLevel(Level level);

        void SetPlayerName(string name);

    }
}
