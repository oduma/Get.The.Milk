using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionExecutionRequestEventArgs:EventArgs
    {
        public GameAction GameAction { get; private set; }

        public NonCharacterObject TargetObject { get; private set; }

        public ICharacter TargetCharacter { get; private set; }
        public ActionExecutionRequestEventArgs(GameAction action,NonCharacterObject targetObject=null,ICharacter activeCharacter=null,
            ICharacter targetCharacter=null)
        {
            GameAction = action;
            TargetObject = targetObject;
            TargetCharacter = targetCharacter;
            ActiveCharacter = activeCharacter;
        }

        public ICharacter ActiveCharacter { get; private set; }
    }
}
