using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Kick:GameAction
    {
        public override bool CanPerform()
        {
            return TargetObject.AllowsIndirectAction(this, ActiveCharacter);
        }

        public Kick()
        {
            Name = new Verb {Infinitive = "To Kick", Past = "kicked", Present = "kick"};
            ActionType = ActionType.Kick;
        }

        public override ActionResult Perform()
        {
            return new ActionResult
                   {
                       ForAction = this,
                       ResultType = (CanPerform()) ? ActionResultType.Ok : ActionResultType.NotOk
                   };
        }
        public override GameAction CreateNewInstance()
        {
            return new Kick();
        }

    }
}
