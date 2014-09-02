using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.GameLevels;

namespace GetTheMilk.UI.Console.ViewModels
{
    public class ActionPanelViewModel:ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        private MovementActionTemplate _movement;

        public MovementActionTemplate Movement
        {
            get { return _movement; }
            set
            {
                if (value != _movement)
                {
                    _movement = value;
                    _movement.ActiveCharacter = _player;
                    RaisePropertyChanged("MovementType");
                }
            }
        }

        public ActionPanelViewModel(Player player)
        {
            _player = player;
            Movement =
                player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
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
                Movement =
                _player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
                Movement.ActiveCharacter = _player;
            }

        }

        private void KeyPressedCommand(KeyEventArgs obj)
        {
            if (obj.Key == Key.RightCtrl || obj.Key == Key.LeftCtrl)
            {
                Movement =
                _player.CreateNewInstanceOfAction<MovementActionTemplate>("Run");
                Movement.ActiveCharacter = _player;
            }
            var direction = CardinalStar.GetDirectionByShortcut(obj.Key.ToString());
            if (direction != Direction.None)
            {
                Movement.Direction = direction;
                if (ActionExecutionRequest != null)
                    ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(Movement));
            }
            else
            {
                if(obj.Key.ToString().ToUpper()=="I")
                {
                    var exposeInventory = ToggleInventory();

                    if (ActionExecutionRequest != null)
                        ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(exposeInventory));

                }
            }
        }

        private ExposeInventoryActionTemplate ToggleInventory()
        {
            if (InventoryShowHide == "Show Inventory")
            {
                InventoryShowHide = "Hide Inventory";
                var exposeInventory = _player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
                exposeInventory.ActiveCharacter = _player;
                exposeInventory.TargetCharacter = _player;
                exposeInventory.FinishingAction = ExposeInventoryFinishingAction.CloseInventory;
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

        public void DisplayPossibleActions(IEnumerable<BaseActionTemplate> possibleActions)
        {
            Actions.Clear();
            foreach (var possibleAction in possibleActions)
                Actions.Add(new ActionWithTargetModel{Action=possibleAction});
        }

    }
}
