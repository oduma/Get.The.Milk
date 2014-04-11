using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class TakeFrom : ObjectTransferFromAction
    {
        public TakeFrom()
        {
            Name = new Verb { Infinitive = "To Take", Past = "took", Present = "take" };
            TransactionType = TransactionType.None;
            ActionType = ActionType.TakeFrom;
            StartingAction = false;

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
