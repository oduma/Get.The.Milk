using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class TwoCharactersActionViewModel:ViewModelBase
    {
        private TwoCharactersActionTemplate _value;

        public TwoCharactersActionTemplate Value
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

        public TwoCharactersActionViewModel(TwoCharactersActionTemplate value)
        {
            Value = value;
        }

    }
}
