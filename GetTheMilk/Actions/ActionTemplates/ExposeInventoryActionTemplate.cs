using System;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ExposeInventoryActionTemplate:BaseActionTemplate
    {
        public ExposeInventoryActionTemplate()
        {
            StartingAction = false;
        }

        IExposeInventoryActionTemplatePerformer _currentPerformer;

        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = (IExposeInventoryActionTemplatePerformer)value;
            }
        }
        [LevelBuilderAccesibleProperty(typeof(string))]
        public string FinishActionType { get; set; }

        [LevelBuilderAccesibleProperty(typeof(Type))]
        public Type FinishActionCategory { get; set; }

        [LevelBuilderAccesibleProperty(typeof(bool))]
        public bool SelfInventory { get; set; }

        public override BaseActionTemplate Clone()
        {
            return new ExposeInventoryActionTemplate
                       {
                           Name = Name,
                           StartingAction = StartingAction,
                           FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                           SelfInventory = SelfInventory,
                           FinishActionCategory = FinishActionCategory,
                           FinishActionType=FinishActionType,
                           CurrentPerformer=CurrentPerformer
                       };
        }

        protected override object[] Translate()
        {
            var result = base.Translate();
            result[5] = (TargetObject is ITransactionalObject)
                            ? ((ITransactionalObject) TargetObject).BuyPrice.ToString()
                            : string.Empty;
            result[6] = (FinishActionType=="Attack") ? "Prepare for Battle" : "Expose Inventory";
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
