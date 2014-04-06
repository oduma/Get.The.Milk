using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Characters;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionPanelViewModel:ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        private MovementAction _movementType;

        public MovementAction MovementType
        {
            get { return _movementType; }
            set
            {
                if (value != _movementType)
                {
                    _movementType = value;
                    _movementType.ActiveCharacter = _player;
                    RaisePropertyChanged("MovementType");
                }
            }
        }

        public ActionPanelViewModel(Player player)
        {
            _player = player;
            MovementType = new Walk();
            KeyPressed = new RelayCommand<KeyEventArgs>(KeyPressedCommand);
            KeyUnPressed=new RelayCommand<KeyEventArgs>(KeyUnPressedCommand);
            PerformAction=new RelayCommand<ActionWithTargetModel>(PerformActionCommand);
            Actions= new ObservableCollection<ActionWithTargetModel>();
            InventoryShowHide = "Show Inventory";
        }

        private void PerformActionCommand(ActionWithTargetModel obj)
        {
            if(ActionExecutionRequest!=null)
                ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(obj.Action));
        }

        private void KeyUnPressedCommand(KeyEventArgs obj)
        {
            if (obj.Key == Key.RightCtrl || obj.Key == Key.LeftCtrl)
            {
                MovementType = new Walk();
                MovementType.ActiveCharacter = _player;
            }

        }

        private void KeyPressedCommand(KeyEventArgs obj)
        {
            if (obj.Key == Key.RightCtrl || obj.Key == Key.LeftCtrl)
            {
                MovementType=new Run();
                MovementType.ActiveCharacter = _player;
            }
            var direction = CardinalStar.GetDirectionByShortcut(obj.Key.ToString());
            if (direction != Direction.None)
            {
                MovementType.Direction = direction;
                if (ActionExecutionRequest != null)
                    ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(MovementType));
            }
            else
            {
                if(obj.Key.ToString().ToUpper()=="I")
                {
                    ExposeInventory exposeInventory = ToggleInventory();

                    if (ActionExecutionRequest != null)
                        ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(exposeInventory));

                }
            }
        }

        private ExposeInventory ToggleInventory()
        {
            if (InventoryShowHide == "Show Inventory")
            {
                InventoryShowHide = "Hide Inventory";
                ExposeInventory exposeInventory = new ExposeInventory
                                                  {
                                                      ActiveCharacter = _player,
                                                      TargetCharacter = _player
                                                  };
                exposeInventory.IncludeWallet = false;
                return exposeInventory;
            }
            InventoryShowHide = "Show Inventory";
            return null;
        }

        private string _inventoryShowHide;
        private Player _player;

        public string InventoryShowHide
        {
            get { return _inventoryShowHide; }
            set
            {
                if (value != _inventoryShowHide)
                {
                    _inventoryShowHide = value;
                    RaisePropertyChanged("InventoryShowHide");
                }

            }
        }
        public CardinalStar Directions { get { return new CardinalStar(); } }

        public ObservableCollection<ActionWithTargetModel> Actions { get; set; }
        
        public RelayCommand<KeyEventArgs> KeyPressed { get; private set; }

        public RelayCommand<KeyEventArgs> KeyUnPressed { get; private set; }

        public RelayCommand<ActionWithTargetModel> PerformAction { get; private set; }

        public void DisplayPossibleActions(List<GameAction> possibleActions)
        {
            Actions.Clear();
            foreach (var possibleAction in possibleActions)
                Actions.Add(new ActionWithTargetModel{Action=possibleAction});
        }

    }
}
