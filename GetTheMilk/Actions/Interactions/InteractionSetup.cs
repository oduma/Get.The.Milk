using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.Interactions
{
    public class InteractionSetup
    {
        public ICharacter Active { get; set; }

        public ICharacter Passive { get; set; }

        public ActionResult Start(GameAction action)
        {
            var result = new ActionResult();
            while (action != null && (result.ResultType == ActionResultType.Ok
                                      || result.ResultType == ActionResultType.RequestQuit))
            {
                var tempResult = Active.TryPerformAction(action, Passive);
                if (tempResult.ResultType == ActionResultType.Ok
                    || tempResult.ResultType == ActionResultType.RequestQuit)
                {
                    action = Passive.TryContinueInteraction(action, Active);
                    var temp = Passive;
                    Passive = Active;
                    Active = temp;
                }
                result = tempResult;
            }
            return EvaluateInteraction(result);
        }



        public ActionResult EvaluateInteraction(ActionResult result)
        {
            switch (result.ResultType)
            {
                case ActionResultType.Win:
                    if (Passive is IPlayer)
                    {
                        result.ResultType = ActionResultType.GameOver;
                        return result;
                    }
                    return Active.PileageCharacter(Passive,ActionResultType.Win);
                case ActionResultType.QuitAccepted:
                    var response=Active.PileageCharacter(Passive, ActionResultType.QuitAccepted);
                    response.ResultType = ActionResultType.Lost;
                    return response;
                case ActionResultType.Lost:
                    if (Active is IPlayer)
                    {
                        result.ResultType = ActionResultType.GameOver;
                        return result;
                    }
                    return Passive.PileageCharacter(Active,ActionResultType.Win);
                default:
                    return result;
            }
        }

    }
}
