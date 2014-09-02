using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.GameLevels;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class FloorPlanViewModel:ViewModelBase
    {

        public FloorPlanViewModel(IEnumerable<CellViewModel> cells, 
            SizeOfLevel sizeOfMap, 
            int floorNumber)
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
        private CellViewModel _linkToUpperRequest;
        private CellViewModel _linkToLowerRequest;

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

        public CellViewModel LinkToUpperRequest
        {
            get { return _linkToUpperRequest; }
            set
            {
                if(value!=_linkToUpperRequest)
                {
                    _linkToUpperRequest = value;
                    if(value==null)
                        foreach (var cellViewModel in Cells)
                        {
                            cellViewModel.LinkToUpperText = "Link to upper floor";
                        }
                    else
                        foreach (var cellViewModel in Cells)
                        {
                            cellViewModel.LinkToUpperText = "Link to upper floor cell(" + value.Value.Number + ")";
                        }
                    RaisePropertyChanged("LinkToUpperRequest");
                }
            }
        }

        public CellViewModel LinkToLowerRequest
        {
            get { return _linkToLowerRequest; }
            set
            {
                if(value!=_linkToLowerRequest)
                {
                    _linkToLowerRequest = value;
                    if(value==null)
                        foreach(var cellViewModel in Cells)
                        {
                            cellViewModel.LinkToLowerText = "Link to lower floor";
                        }
                    else
                        foreach (var cellViewModel in Cells)
                        {
                            cellViewModel.LinkToLowerText = "Link to lower floor cell(" + value.Value.Number + ")";
                        }
                    RaisePropertyChanged("LinkToLowerRequest");
                }
            }
        }

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
