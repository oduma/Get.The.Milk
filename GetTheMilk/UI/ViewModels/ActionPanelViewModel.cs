using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionPanelViewModel:ViewModelBase
    {
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

    }
}
