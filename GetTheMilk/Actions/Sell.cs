using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;

namespace GetTheMilk.Actions
{
    public class Sell:AnyTransferTo
    {
        public override string Name
        {
            get { return "Sell"; }
        }

        public override TransactionType TransactionType
        {
            get { return TransactionType.Credit; }
        }
    }
}
