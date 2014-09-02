using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.BaseCommon;
using GetTheMilk.Navigation;

namespace GetTheMilk.Actions.ActionTemplates
{
    public sealed class MovementActionTemplate : BaseActionTemplate
    {
        public MovementActionTemplate()
        {
            DefaultDistance = 1;
            StartingAction = true;
            PerformerType = typeof(WalkActionPerformer);
            Name = new Verb { UniqueId = "Walk", Present = "walk", Past = "walked" };
            Category = GetType().Name;

        }

        public int TargetCell { get; set; }

        public Direction Direction { get; set; }

        public int DefaultDistance { get; set; }

        public Map CurrentMap { get; set; }

        public override BaseActionTemplate Clone()
        {
            return new MovementActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                CurrentPerformer = CurrentPerformer,
                ActiveCharacter = ActiveCharacter,
                DefaultDistance=DefaultDistance
            };
        }

        internal override object[] Translate()
        {
            var result = base.Translate();
            if (PerformerType != typeof(TeleportActionPerformer))
                result[10] = " " + Direction.ToString();
            return result;
        }
    }
}
