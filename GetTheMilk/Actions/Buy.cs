using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Buy:AnyTransferFrom
    {
        public Buy()
        {
            Name = new Verb {Infinitive = "To Buy", Past = "bought", Present = "buy"};
        }
        public override TransactionType TransactionType
        {
            get { return TransactionType.Debit; }
        }
    }
}
