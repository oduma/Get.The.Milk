using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class NoObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public NoObjectActionTemplate()
        {
            StartingAction = false;
            PerformerType = typeof(INoObjectActionTemplatePerformer);
            Category = GetType().Name;
        }
        #endregion

        public override BaseActionTemplate Clone()
        {
            return new NoObjectActionTemplate { Name = Name, StartingAction = StartingAction, FinishTheInteractionOnExecution = FinishTheInteractionOnExecution };
        }
    }
}
