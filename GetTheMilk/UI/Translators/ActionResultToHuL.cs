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
        public string TranslateMovementResult(ActionResult actionResult)
        {
            var gameSettings = GameSettings.GetInstance();

            if(actionResult.ForAction.ActiveCharacter==null)
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
                          Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(actionResult.ForAction.ActiveCharacter.Name.Narrator.ToLower()),
                          actionResult.ForAction.Name.Past,
                          actionResult.ForAction.Name.Present,
                          ((MovementAction) actionResult.ForAction).Direction,
                          (actionResult.ForAction is EnterLevel) ? ((EnterLevel) actionResult.ForAction).CurrentMap.Parent.Number : 0,
                          ((MovementActionExtraData) actionResult.ExtraData!=null)?
                          FormatList(((MovementActionExtraData) actionResult.ExtraData).ObjectsBlocking,NarratorNaming):"",
                          ((MovementActionExtraData) actionResult.ExtraData!=null)?
                          FormatList(((MovementActionExtraData) actionResult.ExtraData).CharactersBlocking,NarratorNaming):"");

        }

        public string TranslateActionResult(ActionResult actionResult)
        {
            var gameSettings = GameSettings.GetInstance();

            if (actionResult.ForAction.ActiveCharacter == null)
                return gameSettings.TranslatorErrorMessage;
            var messagesFor = gameSettings.MessagesFor.Where(m => m.ResultType == actionResult.ResultType);
            if (!messagesFor.Any())
                return gameSettings.TranslatorErrorMessage;
            var message = messagesFor.SelectMany(m=>m.Messages,(m,f)=>f).FirstOrDefault(o=>o.Id==actionResult.ForAction.Name.Infinitive);
            if (message==null)
                return gameSettings.TranslatorErrorMessage;
            return string.Format(message.Value,
                          Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(
                          actionResult.ForAction.ActiveCharacter.Name.Narrator.ToLower()),
                          actionResult.ForAction.Name.Past,
                          actionResult.ForAction.Name.Present,
                          (actionResult.ForAction.TargetObject==null)?string.Empty:actionResult.ForAction.TargetObject.Name.Narrator,
                          (actionResult.ForAction.TargetCharacter == null) ? string.Empty : actionResult.ForAction.TargetCharacter.Name.Narrator,
                          (actionResult.ForAction.ActiveObject == null) ? string.Empty : actionResult.ForAction.ActiveObject.Name.Narrator);

        }


        private static string NarratorNaming(IPositionable obj)
        {
            return obj.Name.Narrator;
        }

        public string TranslateMovementExtraData(MovementActionExtraData extraData,IPlayer active,Level level)
        {
            var gameSettings = GameSettings.GetInstance();
            var objectsReachable = string.Empty;
            var allObjectsReachable = (extraData.ObjectsInCell == null)
                ? (new List<NonCharacterObject> {}).Union(extraData.ObjectsInRange)
                : extraData.ObjectsInCell.Union(extraData.ObjectsInRange).ToList();
            if(allObjectsReachable.Any())
                objectsReachable=string.Join("\r\n",
	                        allObjectsReachable.OrderBy(o => o.CellNumber).Select(
	                            o =>
	                            string.Format(gameSettings.MovementExtraDataTemplate.MessageForObjectsInRange,
	                                          level.CurrentMap
	                                          .Cells[active.CellNumber].GetDirectionToCell(o.CellNumber),
	                                          ((IObjectHumanInterface) o).CloseUpMessage)));

            var charactersInRange = string.Empty;
            if (extraData.CharactersInRange != null && extraData.CharactersInRange.Any())
                charactersInRange = string.Join("\r\n",
                        extraData.CharactersInRange.OrderBy(o => o.CellNumber).Select(
                            o =>
                            string.Format(gameSettings.MovementExtraDataTemplate.MessageForObjectsInRange,
                                          level.CurrentMap.Cells[active.CellNumber].GetDirectionToCell(o.CellNumber),
                                          ((IObjectHumanInterface)o).CloseUpMessage)));
            return string.Format("{0}{1}", objectsReachable, charactersInRange);
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
