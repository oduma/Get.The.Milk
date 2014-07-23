using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class NoObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public NoObjectActionTemplate()
        {
            StartingAction = false;
            Category = GetType().Name;

        }
        #endregion

        private Type _performerType;
        public Type PerformerType
        {
            get
            {
                return _performerType;
            }
            set
            {
                _performerType = value;
                if(_performerType!=null)
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<INoObjectActionTemplatePerformer>(value.Name);
            }
        }

        private INoObjectActionTemplatePerformer _currentPerformer;

        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = (INoObjectActionTemplatePerformer)value;
                if (PerformerType == null || PerformerType.Name != _currentPerformer.GetType().Name)
                    PerformerType = _currentPerformer.GetType();

            }
        }
        public override bool CanPerform()
        {
            return ((INoObjectActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((INoObjectActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

        public override BaseActionTemplate Clone()
        {
            return new NoObjectActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                CurrentPerformer = CurrentPerformer
            };
        }
    }
}
