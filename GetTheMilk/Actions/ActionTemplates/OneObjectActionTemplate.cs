using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class OneObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public OneObjectActionTemplate()
        {
            StartingAction = true;
        }
        #endregion

        IOneObjectActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = (IOneObjectActionTemplatePerformer)value;
            }
        }
        public override bool CanPerform()
        {
            return ((IOneObjectActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IOneObjectActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

        public override BaseActionTemplate Clone()
        {
            return new OneObjectActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                CurrentPerformer = CurrentPerformer
            };
        }
    }
}
