using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Navigation;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionPanelViewModel:ViewModelBase
    {
        public List<MovementAction> MovementActions
        {
            get { return new List<MovementAction> {new Run(), new Walk()}; }
        }

        public List<Direction> Directions
        {
            get
            {
                return new List<Direction>
                           {
                               Direction.Top,
                               Direction.Bottom,
                               Direction.North,
                               Direction.East,
                               Direction.South,
                               Direction.West
                           };
            }
        }

    }
}
