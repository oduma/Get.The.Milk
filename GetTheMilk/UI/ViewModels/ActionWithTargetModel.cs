using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionWithTargetModel
    {
        private GameAction _action;
        private NonCharacterObject _targetObject;
        private string _displayValue;

        public GameAction Action
        {
            get { return _action; }
            set
            {
                _action = value;
                if(_action !=null && _targetObject!=null)
                {
                    _displayValue = _action.Name.Present + " " + _targetObject.Name.Narrator;
                }
            }
        }

        public NonCharacterObject TargetObject
        {
            get { return _targetObject; }
            set
            {
                _targetObject = value;

                if (_action != null && _targetObject != null)
                {
                    _displayValue = _action.Name.Present + " " + _targetObject.Name.Narrator;
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
