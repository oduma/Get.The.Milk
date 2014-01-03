using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;

namespace GetTheMilk.Actions
{
    public class GiveTo:AnyTransferTo
    {
        public override string Name
        {
            get { return "GiveTo"; }
        }

        public override TransactionType TransactionType
        {
            get { return TransactionType.None; }
        }
    }
}
