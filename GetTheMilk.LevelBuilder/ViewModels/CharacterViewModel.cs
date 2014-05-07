using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Utils;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class CharacterViewModel:ObjectViewModelBase<Character>
    {
        public RelayCommand MoveToInventory { get; private set; }
        public RelayCommand RemoveFromInventory { get; private set; }
        public RelayCommand MoveToCharacterSpecificInteractions { get; private set; }
        public RelayCommand RemoveFromCharacterSpecificInteractions { get; private set; }
        public RelayCommand MoveToPlayerResponseInteractions { get; private set; }
        public RelayCommand RemoveFromPlayerResponseInteractions { get; private set; }

        public CharacterViewModel(Character selectedCharacter, 
            ObservableCollection<NonCharacterObject> allObjectsAvailable,
            ObservableCollection<ActionReaction> allAvailableInteractions)
        {
            if (selectedCharacter.Name == null)
                selectedCharacter.Name = new Noun();
            if(selectedCharacter.InteractionRules==null)
                selectedCharacter.InteractionRules= new SortedList<string, ActionReaction[]>();
            Value = selectedCharacter;
            AllObjectsAvailable = allObjectsAvailable;
            AllAvailableInteractions = allAvailableInteractions;
            var characterTypes = ObjectActionsFactory.GetFactory().ListAllRegisterNames(ObjectCategory.Character);
            if (AllObjectTypes == null)
                AllObjectTypes = new ObservableCollection<string>();
            foreach (var objectType in characterTypes)
                AllObjectTypes.Add(objectType);
            if(CharacterInventory==null)
                CharacterInventory=new ObservableCollection<NonCharacterObject>();
            foreach(var obj in Value.Inventory)
                CharacterInventory.Add(obj);
            if(CharacterSpecificInteractions==null)
                CharacterSpecificInteractions= new ObservableCollection<ActionReaction>();
            if(Value.InteractionRules.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
            {
                foreach( var car in Value.InteractionRules[GenericInteractionRulesKeys.CharacterSpecific])
                    CharacterSpecificInteractions.Add(car);
            }
            if (PlayerInteractions == null)
                PlayerInteractions = new ObservableCollection<ActionReaction>();
            if(Value.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
            {
                foreach (var car in Value.InteractionRules[GenericInteractionRulesKeys.PlayerResponses])
                    PlayerInteractions.Add(car);
            }

            MoveToInventory=new RelayCommand(MoveToInventoryCommand);
            RemoveFromInventory= new RelayCommand(RemoveFromInventoryCommand);
            MoveToCharacterSpecificInteractions=new RelayCommand(MoveToCharacterSpecificInteractionsCommand);
            RemoveFromCharacterSpecificInteractions=new RelayCommand(RemoveFromCharacterSpecificInteractionsCommand);
            MoveToPlayerResponseInteractions=new RelayCommand(MoveToPlayerResponseInteractionsCommand);
            RemoveFromPlayerResponseInteractions=new RelayCommand(RemoveFromPlayerResponseInteractionsCommand);
        }

        private void RemoveFromPlayerResponseInteractionsCommand()
        {
            AllAvailableInteractions.Add(SelectedPlayerInteraction);
            PlayerInteractions.Remove(SelectedPlayerInteraction);
        }

        public ActionReaction SelectedPlayerInteraction
        {
            get { return _selectedPlayerInteraction; }
            set
            {
                if(value!=_selectedPlayerInteraction)
                {
                    _selectedPlayerInteraction = value;
                    RaisePropertyChanged("SelectedPlayerInteraction");
                }
            }
        }

        private void MoveToPlayerResponseInteractionsCommand()
        {
            PlayerInteractions.Add(SelectedAvailablePlayerInteraction);
            AllAvailableInteractions.Remove(SelectedAvailablePlayerInteraction);
        }

        public ActionReaction SelectedAvailablePlayerInteraction
        {
            get { return _selectedAvailablePlayerInteraction; }
            set
            {
                if(value!=_selectedAvailablePlayerInteraction)
                {
                    _selectedAvailablePlayerInteraction = value;
                    RaisePropertyChanged("SelectedAvailablePlayerInteraction");
                }
            }
        }

        private void RemoveFromCharacterSpecificInteractionsCommand()
        {
            AllAvailableInteractions.Add(SelectedCharacterInteraction);
            CharacterSpecificInteractions.Remove(SelectedCharacterInteraction);
        }

        public ActionReaction SelectedCharacterInteraction
        {
            get { return _selectedCharacterInteraction; }
            set
            {
                if(value!=_selectedCharacterInteraction)
                {
                    _selectedCharacterInteraction = value;
                    RaisePropertyChanged("SelectedCharacterInteraction");
                }
            }
        }

        private void MoveToCharacterSpecificInteractionsCommand()
        {
            CharacterSpecificInteractions.Add(SelectedAvailableCharacterInteraction);
            AllAvailableInteractions.Remove(SelectedAvailableCharacterInteraction);
        }

        public ActionReaction SelectedAvailableCharacterInteraction
        {
            get { return _selectedAvailableCharacterInteraction; }
            set
            {
                if(value!=_selectedAvailableCharacterInteraction)
                {
                    _selectedAvailableCharacterInteraction = value;
                    RaisePropertyChanged("SelectedAvailableCharacterInteraction");
                }
            }
        }

        public ObservableCollection<ActionReaction> AllAvailableInteractions
        {
            get { return _allAvailableInteractions; }
            set
            {
                if(value!=_allAvailableInteractions)
                {
                    _allAvailableInteractions = value;
                    RaisePropertyChanged("AllAvailableInteractions");
                }
            }
        }

        private void RemoveFromInventoryCommand()
        {
            CharacterInventory.Remove(SelectedObject);
            AllObjectsAvailable.Add(SelectedObject);
        }

        private void MoveToInventoryCommand()
        {
            CharacterInventory.Add(SelectedAvailableObject);
            AllObjectsAvailable.Remove(SelectedAvailableObject);
        }


        private NonCharacterObject _selectedAvailableObject;
        private NonCharacterObject _selectedObject;

        public NonCharacterObject SelectedAvailableObject
        {
            get { return _selectedAvailableObject; }
            set
            {
                if (value != _selectedAvailableObject)
                {
                    _selectedAvailableObject = value;
                    RaisePropertyChanged("SelectedAvailableObject");
                }
            }
        }

        public NonCharacterObject SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                if (value != _selectedObject)
                {
                    _selectedObject = value;
                    RaisePropertyChanged("SelectedObject");
                }
            }
        }

        private Character _value;
        private ObservableCollection<NonCharacterObject> _allObjectsAvailable;
        private ObservableCollection<NonCharacterObject> _characterInventory;
        private ObservableCollection<ActionReaction> _allAvailableInteractions;
        private ActionReaction _selectedAvailableCharacterInteraction;
        private ActionReaction _selectedAvailablePlayerInteraction;
        private ActionReaction _selectedCharacterInteraction;
        private ActionReaction _selectedPlayerInteraction;
        private ObservableCollection<ActionReaction> _characterSpecificInteractions;
        private ObservableCollection<ActionReaction> _playerInteractions;

        public override Character Value
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

        public ObservableCollection<NonCharacterObject> AllObjectsAvailable
        {
            get
            {
                return _allObjectsAvailable;
            }
            set
            {
                if(value!=_allObjectsAvailable)
                {
                    _allObjectsAvailable = value;
                    RaisePropertyChanged("AllAvailableObjects");
                }
            }
        }

        public ObservableCollection<NonCharacterObject> CharacterInventory
        {
            get { return _characterInventory; }

            set
            {
                if(_characterInventory!=value)
                {
                    _characterInventory = value;
                    RaisePropertyChanged("CharacterInventory");
                }
            }
        }


        public ObservableCollection<ActionReaction> CharacterSpecificInteractions
        {
            get { return _characterSpecificInteractions; }

            set
            {
                if (_characterSpecificInteractions != value)
                {
                    _characterSpecificInteractions = value;
                    RaisePropertyChanged("CharacterSpecificInteractions");
                }
            }
        }

        public ObservableCollection<ActionReaction> PlayerInteractions
        {
            get { return _playerInteractions; }

            set
            {
                if (_playerInteractions != value)
                {
                    _playerInteractions = value;
                    RaisePropertyChanged("PlayerInteractions");
                }
            }
        }

    }
}
