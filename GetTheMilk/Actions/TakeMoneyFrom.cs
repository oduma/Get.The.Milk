using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class TakeMoneyFrom:TwoCharactersAction
    {
        public override bool CanPerform()
        {
            return ActiveCharacter.AllowsAction(this)
                   && TargetCharacter.AllowsIndirectAction(this, ActiveCharacter)
                   && ActiveCharacter.Walet.MaxCapacity >= ActiveCharacter.Walet.CurrentCapacity + Amount;
        }

        public TakeMoneyFrom()
        {
            Name = new Verb {Infinitive = "To Take Money", Past = "took money", Present = "take money"};
            ActionType = ActionType.TakeMoneyFrom;
        }
        public int Amount { get; set; }
        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };

            TargetCharacter.Walet.PerformTransaction(new TransactionDetails
                                                       {
                                                           Price = Amount,
                                                           Towards=ActiveCharacter,
                                                           TransactionType = TransactionType.Debit
                                                       });
            return new ActionResult {ForAction = this, ResultType = ActionResultType.Ok};
        }
        public override GameAction CreateNewInstance()
        {
            return new TakeMoneyFrom();
        }

    }
}
