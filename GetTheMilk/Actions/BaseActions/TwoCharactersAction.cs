using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.BaseActions
{
    public abstract class TwoCharactersAction : GameAction
    {
        public abstract bool Perform(ICharacter active, ICharacter passive);
    }
}
