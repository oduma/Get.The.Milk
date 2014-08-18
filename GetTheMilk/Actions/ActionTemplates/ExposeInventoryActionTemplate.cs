using System;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Factories;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions.ActionTemplates
{
    public enum ExposeInventoryFinishingAction
    {
        CloseInventory,
        Attack
    }

    public class ExposeInventoryActionTemplate:BaseActionTemplate
    {
        public ExposeInventoryActionTemplate()
        {
            StartingAction = false;
            PerformerType = typeof(ExposeInventoryActionTemplatePerformer);
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
                if (_performerType != null)
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<IExposeInventoryActionTemplatePerformer>(value.Name);
            }
        }

        IExposeInventoryActionTemplatePerformer _currentPerformer;

        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            protected set
            {
                _currentPerformer = (IExposeInventoryActionTemplatePerformer)value;
                BuildPerformer<IExposeInventoryActionTemplatePerformer>(ref _currentPerformer);

            }
        }

        public ExposeInventoryFinishingAction FinishingAction { get; set; }

        public bool SelfInventory { get; set; }

        public override BaseActionTemplate Clone()
        {
            return new ExposeInventoryActionTemplate
                       {
                           Name = Name,
                           StartingAction = StartingAction,
                           SelfInventory = SelfInventory,
                           FinishingAction=FinishingAction,
                           CurrentPerformer=CurrentPerformer,
                           ActiveCharacter=ActiveCharacter
                       };
        }

        internal override object[] Translate()
        {
            var result = base.Translate();
            result[0] = (FinishingAction!=null && FinishingAction.ToString() == "Attack") ? "prepared for Battle" : "exposed Inventory";
            result[1] = (FinishingAction!=null && FinishingAction.ToString()=="Attack") ? "prepare for Battle" : "expose Inventory";
            if(!SelfInventory)
            {
                result[3] = (TargetCharacter == null || TargetCharacter.Name == null) ? " to No Target Character Assigned" : " to " + TargetCharacter.Name.Narrator;
            }
            return result;
        }

        public override bool CanPerform()
        {
            return ((IExposeInventoryActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((IExposeInventoryActionTemplatePerformer)CurrentPerformer).Perform(this);
        }
    }
}
