using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectUseOnObjectActionTemplate : BaseActionTemplate
    {
        public bool DestroyActiveObject { get; set; }

        public bool DestroyTargetObject { get; set; }

        public ChanceOfSuccess ChanceOfSuccess { get; set; }

        public int PercentOfHealthFailurePenalty { get; set; }

        public ObjectUseOnObjectActionTemplate()
        {
            ChanceOfSuccess = ChanceOfSuccess.Full;
            StartingAction = true;
        }

        protected override object[] Translate()
        {
            var result = base.Translate();
            result[1] = (TargetObject == null) ? "No Target Object Assigned" : TargetObject.Name.Narrator;
            result[3] = (ActiveObject == null) ? "No Active Object Assigned" : ActiveObject.Name.Narrator;
            return result;
        }

        IObjectUseOnObjectActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = (IObjectUseOnObjectActionTemplatePerformer)value;
            }
        }
        public override BaseActionTemplate Clone()
        {
            return new ObjectUseOnObjectActionTemplate
                       {
                           Name = Name,
                           StartingAction = StartingAction,
                           FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                           DestroyActiveObject = DestroyActiveObject,
                           DestroyTargetObject = DestroyTargetObject,
                           ChanceOfSuccess = ChanceOfSuccess,
                           PercentOfHealthFailurePenalty = PercentOfHealthFailurePenalty,
                           CurrentPerformer = CurrentPerformer
                       };
        }

        public override bool CanPerform()
        {
            return ((IObjectUseOnObjectActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IObjectUseOnObjectActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

    }
}
