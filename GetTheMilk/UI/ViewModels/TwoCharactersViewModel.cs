using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class TwoCharactersViewModel : ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        private GameAction _action;

        public RelayCommand<ActionWithTargetModel> PerformAction { get; private set; }

        public string ActiveVPassive
        {
            get { return _action.ActiveCharacter.Name.Main + " - " + _action.TargetCharacter.Name.Main; }
        }

        public TwoCharactersViewModel(GameAction action)
        {
            _action = action;
            Dialogues = new ObservableCollection<Dialogue>();
            PerformAction = new RelayCommand<ActionWithTargetModel>(PerformActionCommand);

            RecordActionResult(_action.Perform());
        }

        private void PerformActionCommand(ActionWithTargetModel obj)
        {
            if (obj.Action.ActionType == ActionType.Communicate)
            {
                Dialogues.Add(new Dialogue { Who = obj.Action.ActiveCharacter.Name.Narrator, What = ((Communicate)obj.Action).Message });
            }
            if(obj.Action.FinishTheInteractionOnExecution)
            {
                if(ActionExecutionRequest!=null)
                    ActionExecutionRequest(this,new ActionExecutionRequestEventArgs(obj.Action));
            }
            if (obj.Action.ActionType == ActionType.ExposeInventory)
            {
                if (ActionExecutionRequest != null)
                    ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(obj.Action));
            }
            else
                RecordActionResult(obj.Action.Perform());

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
            if (actionResult.ForAction.ActionType == ActionType.ExposeInventory)
            {
                if (ActionExecutionRequest != null)
                    ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(actionResult.ForAction));
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

        public ObservableCollection<Dialogue> Dialogues { get; set; }


        public ObservableCollection<ActionWithTargetModel> Actions { get; set; }

    }
}
