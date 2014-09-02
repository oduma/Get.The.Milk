using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Settings;
using System.Linq;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class PerformActionResult
    {
        public ActionResultType ResultType { get; set; }

        public object ExtraData { get; set; }

        public BaseActionTemplate ForAction { get; set; }

        public override string ToString()
        {
            var gameSettings = GameSettings.GetInstance();

            var messageForAction = gameSettings.ActionTemplateMessages.FirstOrDefault(m => m.Id == ForAction.Category);
            if (messageForAction==null)
                return gameSettings.TranslatorErrorMessage;
            var message = messageForAction.ActionResultMessages.FirstOrDefault(m=>m.ResultType==ResultType.ToString());
            if (message == null)
                return gameSettings.TranslatorErrorMessage;
            if ((ForAction is MovementActionTemplate))
                return string.Format(message.Value, Translate()).Trim();
            return string.Format(message.Value, ForAction.Translate()).Trim();
        }

        private object[] Translate()
        {
            var result = ForAction.Translate();
            result[11] = (ExtraData!=null && ResultType==ActionResultType.Blocked)?
                " " +(ResultType.ToString() + " by " +((MovementActionTemplateExtraData)ExtraData).GetBlockers()):
                ((ResultType != ActionResultType.Ok) ? " (" + ResultType.ToString()+")" : string.Empty);
            result[12]=(ExtraData!=null)?((MovementActionTemplateExtraData)ExtraData).GetPositioningInformation():string.Empty;
            return result;
        }
    }
}
