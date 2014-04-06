using GetTheMilk.Actions.BaseActions;
using GetTheMilk.UI.Translators;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionWithTargetModel
    {
        private GameAction _action;
        private string _displayValue;

        public GameAction Action
        {
            get { return _action; }
            set
            {
                _action = value;
                if(_action !=null)
                {

                    _displayValue = ActionToHuL.TranslateAction(_action);
                }
            }
        }
        public string DisplayValue
        {
            get { return _displayValue; }
            private set { _displayValue = value; }
        }

    }
}
