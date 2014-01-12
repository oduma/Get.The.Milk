using GetTheMilk.Accounts;
using GetTheMilk.Actions.GenericActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class TakeFrom:AnyTransferFrom
    {
        public TakeFrom()
        {
            Name = new Verb {Infinitive = "To Take", Past = "took", Present = "take"};
        }
        public override TransactionType TransactionType
        {
            get { return TransactionType.None; }
        }
    }
}
