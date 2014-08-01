using GetTheMilk.Actions.ActionPerformers;
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
            protected set
            {
                _currentPerformer = (IObjectTransferActionTemplatePerformer)value;
                BuildPerformer(ref _currentPerformer);

            }
        }
        public override BaseActionTemplate Clone()
        {
            return new ObjectTransferActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                CurrentPerformer = CurrentPerformer,
                ActiveCharacter = ActiveCharacter
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

        internal override object[] Translate()
        {
            var result = base.Translate();
            result[2] = (TargetObject == null || TargetObject.Name==null) ? "Target Object Not Assigned" : TargetObject.Name.Narrator;

            result[6] = ((TargetObject!=null && TargetObject is ITransactionalObject)
                &&(PerformerType!=null 
                    && PerformerType==typeof(BuyActionPerformer)))
                            ? "("+ ((ITransactionalObject)TargetObject).BuyPrice.ToString() +")"
                            : string.Empty;
            result[7] = ((TargetObject != null && TargetObject is ITransactionalObject)
                && (PerformerType != null
                    && PerformerType == typeof(SellActionPerformer)))
                            ? "(" + ((ITransactionalObject)TargetObject).SellPrice.ToString() + ")"
                            : string.Empty;

            return result;
        }

    }
}
