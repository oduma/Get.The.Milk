using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Defuse:ObjectUseOnObjectAction
    {
        public Defuse()
        {
            Name = new Verb {Infinitive = "To Defuse", Past = "defused", Present = "defuse"};
            ActionType = ActionType.Defuse;
            DestroyActiveObject = true;
            DestroyTargetObject = true;
            StartingAction = true;
            ChanceOfSuccess = ChanceOfSuccess.VeryBig;
            PercentOfHealthFailurePenalty = 50;

        }

        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};
            var result=base.Perform();
            result.ForAction = this;
            return result;
        }

        public override GameAction CreateNewInstance()
        {
            return new Defuse();
        }

    }
}
