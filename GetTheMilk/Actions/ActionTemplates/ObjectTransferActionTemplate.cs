using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectTransferActionTemplate:BaseActionTemplate
    {
        public ObjectTransferActionTemplate()
        {
            StartingAction = false;
        }

        IObjectTransferActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = (IObjectTransferActionTemplatePerformer)value;
            }
        }
        public override BaseActionTemplate Clone()
        {
            return new ObjectTransferActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                CurrentPerformer = CurrentPerformer
            };
        }

        public override bool CanPerform()
        {
            return ((IObjectTransferActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IObjectTransferActionTemplatePerformer)CurrentPerformer).Perform(this);
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
