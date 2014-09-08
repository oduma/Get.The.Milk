﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Get.The.Milk.UI.BaseViewModels;
using GetTheMilk.Characters.Base;
using GetTheMilk.GameLevels;
using GetTheMilk.LevelBuilder.Utils;
using GetTheMilk.Objects.Base;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class LevelMapViewModel:ViewModelBase
    {
        public RelayCommand AddNewFloor { get; private set; }

        private readonly Level _level;
        private readonly ObservableCollection<NonCharacterObject> _allObjectsAvailable;
        private readonly ObservableCollection<Character> _allCharactersAvailable;

        public LevelMapViewModel(Level level, 
            ObservableCollection<NonCharacterObject> allObjectsAvailable, 
            ObservableCollection<Character> allCharactersAvailable)
        {
            _level = level;
            _allObjectsAvailable = allObjectsAvailable;
            _allCharactersAvailable = allCharactersAvailable;
            if(_tilesets==null)
                _tilesets= new ObservableCollection<string>();
            var tilesets = Directory.GetFiles(@"ImageLibrary", "*.png").Select(f => Path.GetFileName(f).Replace(".png",""));
            foreach(var tileset in tilesets)
                _tilesets.Add(tileset);
            SizeOfMap = level.SizeOfLevel;
            if (_level.CurrentMap == null)
                _level.CurrentMap=GenerateNewEmptyMap();
            SelectedTileset = _tilesets[0];
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
                            {
                                Value = c, 
                                RowIndex = c.Number/(int) SizeOfMap, 
                                ColumnIndex = c.Number%(int) SizeOfMap,
                                MarkAsObjective = new RelayCommand<CellViewModel>(MarkAsObjectiveCommand),
                                MarkAsStart = new RelayCommand<CellViewModel>(MarkAsStartCommand),
                                LinkToUpperFloor=new RelayCommand<CellViewModel>(LinkToUpperFloorCommand,CanLinkToUpperFloor),
                                LinkToLowerFloor = new RelayCommand<CellViewModel>(LinkToLowerFloorCommand, CanLinkToLowerFloor),
                                ClearUp = new RelayCommand<CellViewModel>(ClearUpCommand, CanClearUp),
                                ClearDown = new RelayCommand<CellViewModel>(ClearDownCommand, CanClearDown),
                                StartCellMarking=(_level.StartingCell==c.Number)?">>":"",
                                ObjectiveCellMarking = (_level.ObjectiveCell == c.Number) ? "x" : "",
                                AllObjectsAvailable=_allObjectsAvailable,
                                AllCharactersAvailable =_allCharactersAvailable,
                                OcupancyMarker = CellViewModel.GetColorForCell(c.AllObjects.FirstOrDefault(), c.AllCharacters.FirstOrDefault(),c,_allAvailableTiles),
                                OccupantName = CellViewModel.GetOccupantName(c.AllObjects.FirstOrDefault(), c.AllCharacters.FirstOrDefault()),
                                LinkToUpperText = "Link to Upper Floor",
                                LinkToLowerText = "Link to Lower Floor",
                                LinkToUpperMarking=(c.TopCell==-1)?"":"^",
                                LinkToUpperCell=((c.TopCell==-1)?"":c.TopCell.ToString()),
                                LinkToLowerMarking=(c.BottomCell==-1)?"":"V",
                                LinkToLowerCell = ((c.BottomCell == -1) ? "" : c.BottomCell.ToString()),
                                AllAvailableTiles = _allAvailableTiles
                            }),
                    SizeOfMap,SelectedFloor);
        }

        private bool CanLinkToLowerFloor(CellViewModel obj)
        {
            return Floors.Contains(SelectedFloor - 1);
        }

        private void LinkToLowerFloorCommand(CellViewModel obj)
        {
            var calculationReady = false;
            var pairCellNumber = -1;
            if (FloorPlanViewModel.LinkToLowerRequest != null)
            {
                pairCellNumber = FloorPlanViewModel.LinkToLowerRequest.Value.Number;
                FloorPlanViewModel.LinkToLowerRequest = null;
                calculationReady = true;
            }
            if(calculationReady)
            {
                var targetCellId = obj.Value.Number;
                var targetCell = FloorPlanViewModel.Cells.First(c => c.Value.Number == targetCellId);
                targetCell.Value.BottomCell = pairCellNumber;
                targetCell.MarkFloorCrossings();
                SelectedFloor = SelectedFloor - 1;

                var pairCell = FloorPlanViewModel.Cells.First(c => c.Value.Number == pairCellNumber);
                pairCell.Value.TopCell = targetCellId;
                pairCell.MarkFloorCrossings();
                return;
            }
            SelectedFloor = SelectedFloor - 1;
            FloorPlanViewModel.LinkToUpperRequest = obj;
        }

        private bool CanLinkToUpperFloor(CellViewModel obj)
        {
            return Floors.Contains(SelectedFloor + 1);
        }

        private void LinkToUpperFloorCommand(CellViewModel obj)
        {
            var calculationReady = false;
            var pairCellNumber = -1;

            if (FloorPlanViewModel.LinkToUpperRequest != null)
            {
                pairCellNumber = FloorPlanViewModel.LinkToUpperRequest.Value.Number;
                FloorPlanViewModel.LinkToUpperRequest = null;
                calculationReady = true;

            }
            if (calculationReady)
            {
                var targetCellId = obj.Value.Number;
                var targetCell = FloorPlanViewModel.Cells.First(c => c.Value.Number == targetCellId);
                targetCell.Value.TopCell = pairCellNumber;
                targetCell.MarkFloorCrossings();
                SelectedFloor = SelectedFloor + 1;

                var pairCell = FloorPlanViewModel.Cells.First(c => c.Value.Number == pairCellNumber);
                pairCell.Value.BottomCell = targetCellId;
                pairCell.MarkFloorCrossings();
                return;
            }
            SelectedFloor = SelectedFloor + 1;
            FloorPlanViewModel.LinkToLowerRequest = obj;
        }

        private void AddNewFloorCommand()
        {
            var newFloor = Floors.Max() + 1;
            Floors.Add(newFloor);
            var allExistingCells = _level.CurrentMap.Cells.ToList();
            AddAFloor(allExistingCells,newFloor);
            _level.CurrentMap.Cells = allExistingCells.ToArray();
            _level.CurrentMap.LinkToParentLevel(_level);
        }

        public ObservableCollection<string> Tilesets
        {
            get { return _tilesets; }
            set
            {
                if (value != _tilesets)
                {
                    _tilesets = value;
                    RaisePropertyChanged("Tilesets");
                }
            }
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
            Map newMap = new Map();

            var allCells = new List<Cell>();
            AddAFloor(allCells,0);
            newMap.Cells = allCells.ToArray();
            newMap.LinkToParentLevel(_level);
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

        public IEnumerable<int> CurrentFloorPlanCellNumbers { get { return FloorPlanViewModel.Cells.Select(c => c.Value.Number); } }
        public IEnumerable<int> UpperLevelFloorPlanCellNumbers { get
        {
            if(Floors.Contains(SelectedFloor-1))
                return _level.CurrentMap.Cells.Where(c => c.Floor == SelectedFloor - 1).Select(c => c.Number).Union(new[]
                                                                                                                     {
                                                                                                                         -1
                                                                                                                     });
            return null;
        }
        }
        public IEnumerable<int> LowerLevelFloorPlanCellNumbers { get
        {
            if (Floors.Contains(SelectedFloor + 1))
                return
                    _level.CurrentMap.Cells.Where(c => c.Floor == SelectedFloor + 1).Select(c => c.Number).Union(new[]
                                                                                                                     {
                                                                                                                         -1
                                                                                                                     });
            return null;
        }
        }
        private List<FloorPlanViewModel> _floorPlanViewModels;

        public FloorPlanViewModel FloorPlanViewModel
        {
            get { return _floorPlanViewModels.FirstOrDefault(f=>f.FloorNumber==SelectedFloor); }
            set
            {
                if(_floorPlanViewModels==null)
                    _floorPlanViewModels= new List<FloorPlanViewModel>();
                if (_floorPlanViewModels.All(f => f.FloorNumber != value.FloorNumber))
                {
                    _floorPlanViewModels.Add(value);
                    RaisePropertyChanged("FloorPlanViewModel");
                }
            }
        }

        private void MarkAsStartCommand(CellViewModel obj)
        {
            _level.StartingCell = obj.Value.Number;
            _floorPlanViewModels.ForEach(f=>f.ResetStartCell());
            obj.StartCellMarking = ">>";
        }

        private void MarkAsObjectiveCommand(CellViewModel obj)
        {
            _level.ObjectiveCell = obj.Value.Number;
            _floorPlanViewModels.ForEach(f => f.ResetObjectiveCell());
            obj.ObjectiveCellMarking = "x";
        }


        private int _selectedFloor;
        private ObservableCollection<int> _floors;
        private ObservableCollection<string> _tilesets;
        private IEnumerable<Tile> _allAvailableTiles;

        public string SelectedTileset
        {
            get { return _level.CurrentMap.Tileset; }
            set
            {
                if (_level.CurrentMap.Tileset != value)
                {
                    _level.CurrentMap.Tileset = value;
                    _allAvailableTiles = GetAllAvailableTilesFromTileSet(value);
                    RaisePropertyChanged("SelectedTileset");
                }
            }
        }

        private IEnumerable<Tile> GetAllAvailableTilesFromTileSet(string tileset)
        {
            BitmapSource bmpSource = new BitmapImage(new Uri(@"file://" + AppDomain.CurrentDomain.BaseDirectory + "/ImageLibrary/" + tileset + ".png"));

            var sourceRectangles = new Int32Rect[64];

            int tile = 0;

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    sourceRectangles[tile] = new Int32Rect(
                    x * 32,
                    y * 32,
                    32,
                    32);
                    yield return new Tile {Index = tile, Image = new CroppedBitmap(bmpSource, sourceRectangles[tile])};
                    tile++;
                }
        }

        public int SelectedFloor
        {
            get { return _selectedFloor; }
            set
            {
                if (_selectedFloor != value)
                {
                    _selectedFloor = value;
                    if (_floorPlanViewModels.All(f => f.FloorNumber != value))
                    {
                        FloorPlanViewModel = new FloorPlanViewModel(
                            _level.CurrentMap.Cells.Where(c => c.Floor == SelectedFloor).Select(
                                c =>
                                new CellViewModel
                                    {
                                        Value = c,
                                        RowIndex = (c.Number-SelectedFloor*((int) SizeOfMap*(int) SizeOfMap))/(int) SizeOfMap,
                                        ColumnIndex = (c.Number - SelectedFloor * ((int)SizeOfMap * (int)SizeOfMap)) % (int)SizeOfMap,
                                        MarkAsObjective=new RelayCommand<CellViewModel>(MarkAsObjectiveCommand),
                                        MarkAsStart = new RelayCommand<CellViewModel>(MarkAsStartCommand),
                                        LinkToUpperFloor = new RelayCommand<CellViewModel>(LinkToUpperFloorCommand, CanLinkToUpperFloor),
                                        LinkToLowerFloor = new RelayCommand<CellViewModel>(LinkToLowerFloorCommand, CanLinkToLowerFloor),
                                        ClearUp = new RelayCommand<CellViewModel>(ClearUpCommand, CanClearUp),
                                        ClearDown = new RelayCommand<CellViewModel>(ClearDownCommand, CanClearDown),
                                        AllObjectsAvailable = _allObjectsAvailable,
                                        AllCharactersAvailable=_allCharactersAvailable,
                                        OcupancyMarker = CellViewModel.GetColorForCell(c.AllObjects.FirstOrDefault(),c.AllCharacters.FirstOrDefault(),c,_allAvailableTiles),
                                        OccupantName = CellViewModel.GetOccupantName(c.AllObjects.FirstOrDefault(), c.AllCharacters.FirstOrDefault()),
                                        LinkToUpperText = "Link to Upper Floor",
                                        LinkToLowerText = "Link to Lower Floor",
                                        LinkToUpperMarking = (c.TopCell == -1) ? "" : "^",
                                        LinkToUpperCell = ((c.TopCell == -1) ? "" : c.TopCell.ToString()),
                                        LinkToLowerMarking = (c.BottomCell == -1) ? "" : "V",
                                        LinkToLowerCell = ((c.BottomCell == -1) ? "" : c.BottomCell.ToString()),
                                        AllAvailableTiles = _allAvailableTiles
                                    }),
                            SizeOfMap, SelectedFloor) ;
                    }
                    RaisePropertyChanged("SelectedFloor");
                    RaisePropertyChanged("FloorPlanViewModel");
                    RaisePropertyChanged("CurrentFloorPlanCellNumbers");
                    RaisePropertyChanged("UpperFloorPlanCellNumbers");
                    RaisePropertyChanged("LowerFloorPlanCellNumbers");

                }
            }
        }

        private bool CanClearDown(CellViewModel obj)
        {
            return obj!=null && obj.Value.BottomCell != -1;
        }

        private void ClearDownCommand(CellViewModel obj)
        {
            SelectedFloor -= 1;
            var pairCell=FloorPlanViewModel.Cells.First(c => c.Value.Number == obj.Value.BottomCell);
            pairCell.Value.TopCell = -1;
            pairCell.MarkFloorCrossings();
            SelectedFloor += 1;
            FloorPlanViewModel.Cells.First(c => c.Value.Number == obj.Value.Number).Value.BottomCell = -1;
            obj.MarkFloorCrossings();
        }

        private bool CanClearUp(CellViewModel obj)
        {
            return obj!=null && obj.Value.TopCell != -1;
        }

        private void ClearUpCommand(CellViewModel obj)
        {
            SelectedFloor += 1;
            var pairCell = FloorPlanViewModel.Cells.First(c => c.Value.Number == obj.Value.TopCell);
            pairCell.Value.BottomCell = -1;
            pairCell.MarkFloorCrossings();
            SelectedFloor -= 1;
            FloorPlanViewModel.Cells.First(c => c.Value.Number == obj.Value.Number).Value.TopCell = -1;
            obj.MarkFloorCrossings();
        }
    }
}
