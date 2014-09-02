using GetTheMilk.Actions.ActionPerformers;
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
            return new ObjectTransferActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                CurrentPerformer = CurrentPerformer,
                ActiveCharacter = ActiveCharacter
            };
        }
        internal override object[] Translate()
        {
            var result = base.Translate();
            result[2] = (TargetObject == null || TargetObject.Name==null) ? "Target Object Not Assigned" : TargetObject.Name.Narrator;

            result[6] = ((TargetObject!=null && TargetObject is ITransactionalObject)
                &&(PerformerType!=null 
                    && PerformerType==typeof(BuyActionPerformer)))
                            ? "("+ ((ITransactionalObject)TargetObject).BuyPrice +")"
                            : string.Empty;
            result[7] = ((TargetObject != null && TargetObject is ITransactionalObject)
                && (PerformerType != null
                    && PerformerType == typeof(SellActionPerformer)))
                            ? "(" + ((ITransactionalObject)TargetObject).SellPrice + ")"
                            : string.Empty;

            return result;
        }

    }
}
