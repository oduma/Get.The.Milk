using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Explode: ObjectUseOnObjectAction
    {
        public Explode()
        {
            Name = new Verb {Infinitive = "To Explode", Past = "exploded", Present = "explode"};
            ActionType = ActionType.Explode;
            DestroyActiveObject = true;
            DestroyTargetObject = true;
            StartingAction = true;

        }

        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};
            var result=base.Perform();
            if (result.ResultType == ActionResultType.Ok)
                ActiveCharacter.Health /= 2;
            result.ForAction = this;
            return result;
        }

        public override GameAction CreateNewInstance()
        {
            return new Explode();
        }

    }
}
