using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;

namespace GetTheMilk.Actions
{
    public class Buy:AnyTransferFrom
    {
        public override string Name
        {
            get { return "Buy"; }
        }

        public override TransactionType TransactionType
        {
            get { return TransactionType.Debit; }
        }
    }
}
