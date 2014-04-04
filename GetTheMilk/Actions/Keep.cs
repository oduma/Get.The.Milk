using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Keep:OneObjectAction
    {

        public Keep()
        {
            Name = new Verb {Infinitive = "To Keep", Past = "kept", Present = "keep"};
            ActionType = ActionType.Keep;
            StartingAction = true;

        }
        public override ActionResult Perform()
        {
            if (CanPerform())
            {
                var addedOk = ActiveCharacter.Inventory.Add(TargetObject);
                return new ActionResult
                {
                    ForAction = this,
                    ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk,
                };
            }
            return new ActionResult
            {
                ForAction = this,
                ResultType = ActionResultType.NotOk
            };

        }

        public override GameAction CreateNewInstance()
        {
            return new Keep();
        }


    }
}
