using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Buy:ObjectTransferFromAction
    {
        public Buy()
        {
            Name = new Verb {Infinitive = "To Buy", Past = "bought", Present = "buy"};
            TransactionType = TransactionType.Payed;
            ActionType = ActionType.Buy;
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
            return new Buy();
        }

    }
}
