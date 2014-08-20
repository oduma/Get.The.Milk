using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ExposeInventoryActionViewModel:ViewModelBase
    {
        private ExposeInventoryActionTemplate _value;

        public ExposeInventoryActionTemplate Value
        {
            get { return _value; }
            set
            {
                if(value!=_value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public IEnumerable<ExposeInventoryFinishingAction> AllFinishingActions;
        public ExposeInventoryActionViewModel(ExposeInventoryActionTemplate value)
        {
            AllFinishingActions = new ExposeInventoryFinishingAction[] { ExposeInventoryFinishingAction.Attack, ExposeInventoryFinishingAction.CloseInventory };
            Value = value;
        }
    }
}
