using System;
using System.Collections.ObjectModel;
using System.IO;
using GetTheMilk.Characters;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.Base;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Utils.IO;
using Newtonsoft.Json;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Factories;

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
        private ActionManagerViewModel _actionManagerViewModel;

        public MainViewModel()
        {
            BootstrapRegister bootsrapRegister = new BootstrapRegister();
            bootsrapRegister.RegisterAllComponents();

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

            ManageActions = new RelayCommand(DisplayActionManager);

            _actionManagerViewModel = new ActionManagerViewModel();

            _actionManagerViewModel.AllExistingActions = new ObservableCollection<BaseActionTemplate>();

            _interactionManagerViewModel = new InteractionManagerViewModel();

            _interactionManagerViewModel.AllExistingInteractions = new ObservableCollection<Interaction>();

            _objectManagerViewModel = new ObjectManagerViewModel(_interactionManagerViewModel.AllExistingInteractions);

            _objectManagerViewModel.AllExistingObjects = new ObservableCollection<NonCharacterObject>();

            _characterManagerViewModel = new CharacterManagerViewModel(_objectManagerViewModel.AllExistingObjects,
                                                                       _interactionManagerViewModel.
                                                                           AllExistingInteractions);
            _characterManagerViewModel.AllExistingCharacters= new ObservableCollection<Character>();

        }

        private void DisplayActionManager()
        {
            if (_actionManagerViewModel == null)
                _actionManagerViewModel = new ActionManagerViewModel();
            CurrentViewModel = _actionManagerViewModel;
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
                _objectManagerViewModel=new ObjectManagerViewModel(_interactionManagerViewModel.AllExistingInteractions);

            CurrentViewModel = _objectManagerViewModel;
        }

        private void DisplayLevelMap()
        {
            if (_level == null)
                CreateANewLevel(SizeOfLevel.VerySmall);
            CurrentViewModel = _levelMapViewModel ??
                               new LevelMapViewModel(_level,
                                                     (_objectManagerViewModel == null)
                                                         ? null
                                                         : _objectManagerViewModel.AllExistingObjects,
                                                     (_characterManagerViewModel == null)
                                                         ? null
                                                         : _characterManagerViewModel.AllExistingCharacters);
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

                var levelPackages = JsonConvert.DeserializeObject<ContainerNoActionsPackage>(textReader.ReadToEnd());
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
            _level.Inventory = new Inventory {MaximumCapacity = (int) sizeOfLevel*(int) sizeOfLevel,InventoryType=InventoryType.LevelInventory};
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

        public RelayCommand ManageActions { get; private set; }

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
