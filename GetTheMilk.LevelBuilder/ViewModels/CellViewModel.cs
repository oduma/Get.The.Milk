using System.Collections.ObjectModel;
using System.Windows.Media;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
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

        public RelayCommand<Character> PlaceACharacter { get; set; }

        public CellViewModel()
        {
            PlaceAnObject=new RelayCommand<NonCharacterObject>(PlaceAnObjectCommand);
            PlaceACharacter=new RelayCommand<Character>(PlaceACharacterCommand);
        }

        private void PlaceACharacterCommand(Character obj)
        {
            obj.CellNumber = Value.Number;
            if (Value.Parent.Parent.Characters == null)
                Value.Parent.Parent.Characters = new CharacterCollection();
            MarkTheOccupancy(null,obj);
            Value.Parent.Parent.Characters.Add(obj);
            AllCharactersAvailable.Remove(obj);
        }

        private void PlaceAnObjectCommand(NonCharacterObject obj)
        {
            obj.CellNumber = Value.Number;
            if(Value.Parent.Parent.Inventory==null)
                Value.Parent.Parent.Inventory= new Inventory();
            MarkTheOccupancy(obj,null);
            Value.Parent.Parent.Inventory.Add(obj);
            AllObjectsAvailable.Remove(obj);
        }

        private void MarkTheOccupancy(NonCharacterObject nonCharacterObject,Character character)
        {
            OcupancyMarker = GetColorForCell(nonCharacterObject,character);

            OccupantName = GetOccupantName(nonCharacterObject, character);
        }

        public static string GetOccupantName(NonCharacterObject nonCharacterObject, Character character)
        {
            if (nonCharacterObject != null)
            {
                return nonCharacterObject.Name.Main;
            }
            if (character != null)
                return character.Name.Main;
            return string.Empty;
        }

        public static Brush GetColorForCell(NonCharacterObject nonCharacterObject,Character character)
        {
            object color=null;
            if (nonCharacterObject != null)
            {
                switch (nonCharacterObject.ObjectCategory)
                {
                    case ObjectCategory.Decor:
                        color = ColorConverter.ConvertFromString("Maroon");
                        break;
                    case ObjectCategory.Tool:
                        color = ColorConverter.ConvertFromString("Yellow");
                        break;
                    case ObjectCategory.Weapon:
                        color = ColorConverter.ConvertFromString("Pink");
                        break;
                    default:
                        color = ColorConverter.ConvertFromString("Green");
                        break;
                }
            }
            if(character!=null)
            {
                switch (character.ObjectTypeId)
                {
                    case "NPCFoe":
                        color = ColorConverter.ConvertFromString("Red");
                        break;
                    case "NPCFriendly":
                        color = ColorConverter.ConvertFromString("Cadetblue");
                        break;
                }
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
                if (!Equals(value, _ocupancyMarker))
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

        public ObservableCollection<Character> AllCharactersAvailable { get; set; }
    }
}
