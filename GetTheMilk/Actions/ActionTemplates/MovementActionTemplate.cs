using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class MovementActionTemplate : BaseActionTemplate
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
