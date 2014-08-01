using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Factories;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectUseOnObjectActionTemplate : BaseActionTemplate
    {
        public bool DestroyActiveObject { get; set; }

        public bool DestroyTargetObject { get; set; }

        public ChanceOfSuccess ChanceOfSuccess { get; set; }

        public int PercentOfHealthFailurePenalty { get; set; }

        public ObjectUseOnObjectActionTemplate()
        {
            ChanceOfSuccess = ChanceOfSuccess.Full;
            StartingAction = true;
            Category = GetType().Name;

        }

        internal override object[] Translate()
        {
            var result = base.Translate();
            result[2] = (TargetObject == null || TargetObject.Name==null) ? "No Target Object Assigned" : TargetObject.Name.Narrator;
            result[4] = (ActiveObject == null || ActiveObject.Name==null) ? "No Active Object Assigned" : ActiveObject.Name.Narrator;
            return result;
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
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<IObjectUseOnObjectActionTemplatePerformer>(value.Name);
            }
        }


        IObjectUseOnObjectActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            protected set
            {
                _currentPerformer = (IObjectUseOnObjectActionTemplatePerformer)value;
                BuildPerformer(ref _currentPerformer);
            }
        }
        public override BaseActionTemplate Clone()
        {
            return new ObjectUseOnObjectActionTemplate
                       {
                           Name = Name,
                           StartingAction = StartingAction,
                           FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                           DestroyActiveObject = DestroyActiveObject,
                           DestroyTargetObject = DestroyTargetObject,
                           ChanceOfSuccess = ChanceOfSuccess,
                           PercentOfHealthFailurePenalty = PercentOfHealthFailurePenalty,
                           CurrentPerformer = CurrentPerformer,
                           ActiveCharacter = ActiveCharacter,
                           ActiveObject = ActiveObject
                       };
        }

        public override bool CanPerform()
        {
            return ((IObjectUseOnObjectActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IObjectUseOnObjectActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

    }
}
