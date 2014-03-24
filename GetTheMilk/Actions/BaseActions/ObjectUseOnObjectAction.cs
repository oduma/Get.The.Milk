namespace GetTheMilk.Actions.BaseActions
{
    public class ObjectUseOnObjectAction : GameAction
    {
        public bool DestroyActiveObject { get; set; }

        public bool DestroyTargetObject { get; set; }

        public override bool CanPerform()
        {
            return (ActiveObject.AllowsAction(this) && TargetObject.AllowsIndirectAction(this, ActiveObject));
        }

        public override ActionResult Perform()
        {
            if (DestroyActiveObject)
            {
                ActiveObject.StorageContainer.Remove(ActiveObject);
                ActiveObject = null;
            }
            if (DestroyTargetObject)
            {
                TargetObject.StorageContainer.Remove(TargetObject);
                TargetObject = null;
            }
            return new ActionResult {ForAction = this, ResultType = ActionResultType.Ok};
        }

        public override GameAction CreateNewInstance()
        {
            return new ObjectUseOnObjectAction();
        }

    }
}
