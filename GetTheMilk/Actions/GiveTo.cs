using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class GiveTo:ObjectTransferToAction
    {
        public GiveTo()
        {
            Name = new Verb {Infinitive = "To Give", Past = "gave", Present = "give"};
            TransactionType = TransactionType.None;
            ActionType = ActionType.GiveTo;
            StartingAction = true;

        }
        public override ActionResult Perform()
        {
            var result = base.Perform();
            result.ForAction = this;
            return result;
        }
        public override GameAction CreateNewInstance()
        {
            return new GiveTo();
        }

    }
}
