using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class TwoCharactersViewModel : ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        public event EventHandler<PlayerStatsUpdateRequestEventArgs> PlayerStatsUpdateRequest;

        public event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;

        private IActionTemplatePerformer _action;
        private int _passiveHealth;
        private ObservableCollection<ActionWithTargetModel> _actions;
        private ObservableCollection<Dialogue> _dialogues;


        public IActionTemplatePerformer Action
        {
            get { return _action; }
            set
            {
                if (value != _action)
                {
                    if (_action != null)
                        ((TwoCharactersActionTemplatePerformer)_action).FeedbackFromOriginalAction -= ActionFeedbackFromOriginalAction;
                    _action = value;
                    ((TwoCharactersActionTemplatePerformer)_action).FeedbackFromOriginalAction += ActionFeedbackFromOriginalAction;
                }
            }
        }

        private void ActionFeedbackFromOriginalAction(object sender, FeedbackEventArgs e)
        {
            if (e.ActionResult.ForAction.CurrentPerformer.GetType() == typeof(CommunicateActionPerformer) && e.ActionResult.ForAction.ActiveCharacter.ObjectTypeId == "Player")
                return;
            Dialogues.Add(new Dialogue
                              {
                                  Who = e.ActionResult.ForAction.ActiveCharacter.Name.Narrator,
                                  What = e.ActionResult.ToString()
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
                ((TwoCharactersActionTemplatePerformer)Action).FeedbackFromOriginalAction -= ActionFeedbackFromOriginalAction;
                ((TwoCharactersActionTemplatePerformer)Action).FeedbackFromOriginalAction += ActionFeedbackFromOriginalAction;
                ((TwoCharactersActionTemplatePerformer)Action).FeedbackFromSubAction -= TwoCharactersViewModel_FeedbackFromSubAction;
                ((TwoCharactersActionTemplatePerformer)Action).FeedbackFromSubAction += TwoCharactersViewModel_FeedbackFromSubAction;
                if (obj.Action.CurrentPerformer.GetType() == typeof(CommunicateActionPerformer))
            {
                Dialogues.Add(new Dialogue { Who = obj.Action.ActiveCharacter.Name.Narrator, What = ((TwoCharactersActionTemplate)obj.Action).Message });
            }
            if (obj.Action.Name.UniqueId == "ExposeInventory")
            {
                if(ActionExecutionRequest!=null)
                    ActionExecutionRequest(this,new ActionExecutionRequestEventArgs(obj.Action));
            }
            else
            {
                RecordActionResult(obj.Action.ActiveCharacter.PerformAction(obj.Action));
            }

        }

        void TwoCharactersViewModel_FeedbackFromSubAction(object sender, FeedbackEventArgs e)
        {
            if (FeedbackFromSubAction != null)
                FeedbackFromSubAction(this, e);
        }

        private void RecordActionResult(PerformActionResult actionResult)
        {
            if (actionResult.ForAction.CurrentPerformer.GetType() == typeof(CommunicateActionPerformer))
            {
                Dialogues.Add(new Dialogue
                              {
                                  Who = actionResult.ForAction.ActiveCharacter.Name.Main,
                                  What = ((TwoCharactersActionTemplate) actionResult.ForAction).Message
                              });
            }
            if (actionResult.ForAction.Name.UniqueId == "InitiateHostilities")
            {
                Dialogues.Add(new Dialogue
                              {
                                  Who = actionResult.ForAction.ActiveCharacter.Name.Main,
                                  What = GetOpponentActiveWeapons(actionResult.ForAction.ActiveCharacter)
                              });
            }
            if (actionResult.ForAction.CurrentPerformer.GetType() == typeof(AttackActionPerformer))
            {
                //if(actionResult.ResultType==ActionResultType.Win || actionResult.ResultType== ActionResultType.Lost)
                //{
                //    if(PlayerStatsUpdateRequest!=null)
                //        PlayerStatsUpdateRequest(this,new PlayerStatsUpdateRequestEventArgs(actionResult));
                //    return;
                //}

                Dialogues.Add(new Dialogue
                                  {
                                      Who = actionResult.ForAction.ActiveCharacter.Name.Narrator,
                                      What = actionResult.ToString()
                                  });
                if(!(actionResult.ForAction.ActiveCharacter is IPlayer))
                {
                    PassiveHealth = actionResult.ForAction.ActiveCharacter.Health;
                    if (PlayerStatsUpdateRequest != null)
                        PlayerStatsUpdateRequest(this,
                                                 new PlayerStatsUpdateRequestEventArgs(actionResult));
                }

            }
            if (actionResult.ForAction.Name.UniqueId == "ExposeInventory")
            {
                if (ActionExecutionRequest != null)
                    ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(actionResult.ForAction));
            }
            else if(actionResult.ForAction.Name.UniqueId=="AcceptQuit")
            {
                if (PlayerStatsUpdateRequest != null)
                    PlayerStatsUpdateRequest(this,
                                             new PlayerStatsUpdateRequestEventArgs(actionResult));
            }
            else
            {
            
            Actions = new ObservableCollection<ActionWithTargetModel>();
            foreach (var availableAction in (List<BaseActionTemplate>) actionResult.ExtraData)
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

        public void ExecuteAction(TwoCharactersActionTemplate twoCharactersAction)
        {
            Action = twoCharactersAction.CurrentPerformer;
            RecordActionResult(Action.Perform(twoCharactersAction));
        }
    }
}
