using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Meet:TwoCharactersAction
    {
        public Meet()
        {
            Name = new Verb {Infinitive = "To Meet", Past = "meet", Present = "meet"};
            ActionType = ActionType.Meet;
        }
        public override bool Perform(Character active, Character passive)
        {
            if (active is IPlayer)
                ((IPlayer) active).LoadInteractionsWithPlayer(passive);
            else if (passive is IPlayer)
                ((IPlayer)passive).LoadInteractionsWithPlayer(active);

            return true;
        }
    }
}
