using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Sell:ObjectTransferAction
    {
        public Sell()
        {
            Name = new Verb {Infinitive = "To Sell", Past = "sold", Present = "sell"};
            TransactionType = TransactionType.Credit;
            ActionType = ActionType.Sell;
        }

        public override bool Perform(ICharacter a, ICharacter p)
        {
            return base.Perform(a, p);
        }

    }
}
