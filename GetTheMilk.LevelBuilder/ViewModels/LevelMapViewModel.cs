using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class LevelMapViewModel:ViewModelBase
    {
        public RelayCommand AddNewFloor { get; private set; }

        private readonly Level _level;

        public LevelMapViewModel(Level level)
        {
            _level = level;
            SizeOfMap = level.SizeOfLevel;
            if (_level.CurrentMap == null)
                _level.CurrentMap=GenerateNewEmptyMap();
            var floors = _level.CurrentMap.Cells.Select(c => c.Floor).Distinct().ToList();
            Floors=new ObservableCollection<int>();
            foreach(var floor in floors)
            {
                Floors.Add(floor);
            }
            SelectedFloor = floors.Min();
            AddNewFloor=new RelayCommand(AddNewFloorCommand);

            FloorPlanViewModel =
                new FloorPlanViewModel(
                    _level.CurrentMap.Cells.Where(c => c.Floor == SelectedFloor).Select(
                        c =>
                        new CellViewModel
                            {Value = c, RowIndex = c.Number/(int) SizeOfMap, ColumnIndex = c.Number%(int) SizeOfMap}),
                    SizeOfMap);
        }

        private void AddNewFloorCommand()
        {
            var newFloor = Floors.Max() + 1;
            Floors.Add(newFloor);
            var allExistingCells = _level.CurrentMap.Cells.ToList();
            AddAFloor(allExistingCells,newFloor);
            _level.CurrentMap.Cells = allExistingCells.ToArray();
        }

        public ObservableCollection<int> Floors
        {
            get { return _floors; }
            set
            {
                if (value != _floors)
                {
                    _floors = value;
                    RaisePropertyChanged("Floors");
                }
            }
        }

        private Map GenerateNewEmptyMap()
        {
            Map newMap = new Map {Parent = _level};

            var allCells = new List<Cell>();
            AddAFloor(allCells,0);
            newMap.Cells = allCells.ToArray();
            return newMap;
        }

        private void AddAFloor(List<Cell> allCells,int floorNumber)
        {
            int sizeOfMap = (int) SizeOfMap;
            for (int i = 0; i < sizeOfMap*sizeOfMap; i++)
            {
                allCells.Add(new Cell
                                 {
                                     BottomCell = -1,
                                     Floor = floorNumber,
                                     Number = (floorNumber*sizeOfMap*sizeOfMap)+i,
                                     NorthCell = (i < sizeOfMap) ? -1 : ((floorNumber * sizeOfMap * sizeOfMap) + i - sizeOfMap),
                                     WestCell = (i % sizeOfMap == 0) ? -1 : (floorNumber * sizeOfMap * sizeOfMap) + i - 1,
                                     TopCell = -1,
                                     EastCell = ((i + 1) % sizeOfMap == 0) ? -1 : (floorNumber * sizeOfMap * sizeOfMap) + i + 1,
                                     SouthCell = (i < sizeOfMap * (sizeOfMap - 1)) ? ((floorNumber * sizeOfMap * sizeOfMap) + i + sizeOfMap) : -1
                                 });
            }
        }

        private SizeOfLevel _sizeOfMap;

        public SizeOfLevel SizeOfMap
        {
            get { return _sizeOfMap; }
            set
            {
                if (_sizeOfMap != value)
                {
                    _sizeOfMap = value;
                    RaisePropertyChanged("SizeOfMap");
                }
            }
        }

        private FloorPlanViewModel _floorPlanViewModel;

        public FloorPlanViewModel FloorPlanViewModel
        {
            get { return _floorPlanViewModel; }
            set
            {
                if (_floorPlanViewModel != value)
                {
                    _floorPlanViewModel = value;
                    RaisePropertyChanged("FloorPlanViewModel");
                }
            }
        }

        private int _selectedFloor;
        private ObservableCollection<int> _floors;

        public int SelectedFloor
        {
            get { return _selectedFloor; }
            set
            {
                if (_selectedFloor != value)
                {
                    _selectedFloor = value;
                    RaisePropertyChanged("SelectedFloor");
                }
            }
        }

    }
}
