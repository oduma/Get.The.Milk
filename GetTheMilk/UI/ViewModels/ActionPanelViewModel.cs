using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

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
                    RaisePropertyChanged("MovementType");
                }
            }
        }

        public ActionPanelViewModel()
        {
            MovementType = new Walk();
            KeyPressed = new RelayCommand<KeyEventArgs>(KeyPressedCommand);
            KeyUnPressed=new RelayCommand<KeyEventArgs>(KeyUnPressedCommand);
            PerformAction=new RelayCommand<ActionWithTargetModel>(PerformActionCommand);
            Actions= new ObservableCollection<ActionWithTargetModel>();
        }

        private void PerformActionCommand(ActionWithTargetModel obj)
        {
            if(ActionExecutionRequest!=null)
                ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(obj.Action,obj.TargetObject));
        }

        private void KeyUnPressedCommand(KeyEventArgs obj)
        {
            if (obj.Key == Key.RightCtrl || obj.Key == Key.LeftCtrl)
                MovementType = new Walk();

        }

        private void KeyPressedCommand(KeyEventArgs obj)
        {
            if(obj.Key==Key.RightCtrl || obj.Key==Key.LeftCtrl)
                MovementType=new Run();
            var direction = CardinalStar.GetDirectionByShortcut(obj.Key.ToString());
            if (direction == Direction.None) return;
            MovementType.Direction = direction;
            if(ActionExecutionRequest!=null)
                ActionExecutionRequest(this,new ActionExecutionRequestEventArgs(MovementType));
        }

        public CardinalStar Directions { get { return new CardinalStar(); } }

        public ObservableCollection<ActionWithTargetModel> Actions { get; set; }
        
        public RelayCommand<KeyEventArgs> KeyPressed { get; private set; }

        public RelayCommand<KeyEventArgs> KeyUnPressed { get; private set; }

        public RelayCommand<ActionWithTargetModel> PerformAction { get; private set; }

        public void DisplayPossibleActions(IPositionableObject[] objectsInCell)
        {
            Actions.Clear();
            foreach(var objectInCell in objectsInCell)
            {
                if(objectInCell.StorageContainer.Owner is ILevel)
                {
                    Actions.Add(new ActionWithTargetModel{Action=new Pick(),TargetObject = objectInCell});
                    Actions.Add(new ActionWithTargetModel { Action = new Keep(), TargetObject = objectInCell });
                    Actions.Add(new ActionWithTargetModel { Action = new Kick(), TargetObject = objectInCell});
                }
            }
        }
    }
}
