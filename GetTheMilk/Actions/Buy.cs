using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Buy:ObjectTransferAction
    {
        public Buy()
        {
            Name = new Verb {Infinitive = "To Buy", Past = "bought", Present = "buy"};
            TransactionType = TransactionType.Debit;
            ActionType = ActionType.Buy;
        }

        public override bool Perform(ICharacter a, ICharacter p)
        {
            return base.Perform(p, a);
        }

    }
}
