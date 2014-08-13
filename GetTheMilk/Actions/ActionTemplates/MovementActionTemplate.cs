using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
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


        private Type _performerType;
        public override Type PerformerType
        {
            get
            {
                return _performerType;
            }
            set
            {
                _performerType = value;
                if(_performerType!=null)
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<IMovementActionTemplatePerformer>(value.Name);
            }
        }



        private IMovementActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            protected set
            {
                _currentPerformer = (IMovementActionTemplatePerformer)value;
                BuildPerformer(ref _currentPerformer);

            }
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
        public override bool CanPerform()
        {
            return ((IMovementActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IMovementActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

    }
}
