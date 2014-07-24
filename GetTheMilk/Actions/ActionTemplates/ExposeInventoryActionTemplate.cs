using System;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Factories;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ExposeInventoryActionTemplate:BaseActionTemplate
    {
        public ExposeInventoryActionTemplate()
        {
            StartingAction = false;
            //the prebuilt in default performer will load
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
        [LevelBuilderAccesibleProperty(typeof(string))]
        public string FinishActionUniqueId { get; set; }

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
                           FinishActionUniqueId=FinishActionUniqueId,
                           CurrentPerformer=CurrentPerformer,
                           ActiveCharacter=ActiveCharacter
                       };
        }

        protected override object[] Translate()
        {
            var result = base.Translate();
            result[5] = (TargetObject is ITransactionalObject)
                            ? ((ITransactionalObject) TargetObject).BuyPrice.ToString()
                            : string.Empty;
            result[6] = (FinishActionUniqueId=="Attack") ? "Prepare for Battle" : "Expose Inventory";
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
