using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionExecutionRequestEventArgs:EventArgs
    {
        public GameAction GameAction { get; private set; }
        public ActionExecutionRequestEventArgs(GameAction action,NonCharacterObject targetObject=null,ICharacter activeCharacter=null,
            ICharacter targetCharacter=null)
        {
            GameAction = action;
            action.TargetObject = targetObject;
            action.TargetCharacter = targetCharacter;
            action.ActiveCharacter = activeCharacter;
        }
    }
}
