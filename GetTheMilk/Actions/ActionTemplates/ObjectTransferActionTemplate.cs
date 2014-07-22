using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectTransferActionTemplate:BaseActionTemplate
    {
        public ObjectTransferActionTemplate()
        {
            StartingAction = false;
        }

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
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<IObjectTransferActionTemplatePerformer>(value.Name);
            }
        }


        IObjectTransferActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = (IObjectTransferActionTemplatePerformer)value;
                if (PerformerType == null || PerformerType.Name != _currentPerformer.GetType().Name)
                    PerformerType = _currentPerformer.GetType();

            }
        }
        public override BaseActionTemplate Clone()
        {
            return new ObjectTransferActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                CurrentPerformer = CurrentPerformer
            };
        }

        public override bool CanPerform()
        {
            return ((IObjectTransferActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IObjectTransferActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

        protected override object[] Translate()
        {
            var result = base.Translate();
            result[1] = (TargetObject == null) ? "Target Object Not Assigned" : TargetObject.Name.Narrator;

            result[5] = (TargetObject!=null && TargetObject is ITransactionalObject)
                            ? "("+ ((ITransactionalObject)TargetObject).BuyPrice.ToString() +")"
                            : string.Empty;
            return result;
        }

    }
}
