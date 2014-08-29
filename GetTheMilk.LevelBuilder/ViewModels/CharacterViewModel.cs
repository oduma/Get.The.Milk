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

        public CharacterViewModel(Character selectedCharacter, 
            ObservableCollection<NonCharacterObject> allObjectsAvailable,
            ObservableCollection<Interaction> allAvailableInteractions)
        {
            if (selectedCharacter.Name == null)
                selectedCharacter.Name = new Noun();
            Value = selectedCharacter;
            CurrentInteractionsViewModel = new InteractionsViewModel(Value, allAvailableInteractions);
            AllObjectsAvailable = allObjectsAvailable;
            var characterTypes = ObjectActionsFactory.ListAllRegisterNames(ObjectCategory.Character);
            if (AllObjectTypes == null)
                AllObjectTypes = new ObservableCollection<string>();
            foreach (var objectType in characterTypes)
                AllObjectTypes.Add(objectType);
            if(CharacterInventory==null)
                CharacterInventory=new ObservableCollection<NonCharacterObject>();
            foreach(var obj in Value.Inventory)
                CharacterInventory.Add(obj);
            MoveToInventory=new RelayCommand(MoveToInventoryCommand,CanMoveToInventory);
            RemoveFromInventory= new RelayCommand(RemoveFromInventoryCommand);
        }

        private bool CanMoveToInventory()
        {
            return Value.Inventory.MaximumCapacity > Value.Inventory.Count+1;
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
    }
}
