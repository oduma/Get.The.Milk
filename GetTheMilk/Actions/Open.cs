using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Open: ObjectUseOnObjectAction
    {
        public Open()
        {
            Name = new Verb {Infinitive = "To Open", Past = "opened", Present = "open"};
            ActionType = ActionType.Open;
            DestroyActiveObject = true;
            DestroyTargetObject = true;
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
            return new Open();
        }

    }
}
