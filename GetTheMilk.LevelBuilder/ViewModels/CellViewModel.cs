using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class CellViewModel:ViewModelBase
    {
        public RelayCommand<CellViewModel> MarkAsObjective { get; set; }
        public RelayCommand<CellViewModel> MarkAsStart { get; set; }


        private string _startCellMarking;
        public string StartCellMarking
        {
            get { return _startCellMarking; }
            set
            {
                if (value != _startCellMarking)
                {
                    _startCellMarking = value;
                    RaisePropertyChanged("StartCellMarking");
                }
            }
        }

        private int _rowIndex;
        public int RowIndex
        {
            get { return _rowIndex; }
            set
            {
                if (value != _rowIndex)
                {
                    _rowIndex = value;
                    RaisePropertyChanged("RowIndex");
                }
            }
        }

        private int _columnIndex;
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set
            {
                if (value != _columnIndex)
                {
                    _columnIndex = value;
                    RaisePropertyChanged("ColumnIndex");
                }
            }
        }

        private Cell _value;
        public Cell Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        private string _objectiveCellMarking;
        public string ObjectiveCellMarking
        {
            get { return _objectiveCellMarking; }
            set
            {
                if (value != _objectiveCellMarking)
                {
                    _objectiveCellMarking = value;
                    RaisePropertyChanged("ObjectiveCellMarking");
                }
            }
        }
    }
}
