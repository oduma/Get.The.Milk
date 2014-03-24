using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class TakeFrom : ObjectTransferAction
    {
        public TakeFrom()
        {
            Name = new Verb { Infinitive = "To Give", Past = "gave", Present = "give" };
            TransactionType = TransactionType.None;
            ActionType = ActionType.TakeFrom;
        }
        public override ActionResult Perform()
        {
            var result = base.Perform();
            result.ForAction = this;
            return result;
        }

        public override GameAction CreateNewInstance()
        {
            return new TakeFrom();
        }

    }
}
