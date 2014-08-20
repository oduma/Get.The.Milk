using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class MovementActionViewModel:ViewModelBase
    {
        private MovementActionTemplate _value;

        public MovementActionTemplate Value
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
        public MovementActionViewModel(MovementActionTemplate value)
        {
            Value = value;
        }
    }
}
