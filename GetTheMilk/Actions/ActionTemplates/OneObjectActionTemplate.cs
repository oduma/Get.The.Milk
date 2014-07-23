using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class OneObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public OneObjectActionTemplate()
        {
            StartingAction = true;
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
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<IOneObjectActionTemplatePerformer>(value.Name);
            }
        }

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
                if(PerformerType == null || PerformerType.Name!=_currentPerformer.GetType().Name)
                    PerformerType = _currentPerformer.GetType();
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
