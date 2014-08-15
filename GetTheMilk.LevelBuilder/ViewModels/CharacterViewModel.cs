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
            ObservableCollection<Interaction> allAvailableInteractions)
        {
            if (selectedCharacter.Name == null)
                selectedCharacter.Name = new Noun();
            if(selectedCharacter.Interactions==null)
                selectedCharacter.Interactions= new SortedList<string, Interaction[]>();
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
                CharacterSpecificInteractions= new ObservableCollection<Interaction>();
            if(Value.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacter))
            {
                foreach( var car in Value.Interactions[GenericInteractionRulesKeys.AnyCharacter])
                    CharacterSpecificInteractions.Add(car);
            }
            if (PlayerInteractions == null)
                PlayerInteractions = new ObservableCollection<Interaction>();
            if(Value.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
            {
                foreach (var car in Value.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses])
                    PlayerInteractions.Add(car);
            }

            MoveToInventory=new RelayCommand(MoveToInventoryCommand,CanMoveToInventory);
            RemoveFromInventory= new RelayCommand(RemoveFromInventoryCommand);
            MoveToCharacterSpecificInteractions=new RelayCommand(MoveToCharacterSpecificInteractionsCommand);
            RemoveFromCharacterSpecificInteractions=new RelayCommand(RemoveFromCharacterSpecificInteractionsCommand);
            MoveToPlayerResponseInteractions=new RelayCommand(MoveToPlayerResponseInteractionsCommand);
            RemoveFromPlayerResponseInteractions=new RelayCommand(RemoveFromPlayerResponseInteractionsCommand);
        }

        private bool CanMoveToInventory()
        {
            return Value.Inventory.MaximumCapacity > Value.Inventory.Count+1;
        }

        private void RemoveFromPlayerResponseInteractionsCommand()
        {
            AllAvailableInteractions.Add(SelectedPlayerInteraction);
            PlayerInteractions.Remove(SelectedPlayerInteraction);
        }

        public Interaction SelectedPlayerInteraction
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

        public Interaction SelectedAvailablePlayerInteraction
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

        public Interaction SelectedCharacterInteraction
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

        public Interaction SelectedAvailableCharacterInteraction
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

        public ObservableCollection<Interaction> AllAvailableInteractions
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
            AllObjectsAvailable.Add(SelectedObject);
            CharacterInventory.Remove(SelectedObject);
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
                    RaisePropertyChanged("SelectedAction");
                }
            }
        }

        private Character _value;
        private ObservableCollection<NonCharacterObject> _allObjectsAvailable;
        private ObservableCollection<NonCharacterObject> _characterInventory;
        private ObservableCollection<Interaction> _allAvailableInteractions;
        private Interaction _selectedAvailableCharacterInteraction;
        private Interaction _selectedAvailablePlayerInteraction;
        private Interaction _selectedCharacterInteraction;
        private Interaction _selectedPlayerInteraction;
        private ObservableCollection<Interaction> _characterSpecificInteractions;
        private ObservableCollection<Interaction> _playerInteractions;

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

        public override ObjectViewModelBase<Character> Clone()
        {
            throw new System.NotImplementedException();
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


        public ObservableCollection<Interaction> CharacterSpecificInteractions
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

        public ObservableCollection<Interaction> PlayerInteractions
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
