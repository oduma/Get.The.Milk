using GetTheMilk.Accounts;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class ObjectTransferToAction : GameAction
    {
        public TransactionType TransactionType { get; protected set; }

        public override bool CanPerform()
        {
            return (TargetObject.AllowsIndirectAction(this, ActiveCharacter)
                    && TargetCharacter.AllowsIndirectAction(this, TargetObject)
                    && TargetCharacter.Walet.CanPerformTransaction(GetTransactionDetails())
                    && (TargetObject.StorageContainer.Owner.Name == ActiveCharacter.Name));
        }

        private TransactionDetails GetTransactionDetails()
        {
            return new TransactionDetails
            {
                Price = ((ITransactionalObject)TargetObject).SellPrice,
                Towards = ActiveCharacter,
                TransactionType = TransactionType
            };
        }

        public override ActionResult Perform()
        {
            if (TransactionType != TransactionType.None)
            {
                if (!CanPerform())
                    return new ActionResult
                    {
                        ForAction = this,
                        ResultType = ActionResultType.NotOk
                    };

                if (!TargetCharacter.Walet.PerformTransaction(GetTransactionDetails()))
                {
                    return new ActionResult
                    {
                        ForAction = this,
                        ResultType = ActionResultType.NotOk
                    };
                }
                var addedOk = TargetCharacter.Inventory.Add(TargetObject);
                return new ActionResult
                {
                    ForAction = this,
                    ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk
                };
            }
            if (CanPerform())
            {
                var addedOk = TargetCharacter.Inventory.Add(TargetObject);
                return new ActionResult
                {
                    ForAction = this,
                    ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk
                };
            }
            return new ActionResult
            {
                ForAction = this,
                ResultType = ActionResultType.NotOk
            };

        }

        public override GameAction CreateNewInstance()
        {
            return new ObjectTransferToAction();
        }

    }
}
