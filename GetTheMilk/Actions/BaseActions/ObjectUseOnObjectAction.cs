using GetTheMilk.Utils;

namespace GetTheMilk.Actions.BaseActions
{
    public class ObjectUseOnObjectAction : GameAction
    {
        protected bool DestroyActiveObject { get; set; }

        protected bool DestroyTargetObject { get; set; }

        protected ChanceOfSuccess ChanceOfSuccess { get; set; }

        protected int PercentOfHealthFailurePenalty { get; set; }

        public override bool CanPerform()
        {
            return (ActiveObject.AllowsAction(this) && TargetObject.AllowsIndirectAction(this, ActiveObject));
        }

        public override ActionResult Perform()
        {
            bool success = (ChanceOfSuccess == ChanceOfSuccess.Full) || CalculationStrategies.CalculateSuccessOrFailure(ChanceOfSuccess,
                                                                                                     ActiveCharacter.Experience);
            if (DestroyActiveObject || !success)
            {
                if(ActiveObject.StorageContainer!=null)
                    ActiveObject.StorageContainer.Remove(ActiveObject);
                ActiveObject = null;
            }
            if (DestroyTargetObject || !success)
            {
                if(TargetObject.StorageContainer!=null)
                    TargetObject.StorageContainer.Remove(TargetObject);
                TargetObject = null;
            }
            if(!success)
            {
                ActiveCharacter.Health -= (ActiveCharacter.Health*PercentOfHealthFailurePenalty/100);
            }
            return new ActionResult {ForAction = this, ResultType = (success)?ActionResultType.Ok:ActionResultType.NotOk};
        }

        public ObjectUseOnObjectAction()
        {
            ChanceOfSuccess = ChanceOfSuccess.Full;
        }

        public override GameAction CreateNewInstance()
        {
            return new ObjectUseOnObjectAction();
        }

    }
}
