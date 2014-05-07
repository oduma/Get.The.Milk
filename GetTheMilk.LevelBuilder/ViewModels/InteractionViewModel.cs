using System;
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
        private ObservableCollection<GameAction> _allAvailableActions;
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
                    SelectedAction = value.Action;
                    SelectedReaction = value.Reaction;
                }
            }
        }

        public GameAction SelectedAction
        {
            get { return Value.Action; }
            set
            {
                if (value != Value.Action)
                {
                    Value.Action = value.CreateNewInstance();
                    RaisePropertyChanged("SelectedAction");
                    ActionProperties = DisplayActionProperties(value.GetType(),Value.Action);
                }
            }
        }

        public GameAction SelectedReaction
        {
            get { return Value.Reaction; }
            set
            {
                if (value != Value.Reaction)
                {
                    Value.Reaction = value.CreateNewInstance();
                    RaisePropertyChanged("SelectedReaction");
                    ReactionProperties = DisplayActionProperties(value.GetType(),Value.Reaction);
                }
            }
        }

        private ObservableCollection<ActionPropertyViewModel> DisplayActionProperties(Type actionType,GameAction action)
        {
            var displayProperties =
                actionType.GetProperties().Where(
                    p => p.GetCustomAttributes(typeof (LevelBuilderAccesiblePropertyAttribute), false).Any());
            var props=new ObservableCollection<ActionPropertyViewModel>();
            foreach (var displayProperty in displayProperties)
            {
                props.Add(new ActionPropertyViewModel
                              {
                                  PropertyName = displayProperty.Name,
                                  PropertyType =
                                      ((LevelBuilderAccesiblePropertyAttribute)
                                       displayProperty.GetCustomAttributes(
                                           typeof (LevelBuilderAccesiblePropertyAttribute), false).First()).SourceType,
                                  ParentAction=action,
                                  ActionPropertyInfo=displayProperty
                              });
            }
            return props;
        }


        public InteractionViewModel(ActionReaction selectedInteraction)
        {
            Value = selectedInteraction;
            var actions = ActionsFactory.GetFactory().GetActions().Where(a => a is TwoCharactersAction || a is ExposeInventory);
            if (AllAvailableActions == null)
                AllAvailableActions = new ObservableCollection<GameAction>();
            foreach (var gameAction in actions)
            {
                AllAvailableActions.Add(gameAction);
            }
            RaisePropertyChanged("SelectedAction");
            RaisePropertyChanged("SelectedReaction");
            if (selectedInteraction != null && selectedInteraction.Action != null)
                ActionProperties=DisplayActionProperties(selectedInteraction.Action.GetType(), selectedInteraction.Action);
            if (selectedInteraction != null && selectedInteraction.Reaction != null)
                ReactionProperties = DisplayActionProperties(selectedInteraction.Reaction.GetType(), selectedInteraction.Reaction);
        }

        public ObservableCollection<GameAction> AllAvailableActions
        {
            get { return _allAvailableActions; }
            set
            {
                if(value!=_allAvailableActions)
                {
                    _allAvailableActions = value;
                    RaisePropertyChanged("AllAvailableActions");
                }
            }
        }
    }
}
