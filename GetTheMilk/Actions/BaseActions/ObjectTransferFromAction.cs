using GetTheMilk.Accounts;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class ObjectTransferFromAction : GameAction
    {
        public TransactionType TransactionType { get; protected set; }

        public override bool CanPerform()
        {
            return (TargetObject.AllowsIndirectAction(this, ActiveCharacter)
                    && TargetCharacter.AllowsIndirectAction(this, TargetObject)
                    && ActiveCharacter.Walet.CanPerformTransaction(GetTransactionDetails())
                    && (TargetObject.StorageContainer.Owner.Name == TargetCharacter.Name));
        }

        private TransactionDetails GetTransactionDetails()
        {
            return new TransactionDetails
            {
                Price = ((ITransactionalObject)TargetObject).BuyPrice,
                Towards = TargetCharacter,
                TransactionType = TransactionType
            };
        }

        public override ActionResult Perform()
        {
            if (TransactionType != TransactionType.None)
            {
                if(!CanPerform())
                    return new ActionResult
                    {
                        ForAction = this,
                        ResultType = ActionResultType.NotOk
                    };

                if (!ActiveCharacter.Walet.PerformTransaction(GetTransactionDetails()))
                {
                    return new ActionResult
                    {
                        ForAction = this,
                        ResultType = ActionResultType.NotOk
                    };
                }
                var addedOk = ActiveCharacter.Inventory.Add(TargetObject);
                return new ActionResult
                {
                    ForAction = this,
                    ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk
                };
            }
            if (CanPerform())
            {
                var addedOk = ActiveCharacter.Inventory.Add(TargetObject);
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
            return new ObjectTransferFromAction();
        }

    }
}
