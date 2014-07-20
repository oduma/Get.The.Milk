using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class OneObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public OneObjectActionTemplate()
        {
            StartingAction = true;
            PerformerType = typeof (IOneObjectActionTemplatePerformer);
            Category = GetType().Name;
        }
        #endregion

        public override BaseActionTemplate Clone()
        {
            return new OneObjectActionTemplate { Name = Name, StartingAction = StartingAction, FinishTheInteractionOnExecution = FinishTheInteractionOnExecution };
        }
    }
}
