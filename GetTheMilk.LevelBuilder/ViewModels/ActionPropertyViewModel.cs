﻿using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ActionPropertyViewModel:ViewModelBase
    {
        private string _propertyName;
        private Type _propertyType;
        private Visibility _boolInputControlVisibility;
        private Visibility _choiceInputControlVisibility;
        private Visibility _textInputControlVisibility;
        private ObservableCollection<ActionType> _choiceInputControlSource;


        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (value != _propertyName)
                {
                    _propertyName = value;
                    RaisePropertyChanged("PropertyName");
                }
            }
        }

        public object PropertyValue
        {
            get { return _actionPropertyInfo.GetValue(_parentAction); }
            set
            {
                if (value != _actionPropertyInfo.GetValue(_parentAction))
                {
                    _actionPropertyInfo.SetValue(_parentAction,value);
                    RaisePropertyChanged("PropertyValue");
                }
            }
        }

        private PropertyInfo _actionPropertyInfo;

        public Type PropertyType
        {
            get { return _propertyType; }
            set
            {
                if(value!=_propertyType)
                {
                    _propertyType = value;
                    SetPropertyInputControl(value);
                }
            }
        }

        private void SetPropertyInputControl(Type propertyType)
        {
            if(propertyType==typeof(bool))
            {
                BoolInputControlVisibility = Visibility.Visible;
            }
            else if(propertyType==typeof(ActionType))
            {
                ChoiceInputControlVisibility = Visibility.Visible;
                ChoiceInputControlSource = new ObservableCollection<ActionType>();
                var availableACtionTypes = Enum.GetValues(typeof(ActionType));
                foreach (var availableACtionType in availableACtionTypes)
                {
                    ChoiceInputControlSource.Add((ActionType)availableACtionType);
                }
            }
            else
            {
                TextInputControlVisibility = Visibility.Visible;
            }
        }

        public ObservableCollection<ActionType> ChoiceInputControlSource
        {
            get { return _choiceInputControlSource; }
            set
            {
                if(value!=_choiceInputControlSource)
                {
                    _choiceInputControlSource = value;
                    RaisePropertyChanged("ChoiceInputControlSource");
                }
            }
        }

        public Visibility TextInputControlVisibility
        {
            get { return _textInputControlVisibility; }
            set
            {
                if (value != _textInputControlVisibility)
                {
                    _textInputControlVisibility = value;
                    RaisePropertyChanged("TextInputControlVisibility");
                }
                if (value == Visibility.Visible)
                {
                    ChoiceInputControlVisibility = Visibility.Hidden;
                    BoolInputControlVisibility = Visibility.Hidden;
                }
            }
        }

        public Visibility ChoiceInputControlVisibility
        {
            get { return _choiceInputControlVisibility; }
            set
            {
                if (value != _choiceInputControlVisibility)
                {
                    _choiceInputControlVisibility = value;
                    RaisePropertyChanged("ChoiceInputControlVisibility");
                }
                if (value == Visibility.Visible)
                {
                    BoolInputControlVisibility = Visibility.Hidden;
                    TextInputControlVisibility = Visibility.Hidden;
                }
            }
        }

        public Visibility BoolInputControlVisibility
        {
            get
            {
                return _boolInputControlVisibility;
            }
            set
            {
                if(value!=_boolInputControlVisibility)
                {
                    _boolInputControlVisibility = value;
                    RaisePropertyChanged("BoolInputControlVisibility");
                }
                if (value == Visibility.Visible)
                {
                    ChoiceInputControlVisibility = Visibility.Hidden;
                    TextInputControlVisibility = Visibility.Hidden;
                }

            }
        }

        private GameAction _parentAction;

        public ActionPropertyViewModel(PropertyInfo actionPropertyInfo, GameAction parentAction, GameAction sourceAction)
        {
            _actionPropertyInfo = actionPropertyInfo;
            _parentAction = parentAction;
            PropertyValue = _actionPropertyInfo.GetValue(sourceAction);
        }
    }
}
