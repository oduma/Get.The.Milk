using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class WeaponObjectViewModel:ObjectViewModelBase
    {
        public override NonCharacterObject Value { get; set; }
    }
}
