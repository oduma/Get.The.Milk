using GetTheMilk.Actions;
using GetTheMilk.Levels;

namespace GetTheMilk.Characters.BaseCharacters
{
    public interface IPlayer:ICharacter
    {
        void LoadPlayer();

        void SavePlayer();

        void LoadInteractionsWithPlayer(ICharacter targetCharacter);

        ActionResult EnterLevel(ILevel level);


    }
}
