using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;

namespace GetTheMilk.UI.Translators
{
    public static class ActionToHuL
    {
        public static string TranslateAction(GameAction action)
        {
            var gameSettings = GameSettings.GetInstance();

            if (action.ActiveCharacter == null)
                return gameSettings.TranslatorErrorMessage;
            var message = gameSettings.ActionTypeMessages.FirstOrDefault(m => m.Id == action.ActionType.ToString());
            if (message == null)
                return gameSettings.TranslatorErrorMessage;
            return string.Format(message.Value,
                          action.Name.Present,
                          (action.TargetObject == null) ? string.Empty : action.TargetObject.Name.Narrator,
                          (action.TargetCharacter == null) ? string.Empty : action.TargetCharacter.Name.Narrator,
                          (action.ActiveObject == null) ? string.Empty : action.ActiveObject.Name.Narrator,
                          (action.ActionType==ActionType.Communicate)?((Communicate)action).Message:string.Empty,
                          (action.TargetObject is ITransactionalObject)?((ITransactionalObject)action.TargetObject).BuyPrice.ToString():string.Empty);


        }
    }
}
