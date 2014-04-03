using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Sell:ObjectTransferToAction
    {
        public Sell()
        {
            Name = new Verb {Infinitive = "To Sell", Past = "sold", Present = "sell"};
            TransactionType = TransactionType.Payed;
            ActionType = ActionType.Sell;
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
            return new Sell();
        }

    }
}
