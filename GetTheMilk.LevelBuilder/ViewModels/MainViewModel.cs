using System;
using System.Collections.ObjectModel;
using System.IO;
using GetTheMilk.Characters;
using GetTheMilk.Levels;
using GetTheMilk.Objects;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Utils.IO;
using Newtonsoft.Json;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private LevelPropertiesViewModel _levelPropertiesViewModel;
        private ObjectManagerViewModel _objectManagerViewModel;
        private CharacterManagerViewModel _characterManagerViewModel;

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    RaisePropertyChanged("CurrentViewModel");
                }
            }

        }

        private Level _level;
        private ObservableCollection<SizeOfLevel> _allSizes;
        private LevelMapViewModel _levelMapViewModel;
        private InteractionManagerViewModel _interactionManagerViewModel;

        public MainViewModel()
        {

            CreateANewLevel(SizeOfLevel.VerySmall);

            CreateNewLevel = new RelayCommand<SizeOfLevel>(CreateANewLevel);

            SaveCommand = new RelayCommand(Save);

            LoadCommand = new RelayCommand(Load);

            ExitCommand = new RelayCommand(Exit);

            GetLevelProperties = new RelayCommand(DisplayLevelProperties);

            GetLevelMap = new RelayCommand(DisplayLevelMap);

            ManageObjects=new RelayCommand(DisplayObjectManager);

            ManageCharacters=new RelayCommand(DisplayCharacterManager);

            ManageInteractions= new RelayCommand(DisplayInteractionManager);

        }

        private void DisplayInteractionManager()
        {
            if (_interactionManagerViewModel == null)
                _interactionManagerViewModel = new InteractionManagerViewModel();
            CurrentViewModel = _interactionManagerViewModel;
        }

        private void DisplayCharacterManager()
        {

            if (_characterManagerViewModel == null)
                _characterManagerViewModel = new CharacterManagerViewModel((_objectManagerViewModel == null) ? null : _objectManagerViewModel.AllExistingObjects, (_interactionManagerViewModel == null) ? null : _interactionManagerViewModel.AllExistingInteractions);
            else
            {
                _characterManagerViewModel.AllAvailableObjects = (_objectManagerViewModel == null)
                                                                     ? null
                                                                     : _objectManagerViewModel.AllExistingObjects;
            }
            CurrentViewModel = _characterManagerViewModel;
        }

        private void DisplayObjectManager()
        {
            if(_objectManagerViewModel==null)
                _objectManagerViewModel=new ObjectManagerViewModel();

            CurrentViewModel = _objectManagerViewModel;
        }

        private void DisplayLevelMap()
        {
            if (_level == null)
                CreateANewLevel(SizeOfLevel.VerySmall);
            CurrentViewModel = _levelMapViewModel ?? new LevelMapViewModel(_level,(_objectManagerViewModel==null)?null:_objectManagerViewModel.AllExistingObjects);
        }

        private void DisplayLevelProperties()
        {
            if(_level==null)
                CreateANewLevel(SizeOfLevel.VerySmall);
            CurrentViewModel = _levelPropertiesViewModel ?? new LevelPropertiesViewModel(_level);
        }

        private void Exit()
        {
            throw new System.NotImplementedException();
        }

        private void Load()
        {
            CurrentViewModel = new LoadLevelViewModel(AppDomain.CurrentDomain.BaseDirectory,"*.gdu",LoadALevel);
        }

        private void LoadALevel(string fileName)
        {
            using (TextReader textReader = new StreamReader(ReadWriteStrategies.UncompressedReader(fileName)))
            {

                var levelPackages = JsonConvert.DeserializeObject<LevelPackages>(textReader.ReadToEnd());
                _level = Level.Create(levelPackages);
            }
            _levelPropertiesViewModel = new LevelPropertiesViewModel(_level);
            CurrentViewModel = _levelPropertiesViewModel;
        }

        private void Save()
        {
            ReadWriteStrategies.UncompressedWriter(JsonConvert.SerializeObject(_level.PackageForSave()),
                                                   string.Format("GL{0}.gdu", _level.Number));
        }

        private void CreateANewLevel(SizeOfLevel sizeOfLevel)
        {
            _level = new Level();
            _level.SizeOfLevel = sizeOfLevel;
            _levelPropertiesViewModel = new LevelPropertiesViewModel(_level);
            _level.Inventory = new Inventory {MaximumCapacity = (int) sizeOfLevel*(int) sizeOfLevel};
            _level.Characters = new CharacterCollection();

            CurrentViewModel = _levelPropertiesViewModel;

        }

        public RelayCommand<SizeOfLevel> CreateNewLevel { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand LoadCommand { get; private set; }

        public RelayCommand ExitCommand { get; private set; }

        public RelayCommand GetLevelProperties { get; private set; }

        public RelayCommand GetLevelMap { get; private set; }

        public RelayCommand ManageObjects { get; private set; }

        public RelayCommand ManageCharacters { get; private set; }

        public RelayCommand ManageInteractions { get; private set; }

        public ObservableCollection<SizeOfLevel> AllSizes 
        {
            get
            {
                return
                    _allSizes =
                    (_allSizes) ??
                    new ObservableCollection<SizeOfLevel>()
                        {SizeOfLevel.VerySmall,SizeOfLevel.Small, SizeOfLevel.Medium, SizeOfLevel.Big, SizeOfLevel.Huge};
            }
        }
    }
}
