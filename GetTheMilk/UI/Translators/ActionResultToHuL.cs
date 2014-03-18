using System;
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
        public string TranslateMovementResult(ActionResult actionResult,IPlayer active)
        {
            var gameSettings = GameSettings.GetInstance();

            if(active==null)
                return gameSettings.TranslatorErrorMessage;
            if(!(actionResult.ForAction is MovementAction))
                return gameSettings.TranslatorErrorMessage;
            var messagesFor = gameSettings.MessagesFor.Where(m => m.ResultType == actionResult.ResultType);
            if (!messagesFor.Any())
                return gameSettings.TranslatorErrorMessage;
            var message = messagesFor.SelectMany(m => m.Messages, (m, f) => f).FirstOrDefault(o => o.Id == actionResult.ForAction.Name.Infinitive);
            if (message == null)
                return gameSettings.TranslatorErrorMessage;
            return string.Format(message.Value,
                          Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(active.Name.Narrator.ToLower()),
                          actionResult.ForAction.Name.Past,
                          actionResult.ForAction.Name.Present,
                          ((MovementAction) actionResult.ForAction).Direction,
                          (actionResult.ForAction is EnterLevel) ? ((EnterLevel) actionResult.ForAction).LevelNo : 0,
                          ((MovementActionExtraData) actionResult.ExtraData!=null)?
                          FormatList(((MovementActionExtraData) actionResult.ExtraData).ObjectsBlocking,NarratorNaming):"",
                          ((MovementActionExtraData) actionResult.ExtraData!=null)?
                          FormatList(((MovementActionExtraData) actionResult.ExtraData).CharactersBlocking,NarratorNaming):"");

        }

        public string TranslateActionResult(ActionResult actionResult, IPlayer active,IPositionable targetObject)
        {
            var gameSettings = GameSettings.GetInstance();

            if (active == null)
                return gameSettings.TranslatorErrorMessage;
            var messagesFor = gameSettings.MessagesFor.Where(m => m.ResultType == actionResult.ResultType);
            if (!messagesFor.Any())
                return gameSettings.TranslatorErrorMessage;
            var message = messagesFor.SelectMany(m=>m.Messages,(m,f)=>f).FirstOrDefault(o=>o.Id==actionResult.ForAction.Name.Infinitive);
            if (message==null)
                return gameSettings.TranslatorErrorMessage;
            return string.Format(message.Value,
                          Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(active.Name.Narrator.ToLower()),
                          actionResult.ForAction.Name.Past,
                          actionResult.ForAction.Name.Present,
                          targetObject.Name.Narrator);

        }


        private static string NarratorNaming(IPositionable obj)
        {
            return obj.Name.Narrator;
        }

        public string TranslateMovementExtraData(MovementActionExtraData extraData,IPlayer active,Level level)
        {
            var gameSettings = GameSettings.GetInstance();
            var objectsInTheCell = string.Empty;
            if(extraData.ObjectsInCell !=null && extraData.ObjectsInCell.Any())
                objectsInTheCell=string.Format(gameSettings.MovementExtraDataTemplate.MessageForObjectsInCell +"\r\n",
                                                 FormatList(extraData.ObjectsInCell, CloseUpMessageNaming));
            var objectsInRange =string.Empty;
            if(extraData.ObjectsInRange!=null && extraData.ObjectsInRange.Any())
                objectsInRange = string.Join("\r\n",
                        extraData.ObjectsInRange.OrderBy(o => o.CellNumber).Select(
                            o =>
                            string.Format(gameSettings.MovementExtraDataTemplate.MessageForObjectsInRange,
                                          level.Maps.FirstOrDefault(m=>m.Number==active.MapNumber)
                                          .Cells.FirstOrDefault(c => c.Number == active.CellNumber).GetDirectionToCell(o.CellNumber),
                                          ((IObjectHumanInterface) o).ApproachingMessage)));
            var charactersInRange = string.Empty;
            if (extraData.CharactersInRange != null && extraData.CharactersInRange.Any())
                charactersInRange = string.Join("\r\n",
                        extraData.CharactersInRange.OrderBy(o => o.CellNumber).Select(
                            o =>
                            string.Format(gameSettings.MovementExtraDataTemplate.MessageForObjectsInRange,
                                          level.Maps.FirstOrDefault(m => m.Number == active.MapNumber)
                                          .Cells.FirstOrDefault(c => c.Number == active.CellNumber).GetDirectionToCell(o.CellNumber),
                                          ((IObjectHumanInterface)o).ApproachingMessage)));
            return string.Format("{0}{1}{2}", objectsInTheCell, objectsInRange,charactersInRange);
        }

        private string CloseUpMessageNaming(IPositionable obj)
        {
            return obj is IObjectHumanInterface ? ((IObjectHumanInterface) obj).CloseUpMessage : string.Empty;
        }

        private string FormatList(IEnumerable<IPositionable> objects,Func<IPositionable,string> namingMethod)
        {
            if (objects == null || !objects.Any())
                return "";
            string result = objects.Aggregate(string.Empty,
                                                      (current, currentObject) =>
                                                      string.Format("{0}, {1}", current, namingMethod(currentObject)));
            if (result.StartsWith(","))
                result = result.Substring(2, result.Length - 2);
            var andReplacementPosition = result.LastIndexOf(",");
            if(objects.Count()>1)
                result = string.Format("{0} and {1}", result.Substring(0, andReplacementPosition),
                                   result.Substring(andReplacementPosition + 2, result.Length - andReplacementPosition - 2));
            return result;
        }
    }
}
