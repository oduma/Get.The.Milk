using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class CellViewModel:ViewModelBase
    {
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


    }
}
