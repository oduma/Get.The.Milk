namespace GetTheMilk.Characters.BaseCharacters
{
    public interface IPlayer:ICharacter
    {
        void LoadPlayer();

        void SavePlayer();

        void LoadInteractionsWithPlayer(ICharacter targetCharacter);
    }
}
