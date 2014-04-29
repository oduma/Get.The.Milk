using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Levels;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class FloorPlanViewModel:ViewModelBase
    {

        public FloorPlanViewModel(IEnumerable<CellViewModel> cells, SizeOfLevel sizeOfMap,int floorNumber)
        {
            Cells= new ObservableCollection<CellViewModel>();
            foreach(var cell in cells)
                Cells.Add(cell);
            FloorNumber = floorNumber;
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

        private ObservableCollection<CellViewModel> _cells;
        public ObservableCollection<CellViewModel> Cells
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

        public int FloorNumber { get; set; }

        public void ResetStartCell()
        {
            var prevStartCell = Cells.FirstOrDefault(c => !string.IsNullOrEmpty(c.StartCellMarking));
            if(prevStartCell!=null)
            {
                prevStartCell.StartCellMarking = "";
            }
        }

        public void ResetObjectiveCell()
        {
            var prevObjCell = Cells.FirstOrDefault(c => !string.IsNullOrEmpty(c.ObjectiveCellMarking));
            if (prevObjCell != null)
            {
                prevObjCell.ObjectiveCellMarking = "";
            }
        }
    }
}
