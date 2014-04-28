using System;
using System.Collections.Generic;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class FloorPlanViewModel:ViewModelBase
    {
        public FloorPlanViewModel(IEnumerable<CellViewModel> cells, SizeOfLevel sizeOfMap)
        {
            Cells = cells;
            SizeOfMap = (int)sizeOfMap;
        }

        private int _sizeOfMap;
        public int SizeOfMap
        {
            get { return _sizeOfMap; }
            set
            {
                if (value != _sizeOfMap)
                {
                    _sizeOfMap = value;
                    RaisePropertyChanged("SizeOfMap");
                }
            }
        }

        private IEnumerable<CellViewModel> _cells;
        public IEnumerable<CellViewModel> Cells
        {
            get { return _cells; }
            set
            {
                if (value != _cells)
                {
                    _cells = value;
                    RaisePropertyChanged("Cells");
                }
            }
        }
    }
}
