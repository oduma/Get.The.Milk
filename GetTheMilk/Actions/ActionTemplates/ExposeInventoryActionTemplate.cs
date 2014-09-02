using System;
using GetTheMilk.Actions.ActionPerformers.Base;
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
    }
}
