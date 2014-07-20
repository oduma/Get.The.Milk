using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectTransferActionTemplate:BaseActionTemplate
    {
        public ObjectTransferActionTemplate()
        {
            StartingAction = false;
            Category = GetType().Name;
        }

        public override BaseActionTemplate Clone()
        {
            return new ObjectTransferActionTemplate { Name = Name, StartingAction = StartingAction, FinishTheInteractionOnExecution = FinishTheInteractionOnExecution };
        }

        protected override object[] Translate()
        {
            var result = base.Translate();
            result[1] = (TargetObject == null) ? "Target Object Not Assigned" : TargetObject.Name.Narrator;

            result[5] = (TargetObject!=null && TargetObject is ITransactionalObject)
                            ? "("+ ((ITransactionalObject)TargetObject).BuyPrice.ToString() +")"
                            : string.Empty;
            return result;
        }

    }
}
