﻿using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.UI.Translators;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionWithTargetModel
    {
        private BaseActionTemplate _action;
        private string _displayValue;
        public BaseActionTemplate Action
        {
            get { return _action; }
            set
            {
                _action = value;
                if(_action !=null)
                {

                    _displayValue = _action.ToString();
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
