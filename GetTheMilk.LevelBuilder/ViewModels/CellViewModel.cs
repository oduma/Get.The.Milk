using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using GetTheMilk.BaseCommon;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class CellViewModel:ViewModelBase
    {
        public RelayCommand<CellViewModel> MarkAsObjective { get; set; }
        public RelayCommand<CellViewModel> MarkAsStart { get; set; }

        public RelayCommand<NonCharacterObject> PlaceAnObject { get; set; }

        public CellViewModel()
        {
            PlaceAnObject=new RelayCommand<NonCharacterObject>(PlaceAnObjectCommand);
        }

        private void PlaceAnObjectCommand(NonCharacterObject obj)
        {
            obj.CellNumber = Value.Number;
            if(Value.Parent.Parent.Inventory==null)
                Value.Parent.Parent.Inventory= new Inventory();
            MarkTheOccupancy(obj);
            Value.Parent.Parent.Inventory.Add(obj);
            AllObjectsAvailable.Remove(obj);
        }

        private void MarkTheOccupancy(NonCharacterObject nonCharacterObject)
        {
            OcupancyMarker = GetColorForObject(nonCharacterObject);

            OccupantName = nonCharacterObject.Name.Main;
        }

        public static Brush GetColorForObject(NonCharacterObject nonCharacterObject)
        {
            object color;
            switch(nonCharacterObject.ObjectCategory)
            {
                case ObjectCategory.Decor:
                    color = ColorConverter.ConvertFromString("Maroon");
                    break;
                case ObjectCategory.Tool:
                    color = ColorConverter.ConvertFromString("Yellow");
                    break;
                default:
                    color = ColorConverter.ConvertFromString("Green");
                    break;
            }
            if(color!=null)
            {
                var convColor = (Color) color;
                return new SolidColorBrush(convColor);
            }
            else
            {
                var convColor = (Color) ColorConverter.ConvertFromString("Green");
                return new SolidColorBrush(convColor);
            }
        }

        private Brush _ocupancyMarker;
        public Brush OcupancyMarker
        {
            get { return _ocupancyMarker; }
            set
            {
                if (value != _ocupancyMarker)
                {
                    _ocupancyMarker = value;
                    RaisePropertyChanged("OcupancyMarker");
                }
            }
        }

        private string _occupantName;
        public string OccupantName
        {
            get { return _occupantName; }
            set
            {
                if (value != _occupantName)
                {
                    _occupantName = value;
                    RaisePropertyChanged("OccupantName");
                }
            }
        }

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

        public ObservableCollection<NonCharacterObject> AllObjectsAvailable { get; set; }

    }
}
