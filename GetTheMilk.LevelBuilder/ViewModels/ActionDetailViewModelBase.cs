using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public abstract class ActionDetailViewModelBase:ViewModelBase
    {
        public abstract void ApplyDetailsToValue(ref BaseActionTemplate inValue);
    }
}
