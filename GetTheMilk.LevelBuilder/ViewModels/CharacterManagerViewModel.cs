using System.Collections.ObjectModel;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class CharacterManagerViewModel:ViewModelBase
    {
        private CharacterViewModel _currentCharacterViewModel;

        public CharacterViewModel CurrentCharacterViewModel
        {
            get { return _currentCharacterViewModel; }
            set
            {
                if (value != _currentCharacterViewModel)
                {
                    _currentCharacterViewModel = value;
                    RaisePropertyChanged("CurrentCharacterViewModel");
                }
            }

        }


        public RelayCommand CreateNewCharacter { get; private set; }
        public RelayCommand Done { get; private set; }

        public CharacterManagerViewModel(ObservableCollection<NonCharacterObject> allAvailableObjects)
        {
            AllAvailableObjects = allAvailableObjects;
            CreateNewCharacter= new RelayCommand(DisplayNewCharacterEditor);
            Done= new RelayCommand(DoneCommand);
        }

        private void DispalyCharacterEditor()
        {
                    CurrentCharacterViewModel = new CharacterViewModel(SelectedCharacter,AllAvailableObjects);

        }

        private void DoneCommand()
        {
            if(AllExistingCharacters==null)
                AllExistingCharacters=new ObservableCollection<Character>();
            foreach (var obj in CurrentCharacterViewModel.CharacterInventory)
                CurrentCharacterViewModel.Value.Inventory.Add(obj);
            CurrentCharacterViewModel.Value.Inventory.LinkObjectsToInventory();
            AllExistingCharacters.Add(CurrentCharacterViewModel.Value);
            DisplayNewCharacterEditor();
        }

        private void DisplayNewCharacterEditor()
        {
            CurrentCharacterViewModel = new CharacterViewModel(new Character(),AllAvailableObjects);
        }

        private Character _selectedCharacter;
        public Character SelectedCharacter
        {
            get { return _selectedCharacter; }
            set
            {
                if (value != _selectedCharacter)
                {
                    _selectedCharacter = value;
                    RaisePropertyChanged("SelectedCharacter");
                    DispalyCharacterEditor();
                }
            }
        }
        private ObservableCollection<Character> _allExistingCharacters;
        public ObservableCollection<NonCharacterObject> AllAvailableObjects { get; set; }

        public ObservableCollection<Character> AllExistingCharacters
        {
            get { return _allExistingCharacters; }
            set
            {
                if (value != _allExistingCharacters)
                {
                    _allExistingCharacters = value;
                    RaisePropertyChanged("AllExistingCharacters");
                }
            }
        }


    }
}
