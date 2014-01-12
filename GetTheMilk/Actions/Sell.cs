using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Sell:AnyTransferTo
    {
        public Sell()
        {
            Name = new Verb {Infinitive = "To Sell", Past = "sold", Present = "sell"};
        }
        public override TransactionType TransactionType
        {
            get { return TransactionType.Credit; }
        }
    }
}
