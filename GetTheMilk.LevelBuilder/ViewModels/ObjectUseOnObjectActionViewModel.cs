using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ObjectUseOnObjectActionViewModel:ViewModelBase
    {
        private ObjectUseOnObjectActionTemplate _value;

        public ObjectUseOnObjectActionTemplate Value
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

        IEnumerable<ChanceOfSuccess> AllChancesOfSuccess;
        public ObjectUseOnObjectActionViewModel(ObjectUseOnObjectActionTemplate value)
        {
            AllChancesOfSuccess = new ChanceOfSuccess[] { ChanceOfSuccess.Full, 
                ChanceOfSuccess.VeryBig, 
                ChanceOfSuccess.Big, 
                ChanceOfSuccess.Half, 
                ChanceOfSuccess.Small, 
                ChanceOfSuccess.VerySmall, 
                ChanceOfSuccess.None };
            Value = value;

        }

    }
}
