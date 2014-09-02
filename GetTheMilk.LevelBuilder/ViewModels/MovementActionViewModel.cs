using GetTheMilk.Actions.ActionTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class MovementActionViewModel:ActionDetailViewModelBase
    {
        private int _targetCell;

        public int TargetCell
        {
            get
            {
                return _targetCell;
            }
            set
            {
                if (value != _targetCell)
                {
                    _targetCell = value;
                    RaisePropertyChanged("TargetCell");
                }
            }
        }


        public MovementActionViewModel(MovementActionTemplate value)
        {
            TargetCell = value.TargetCell;
        }

        public override void ApplyDetailsToValue(ref BaseActionTemplate inValue)
        {
            ((MovementActionTemplate)inValue).TargetCell = TargetCell;
        }
    }
}
