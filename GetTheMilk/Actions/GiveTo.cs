using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class GiveTo:AnyTransferTo
    {
        public GiveTo()
        {
            Name = new Verb {Infinitive = "To Give", Past = "gave", Present = "give"};
        }
        public override TransactionType TransactionType
        {
            get { return TransactionType.None; }
        }
    }
}
