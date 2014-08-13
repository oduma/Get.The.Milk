using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Utils;

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

        public CharacterManagerViewModel(ObservableCollection<NonCharacterObject> allAvailableObjects, 
            ObservableCollection<Interaction> allAvailableInteractions)
        {
            AllAvailableObjects = allAvailableObjects;
            AllAvailableInteractions = allAvailableInteractions;
            CreateNewCharacter= new RelayCommand(DisplayNewCharacterEditor);
            Done= new RelayCommand(DoneCommand);
        }

        private void DispalyCharacterEditor()
        {
             CurrentCharacterViewModel = new CharacterViewModel(SelectedCharacter,AllAvailableObjects,AllAvailableInteractions);

        }

        public ObservableCollection<Interaction> AllAvailableInteractions { get; set; }

        private void DoneCommand()
        {
            if(AllExistingCharacters==null)
                AllExistingCharacters=new ObservableCollection<Character>();
            foreach (var obj in CurrentCharacterViewModel.CharacterInventory)
                CurrentCharacterViewModel.Value.Inventory.Add(obj);
            RePopulateInteractions(GenericInteractionRulesKeys.AnyCharacter,
                                   CurrentCharacterViewModel.CharacterSpecificInteractions);
            RePopulateInteractions(GenericInteractionRulesKeys.AnyCharacterResponses,
                                   CurrentCharacterViewModel.PlayerInteractions);
            CurrentCharacterViewModel.Value.Inventory.LinkObjectsToInventory();
            if (AllExistingCharacters.Any(c => c.Name.Main == CurrentCharacterViewModel.Value.Name.Main))
                AllExistingCharacters.Remove(
                    AllExistingCharacters.First(c => c.Name.Main == CurrentCharacterViewModel.Value.Name.Main));
            AllExistingCharacters.Add(CurrentCharacterViewModel.Value);
            DisplayNewCharacterEditor();
        }


        private void RePopulateInteractions(string interactionType,ObservableCollection<Interaction> newInteractions)
        {
            if (CurrentCharacterViewModel.Value.Interactions.ContainsKey(interactionType))
                CurrentCharacterViewModel.Value.Interactions.Remove(interactionType);
            CurrentCharacterViewModel.Value.Interactions.Add(interactionType, newInteractions.ToArray());
        }

        private void DisplayNewCharacterEditor()
        {
            CurrentCharacterViewModel = new CharacterViewModel(new Character(),AllAvailableObjects,AllAvailableInteractions);
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
                    if(_selectedCharacter!=null)
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
