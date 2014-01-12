using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;

namespace GetTheMilk.UI.Translators
{
    public class ActionResultToHuL
    {
        public string TranslateMovementResult(ActionResult actionResult,IPlayer active)
        {
            var gameInstance = Game.CreateGameInstance();
            var messageFor = gameInstance.MessagesFor.FirstOrDefault(m => m.ResultType == actionResult.ResultType);
            if (messageFor == null)
                return GameSettings.TranslatorErrorMessage;
            var message = messageFor.Messages.FirstOrDefault(m => m.Id == actionResult.ForAction.Name.Infinitive);
            if(message==null)
                return GameSettings.TranslatorErrorMessage;
            return string.Format(message.Value,
                          Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(active.Name.Narrator.ToLower()),
                          actionResult.ForAction.Name.Past,
                          actionResult.ForAction.Name.Present,
                          ((MovementAction) actionResult.ForAction).Direction,
                          (actionResult.ForAction is EnterLevel) ? ((EnterLevel) actionResult.ForAction).LevelNo : 0,
                          ((MovementActionExtraData) actionResult.ExtraData!=null)?
                          FormatList(((MovementActionExtraData) actionResult.ExtraData).ObjectsBlocking):"",
                          ((MovementActionExtraData) actionResult.ExtraData!=null)?
                          FormatList(((MovementActionExtraData) actionResult.ExtraData).ObjectsBlocking):"");

        }

        private string FormatList(IEnumerable<IPositionableObject> objectsBlocking)
        {
            if (objectsBlocking == null || !objectsBlocking.Any())
                return "";
            string result = objectsBlocking.Aggregate(string.Empty,
                                                      (current, objectBlocking) =>
                                                      string.Format("{0}, {1}", current, objectBlocking.Name.Narrator));
            if (result.StartsWith(","))
                result = result.Substring(1, result.Length - 1);
            var andReplacementPosition = result.LastIndexOf(",");
            result = string.Format("{0} and {1}", result.Substring(0, andReplacementPosition),
                                   result.Substring(andReplacementPosition, result.Length - andReplacementPosition));
            return result;
        }
    }
}
