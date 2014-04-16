using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.UI.Translators;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class TwoCharactersViewModel : ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        public event EventHandler<PlayerStatsUpdateRequestEventArgs> PlayerStatsUpdateRequest;

        private TwoCharactersAction _action;
        private int _passiveHealth;
        private ObservableCollection<ActionWithTargetModel> _actions;
        private ObservableCollection<Dialogue> _dialogues;


        public TwoCharactersAction Action
        {
            get { return _action; }
            set
            {
                if (value != _action)
                {
                    if (_action != null)
                        _action.FeedbackFromOriginalAction -= ActionFeedbackFromOriginalAction;
                    _action = value;
                    _action.FeedbackFromOriginalAction += ActionFeedbackFromOriginalAction;
                }
            }
        }

        private void ActionFeedbackFromOriginalAction(object sender, FeedbackEventArgs e)
        {
            Dialogues.Add(new Dialogue
                              {
                                  Who = e.ActionResult.ForAction.ActiveCharacter.Name.Narrator,
                                  What = (new ActionResultToHuL()).TranslateActionResult(e.ActionResult)
                              });
            PassiveHealth = e.ActionResult.ForAction.TargetCharacter.Health;
            if (PlayerStatsUpdateRequest != null)
                PlayerStatsUpdateRequest(this,
                                         new PlayerStatsUpdateRequestEventArgs(e.ActionResult));
        }

        public RelayCommand<ActionWithTargetModel> PerformAction { get; private set; }

        public string Active { get; private set; }

        public string Passive { get; private set; }
        
        public int PassiveHealth
        {
            get { return _passiveHealth; }
            set
            {
                if (value != _passiveHealth)
                {
                    _passiveHealth = value;
                    RaisePropertyChanged("PassiveHealth");
                }
            }
        }

        public TwoCharactersViewModel(ICharacter activeCharacter, ICharacter targetCharacter)
        {
            Active = activeCharacter.Name.Main;
            Passive = targetCharacter.Name.Main;
            PassiveHealth = targetCharacter.Health;
            Dialogues = new ObservableCollection<Dialogue>();
            PerformAction = new RelayCommand<ActionWithTargetModel>(PerformActionCommand);
        }

        private void PerformActionCommand(ActionWithTargetModel obj)
        {
            if(obj.Action is TwoCharactersAction)
            {
                ((TwoCharactersAction) obj.Action).FeedbackFromOriginalAction -= ActionFeedbackFromOriginalAction;
                ((TwoCharactersAction)obj.Action).FeedbackFromOriginalAction += ActionFeedbackFromOriginalAction;
            }
            if (obj.Action.ActionType == ActionType.Communicate)
            {
                Dialogues.Add(new Dialogue { Who = obj.Action.ActiveCharacter.Name.Narrator, What = ((Communicate)obj.Action).Message });
            }
            if (obj.Action.FinishTheInteractionOnExecution || obj.Action.ActionType == ActionType.ExposeInventory)
            {
                if(ActionExecutionRequest!=null)
                    ActionExecutionRequest(this,new ActionExecutionRequestEventArgs(obj.Action));
            }
            else
            {
                RecordActionResult(obj.Action.Perform());
            }

        }

        private void RecordActionResult(ActionResult actionResult)
        {
            if (actionResult.ForAction.ActionType == ActionType.Communicate)
            {
                Dialogues.Add(new Dialogue
                              {
                                  Who = actionResult.ForAction.ActiveCharacter.Name.Main,
                                  What = ((Communicate) actionResult.ForAction).Message
                              });
            }
            if (actionResult.ForAction.ActionType == ActionType.InitiateHostilities)
            {
                Dialogues.Add(new Dialogue
                              {
                                  Who = actionResult.ForAction.ActiveCharacter.Name.Main,
                                  What = GetOpponentActiveWeapons(actionResult.ForAction.ActiveCharacter)
                              });
            }
            if(actionResult.ForAction.ActionType==ActionType.Attack)
            {
                if(actionResult.ResultType==ActionResultType.Win || actionResult.ResultType== ActionResultType.Lost)
                {
                    PerformActionCommand(new ActionWithTargetModel {Action = ((List<GameAction>) actionResult.ExtraData)[0]});
                    return;
                }

                Dialogues.Add(new Dialogue
                                  {
                                      Who = actionResult.ForAction.ActiveCharacter.Name.Narrator,
                                      What = (new ActionResultToHuL()).TranslateActionResult(actionResult)
                                  });
                if(!(actionResult.ForAction.ActiveCharacter is IPlayer))
                {
                    PassiveHealth = actionResult.ForAction.ActiveCharacter.Health;
                    if (PlayerStatsUpdateRequest != null)
                        PlayerStatsUpdateRequest(this,
                                                 new PlayerStatsUpdateRequestEventArgs(actionResult));
                }

            }
            if (actionResult.ForAction.ActionType == ActionType.ExposeInventory)
            {
                if (ActionExecutionRequest != null)
                    ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(actionResult.ForAction));
            }
            else if(actionResult.ForAction.ActionType==ActionType.AcceptQuit)
            {
                if (PlayerStatsUpdateRequest != null)
                    PlayerStatsUpdateRequest(this,
                                             new PlayerStatsUpdateRequestEventArgs(actionResult));
            }
            else
            {
            
            Actions = new ObservableCollection<ActionWithTargetModel>();
            foreach (var availableAction in (List<GameAction>) actionResult.ExtraData)
            {
                Actions.Add(new ActionWithTargetModel {Action = availableAction});
            }
        }
    }

        private string GetOpponentActiveWeapons(ICharacter targetCharacter)
        {
            var attackWeaponMessage = (targetCharacter.ActiveAttackWeapon != null)
                ? string.Format("chose {0} for attack", targetCharacter.ActiveAttackWeapon.Name.Main)
                : "bare nuckle attack";
            var defenseWeaponMessage = (targetCharacter.ActiveDefenseWeapon != null)
                ? string.Format("chose {0} for defense", targetCharacter.ActiveDefenseWeapon.Name.Main)
                : "bare nuckle defense";
            return string.Format("{0} {1}", attackWeaponMessage, defenseWeaponMessage);

        }

        public ObservableCollection<Dialogue> Dialogues
        {
            get { return _dialogues; }
            set
            {
                if(value!=_dialogues)
                {
                    _dialogues = value;
                    RaisePropertyChanged("Dialogues");
                }
            }
        }


        public ObservableCollection<ActionWithTargetModel> Actions
        {
            get { return _actions; }
            set
            {
                if(value!=_actions)
                {
                    _actions = value;
                    RaisePropertyChanged("Actions");                    
                }
            }
        }

        public void ExecuteAction(TwoCharactersAction twoCharactersAction)
        {
            Action = twoCharactersAction;
            RecordActionResult(Action.Perform());
        }
    }
}
