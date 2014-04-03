using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Meet:TwoCharactersAction
    {
        public Meet()
        {
            Name = new Verb {Infinitive = "To Meet", Past = "meet", Present = "meet"};
            ActionType = ActionType.Meet;
            StartingAction = true;

        }
        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };

            EstablishInteractionRules();
            if (ActiveCharacter is IPlayer)
                return PerformResponseAction(ActionType);
            return new ActionResult {ForAction = this, ResultType = ActionResultType.Ok, ExtraData=GetAvailableActions()};
        }
        public override GameAction CreateNewInstance()
        {
            return new Meet();
        }

    }
}
