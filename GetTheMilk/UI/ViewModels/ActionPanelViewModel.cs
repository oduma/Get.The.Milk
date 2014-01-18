using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Navigation;
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
            var direction = Directions.FirstOrDefault(d => d.Shortcuts.Contains(obj.Key.ToString()));
            if(direction!=null)
            {
                MovementType.Direction = direction.Direction;
                if(ActionExecutionRequest!=null)
                    ActionExecutionRequest(this,new ActionExecutionRequestEventArgs(MovementType));
            }
        }

        public List<ShortcutDirection> Directions
        {
            get
            {
                return new List<ShortcutDirection>
                           {
                               new ShortcutDirection
                               {
                               Direction = Direction.Top,
                               Shortcuts=new []{"E"}},
                               new ShortcutDirection
                               {
                               Direction = Direction.Bottom,
                               Shortcuts=new []{"D"}},
                               new ShortcutDirection
                               {
                               Direction = Direction.North,
                               Shortcuts=new []{"W"}},
                               new ShortcutDirection
                               {
                               Direction = Direction.East,
                               Shortcuts=new []{"S"}},
                               new ShortcutDirection
                               {
                               Direction = Direction.South,
                               Shortcuts=new []{"Z"}},
                               new ShortcutDirection
                               {
                               Direction = Direction.West,
                               Shortcuts=new []{"A"}}
                           };
            }
        }

        public RelayCommand<KeyEventArgs> KeyPressed { get; private set; }

        public RelayCommand<KeyEventArgs> KeyUnPressed { get; private set; }

    }
}
