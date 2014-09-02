using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Get.The.Milk.UI.BaseViewModels;
using GetTheMilk.Characters;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.Base;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class CellViewModel:ViewModelBase
    {
        public RelayCommand<CellViewModel> MarkAsObjective { get; set; }
        public RelayCommand<CellViewModel> MarkAsStart { get; set; }

        public RelayCommand<CellViewModel> LinkToUpperFloor { get; set; }
        public RelayCommand<CellViewModel> LinkToLowerFloor { get; set; }

        public RelayCommand<CellViewModel> ClearUp { get; set; }
        public RelayCommand<CellViewModel> ClearDown { get; set; }

        public RelayCommand<NonCharacterObject> PlaceAnObject { get; set; }

        public RelayCommand<Character> PlaceACharacter { get; set; }

        public RelayCommand UnPlaceAnObject { get; private set; }

        public RelayCommand UnPlaceACharacter { get; private set; }

        public CellViewModel()
        {
            PlaceAnObject=new RelayCommand<NonCharacterObject>(PlaceAnObjectCommand);
            PlaceACharacter=new RelayCommand<Character>(PlaceACharacterCommand);
            UnPlaceAnObject= new RelayCommand(UnPlaceAnObjectCommand,CanUnPlaceAnObject);
            UnPlaceACharacter=new RelayCommand(UnPlaceACharacterCommand,CanUnPlaceACharacter);
        }

        private bool CanUnPlaceACharacter()
        {
            return Value.AllCharacters.Any();
        }

        private void UnPlaceACharacterCommand()
        {
            AllCharactersAvailable.Add(Value.AllCharacters.First());
            Value.Parent.Parent.Characters.Remove(Value.AllCharacters.First());
            MarkTheOccupancy(Value.AllObjects.FirstOrDefault(), Value.AllCharacters.FirstOrDefault());
        }

        private bool CanUnPlaceAnObject()
        {
            return Value.AllObjects.Any();
        }

        private void UnPlaceAnObjectCommand()
        {
            AllObjectsAvailable.Add(Value.AllObjects.First());
            Value.Parent.Parent.Inventory.Remove(Value.AllObjects.First());
            MarkTheOccupancy(Value.AllObjects.FirstOrDefault(),Value.AllCharacters.FirstOrDefault());
            
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
                Value.Parent.Parent.Inventory= new Inventory{InventoryType=InventoryType.LevelInventory};
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

        private string _linkToUpperText;
        private string _linkToLowerText;
        private string _linkToUpperMarking;
        private string _linkToUpperCell;
        private string _linkToLowerMarking;
        private string _linkToLowerCell;

        public string LinkToUpperText
        {
            get { return _linkToUpperText; }
            set
            {
                if (value != _linkToUpperText)
                {
                    _linkToUpperText = value;
                    RaisePropertyChanged("LinkToUpperText");
                }
            }
        }

        public ObservableCollection<NonCharacterObject> AllObjectsAvailable { get; set; }

        public ObservableCollection<Character> AllCharactersAvailable { get; set; }

        public string LinkToLowerText
        {
            get { return _linkToLowerText; }
            set
            {
                if(value!=_linkToLowerText)
                {
                    _linkToLowerText = value;
                    RaisePropertyChanged("LinkToLowerText");
                }
            }
        }

        public string LinkToUpperMarking
        {
            get { return _linkToUpperMarking; }
            set
            {
                if(value!=_linkToUpperMarking)
                {
                    _linkToUpperMarking = value;
                    RaisePropertyChanged("LinkToUpperMarking");
                }
            }
        }

        public string LinkToUpperCell
        {
            get { return _linkToUpperCell; }
            set { if(value!=_linkToUpperCell)
            {
                _linkToUpperCell = value;
                RaisePropertyChanged("LinkToUpperCell");
            } }
        }

        public string LinkToLowerMarking
        {
            get { return _linkToLowerMarking; }
            set
            {
                if(value!=_linkToLowerMarking)
                {
                    _linkToLowerMarking = value;
                    RaisePropertyChanged("LinkToLowerMarking");
                }
            }
        }

        public string LinkToLowerCell
        {
            get { return _linkToLowerCell; }
            set
            {
                if(value!=_linkToLowerCell)
                {
                    _linkToLowerCell = value;
                    RaisePropertyChanged("LinkToLowerCell");
                }
            }
        }

        public void MarkFloorCrossings()
        {
            LinkToUpperMarking = (Value.TopCell == -1) ? "" : "^";
            LinkToUpperCell = ((Value.TopCell == -1) ? "" : Value.TopCell.ToString());
            LinkToLowerMarking = (Value.BottomCell == -1) ? "" : "V";
            LinkToLowerCell = ((Value.BottomCell == -1) ? "" : Value.BottomCell.ToString());
        }
    }
}
