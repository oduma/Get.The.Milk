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
    public class ObjectUseOnObjectActionViewModel:ActionDetailViewModelBase
    {
        private bool _destroyActiveObject;

        public bool DestroyActiveObject
        {
            get
            {
                return _destroyActiveObject;
            }
            set
            {
                if (value != _destroyActiveObject)
                {
                    _destroyActiveObject = value;
                    RaisePropertyChanged("DestroyActiveObject");
                }
            }
        }

        private bool _destroyTargetObject;

        public bool DestroyTargetObject
        {
            get
            {
                return _destroyTargetObject;
            }
            set
            {
                if (value != _destroyTargetObject)
                {
                    _destroyTargetObject = value;
                    RaisePropertyChanged("DestroyTargetObject");
                }
            }
        }

        private ChanceOfSuccess _chanceOfSuccess;

        public ChanceOfSuccess ChanceOfSuccess
        {
            get
            {
                return _chanceOfSuccess;
            }
            set
            {
                if (value != _chanceOfSuccess)
                {
                    _chanceOfSuccess = value;
                    RaisePropertyChanged("ChanceOfSuccess");
                }
            }
        }

        private int _percentageOfHealthFailure;

        public int PercentOfHealthFailure
        {
            get
            {
                return _percentageOfHealthFailure;
            }
            set
            {
                if (value != _percentageOfHealthFailure)
                {
                    _percentageOfHealthFailure = value;
                    RaisePropertyChanged("PercentOfHealthFailure");
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
            DestroyActiveObject = value.DestroyActiveObject;
            DestroyTargetObject = value.DestroyTargetObject;
            ChanceOfSuccess = value.ChanceOfSuccess;
            PercentOfHealthFailure = value.PercentOfHealthFailurePenalty;

        }


        public override void ApplyDetailsToValue(ref BaseActionTemplate inValue)
        {
            ((ObjectUseOnObjectActionTemplate)inValue).ChanceOfSuccess = ChanceOfSuccess;
            ((ObjectUseOnObjectActionTemplate)inValue).DestroyActiveObject = DestroyActiveObject;
            ((ObjectUseOnObjectActionTemplate)inValue).DestroyTargetObject = DestroyTargetObject;
            ((ObjectUseOnObjectActionTemplate)inValue).PercentOfHealthFailurePenalty = PercentOfHealthFailure;
        }
    }
}
