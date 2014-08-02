using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectResponseActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public ObjectResponseActionTemplate()
        {
            StartingAction = false;
            Category = GetType().Name;
        }
        #endregion


        public override bool CanPerform()
        {
            return ((IObjectResponseActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IObjectResponseActionTemplatePerformer)CurrentPerformer).Perform(this);
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
                if (_performerType != null)
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<IObjectResponseActionTemplatePerformer>(value.Name);
            }
        }


        private IObjectResponseActionTemplatePerformer _currentPerformer;

        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            protected set
            {
                _currentPerformer = (IObjectResponseActionTemplatePerformer)value;
                BuildPerformer(ref _currentPerformer);

            }
        }

        public override BaseActionTemplate Clone()
        {
            return new ObjectResponseActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                CurrentPerformer = CurrentPerformer,
                ActiveObject=ActiveObject
            };
        }


    }
}
