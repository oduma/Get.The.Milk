using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ExposeInventoryActionViewModel:ActionDetailViewModelBase
    {
        public IEnumerable<ExposeInventoryFinishingAction> AllFinishingActions{get;set;}

        private bool _selfInventory;

        public bool SelfInventory
        {
            get
            {
                return _selfInventory;
            }
            set
            {
                if (value != _selfInventory)
                {
                    _selfInventory = value;
                    RaisePropertyChanged("SelfInventory");
                }
            }
        }

        private ExposeInventoryFinishingAction _finishingAction;

        public ExposeInventoryFinishingAction FinishingAction
        {
            get
            {
                return _finishingAction;
            }
            set
            {
                if (value != _finishingAction)
                {
                    _finishingAction = value;
                    RaisePropertyChanged("FinishingAction");
                }
            }
        }


        public ExposeInventoryActionViewModel(ExposeInventoryActionTemplate value)
        {
            AllFinishingActions = new ExposeInventoryFinishingAction[] { ExposeInventoryFinishingAction.Attack, ExposeInventoryFinishingAction.CloseInventory };
            FinishingAction = value.FinishingAction;
            SelfInventory = value.SelfInventory;
        }

        public override void ApplyDetailsToValue(ref BaseActionTemplate inValue)
        {
            ((ExposeInventoryActionTemplate)inValue).SelfInventory = SelfInventory;
            ((ExposeInventoryActionTemplate)inValue).FinishingAction = FinishingAction;
        }
    }
}
