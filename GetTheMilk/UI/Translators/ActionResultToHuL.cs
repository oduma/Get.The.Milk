using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;

namespace GetTheMilk.UI.Translators
{
    public class ActionResultToHuL
    {
        public string TranslateActionResult(ActionResult actionResult,IPlayer active)
        {
            if(active==null)
                return GameSettings.TranslatorErrorMessage;
            if(!(actionResult.ForAction is MovementAction))
                return GameSettings.TranslatorErrorMessage;
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

        public string TranslateMovementExtraData(MovementActionExtraData extraData,IPlayer active,ILevel level)
        {
            return string.Join("\r\n",
                        extraData.ObjectsInRange.OrderBy(o => o.CellNumber).Select(
                            o =>
                            string.Format(Game.CreateGameInstance().MovementExtraDataTemplate.MessageForObjectsInRange,
                                          level.Maps.FirstOrDefault(m=>m.Number==active.MapNumber)
                                          .Cells.FirstOrDefault(c => c.Number == active.CellNumber).GetDirectionToCell(o.CellNumber),
                                          ((IObjectHumanInterface) o).ApproachingMessage)));
        }

        private string FormatList(IEnumerable<IPositionableObject> objectsBlocking)
        {
            if (objectsBlocking == null || !objectsBlocking.Any())
                return "";
            string result = objectsBlocking.Aggregate(string.Empty,
                                                      (current, objectBlocking) =>
                                                      string.Format("{0}, {1}", current, objectBlocking.Name.Narrator));
            if (result.StartsWith(","))
                result = result.Substring(2, result.Length - 2);
            var andReplacementPosition = result.LastIndexOf(",");
            result = string.Format("{0} and {1}", result.Substring(0, andReplacementPosition),
                                   result.Substring(andReplacementPosition + 2, result.Length - andReplacementPosition - 2));
            return result;
        }
    }
}
