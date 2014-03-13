using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class GiveTo:ObjectTransferAction
    {
        public GiveTo()
        {
            Name = new Verb {Infinitive = "To Give", Past = "gave", Present = "give"};
            TransactionType = TransactionType.None;
            ActionType = ActionType.GiveTo;
        }
        public override bool Perform(ICharacter a, ICharacter p)
        {
            return base.Perform(a, p);
        }

    }
}
