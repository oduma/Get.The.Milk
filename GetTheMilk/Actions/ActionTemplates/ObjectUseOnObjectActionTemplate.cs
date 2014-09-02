using GetTheMilk.Actions.ActionPerformers.Base;
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
        public override BaseActionTemplate Clone()
        {
            return new ObjectUseOnObjectActionTemplate
                       {
                           Name = Name,
                           StartingAction = StartingAction,
                           DestroyActiveObject = DestroyActiveObject,
                           DestroyTargetObject = DestroyTargetObject,
                           ChanceOfSuccess = ChanceOfSuccess,
                           PercentOfHealthFailurePenalty = PercentOfHealthFailurePenalty,
                           CurrentPerformer = CurrentPerformer,
                           ActiveCharacter = ActiveCharacter,
                           ActiveObject = ActiveObject,
                           TargetObject=TargetObject
                       };
        }

    }
}
