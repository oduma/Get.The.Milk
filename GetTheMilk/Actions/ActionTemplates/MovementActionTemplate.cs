using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Navigation;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class MovementActionTemplate : BaseActionTemplate
    {
        public MovementActionTemplate()
        {
            DefaultDistance = 1;
            PerformerType = typeof (WalkActionPerformer);
            Category = GetType().Name;
            StartingAction = true;
        }

        public int TargetCell { get; set; }

        public Direction Direction { get; set; }

        public int DefaultDistance { get; set; }

        public Map CurrentMap { get; set; }

        public override BaseActionTemplate Clone()
        {
            return new MovementActionTemplate { Name = Name, StartingAction = StartingAction, FinishTheInteractionOnExecution = FinishTheInteractionOnExecution };
        }

        protected override object[] Translate()
        {
            return new object[]
                       {
                           null,
                           (Name.Present)??(GetType().Name + "-" + PerformerType.Name),
                           null,
                           null,
                           null,
                           null,null,null
                       };
        }
    }
}
