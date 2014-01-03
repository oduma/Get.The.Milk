using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;

namespace GetTheMilk.Actions
{
    public class TakeFrom:AnyTransferFrom
    {
        public override string Name
        {
            get { return "TakeFrom"; }
        }

        public override TransactionType TransactionType
        {
            get { return TransactionType.None; }
        }
    }
}
