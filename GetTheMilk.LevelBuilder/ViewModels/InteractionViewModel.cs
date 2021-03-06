﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Factories;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class InteractionViewModel:ViewModelBase
    {
        private ActionReaction _value;
        private ObservableCollection<ActionType> _allAvailableActionTypes;
        private ObservableCollection<ActionPropertyViewModel> _actionProperties;

        public ObservableCollection<ActionPropertyViewModel> ActionProperties
        {
            get { return _actionProperties; }
            set
            {
                if (value != _actionProperties)
                {
                    _actionProperties = value;
                    RaisePropertyChanged("ActionProperties");
                }
            }
        }

        private ObservableCollection<ActionPropertyViewModel> _reactionProperties;

        public ObservableCollection<ActionPropertyViewModel> ReactionProperties
        {
            get { return _reactionProperties; }
            set
            {
                if (value != _reactionProperties)
                {
                    _reactionProperties = value;
                    RaisePropertyChanged("ReactionProperties");
                }
            }
        }

        public ActionReaction Value
        {
            get { return _value; }
            set
            {
                if(value!=_value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public GameAction CurrentAction
        {
            get { return _currentAction; }
            set
            {
                _currentAction = value;
                ActionProperties = DisplayActionProperties(_currentAction.GetType(), _currentAction, _currentAction);
            }
        }

        public GameAction CurrentReaction
        {
            get { return _currentReaction; }
            set
            {
                _currentReaction = value;
                ReactionProperties = DisplayActionProperties(_currentReaction.GetType(), _currentReaction, _currentReaction);
            }
        }

        private ActionType _selectedActionType;
        public ActionType SelectedActionType
        {
            get { return _selectedActionType; }
            set
            {
                if (value != _selectedActionType)
                {
                    _selectedActionType = value;
                    RaisePropertyChanged("SelectedActionType");
                    CurrentAction = ActionsFactory.GetFactory().CreateNewActionInstance(_selectedActionType);
                }
            }
        }

        private ActionType _selectedReactionType;
        private GameAction _currentAction;
        private GameAction _currentReaction;

        public ActionType SelectedReactionType
        {
            get { return _selectedReactionType; }
            set
            {
                if (value != _selectedReactionType)
                {
                    _selectedReactionType = value;
                    RaisePropertyChanged("SelectedReactionType");
                    CurrentReaction = ActionsFactory.GetFactory().CreateNewActionInstance(_selectedReactionType);

                }
            }
        }

        private ObservableCollection<ActionPropertyViewModel> DisplayActionProperties(Type actionType,
            GameAction action, GameAction sourceAction)
        {
            var displayProperties =
                actionType.GetProperties().Where(
                    p => p.GetCustomAttributes(typeof (LevelBuilderAccesiblePropertyAttribute), false).Any());
            var props=new ObservableCollection<ActionPropertyViewModel>();
            foreach (var displayProperty in displayProperties)
            {
                props.Add(new ActionPropertyViewModel(displayProperty,action,sourceAction)
                              {
                                  PropertyName = displayProperty.Name,
                                  PropertyType =
                                      ((LevelBuilderAccesiblePropertyAttribute)
                                       displayProperty.GetCustomAttributes(
                                           typeof (LevelBuilderAccesiblePropertyAttribute), false).First()).SourceType
                              });
            }
            return props;
        }


        public InteractionViewModel(ActionReaction selectedInteraction)
        {
            var actionTypes = ActionsFactory.GetFactory().GetActions().Where(a => a.ActionCategory=="TwoCharactersAction" || a is ExposeInventory).Select(a=>a.ActionType);
            if (AllAvailableActionTypes == null)
                AllAvailableActionTypes = new ObservableCollection<ActionType>();
            foreach (var actionType in actionTypes)
            {
                AllAvailableActionTypes.Add(actionType);
            }
            Value = selectedInteraction;
            if (selectedInteraction !=null && selectedInteraction.Action != null)
            {
                _selectedActionType =selectedInteraction.Action.ActionType;
                RaisePropertyChanged("SelectedActionType");
                CurrentAction = selectedInteraction.Action;
            }
            if (selectedInteraction != null && selectedInteraction.Reaction != null)
            {
                _selectedReactionType =selectedInteraction.Reaction.ActionType;
                RaisePropertyChanged("SelectedReactionType");
                CurrentReaction = selectedInteraction.Reaction;
            }
        }

        public ObservableCollection<ActionType> AllAvailableActionTypes
        {
            get { return _allAvailableActionTypes; }
            set
            {
                if(value!=_allAvailableActionTypes)
                {
                    _allAvailableActionTypes = value;
                    RaisePropertyChanged("AllAvailableActionTypes");
                }
            }
        }
    }
}
