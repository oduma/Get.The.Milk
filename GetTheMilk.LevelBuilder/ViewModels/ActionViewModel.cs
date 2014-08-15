using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ActionViewModel:ViewModelBase
    {
        public BaseActionTemplate Value { get; set; }

    }
}
