using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class TwoCharactersActionViewModel:ActionDetailViewModelBase
    {
        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    RaisePropertyChanged("Message");
                }
            }
        }


        public TwoCharactersActionViewModel(TwoCharactersActionTemplate value)
        {
            Message = value.Message;
        }


        public override void ApplyDetailsToValue(ref BaseActionTemplate inValue)
        {
            ((TwoCharactersActionTemplate)inValue).Message = Message;
        }
    }
}
