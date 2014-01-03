using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Meet:TwoCharactersAction
    {
        public override string Name
        {
            get { return "Meet"; }
        }

        public override bool Perform(ICharacter active, ICharacter passive)
        {
            if (active is IPlayer)
                ((IPlayer) active).LoadInteractionsWithPlayer(passive);
            else if (passive is IPlayer)
                ((IPlayer)passive).LoadInteractionsWithPlayer(active);

            return true;
        }
    }
}
