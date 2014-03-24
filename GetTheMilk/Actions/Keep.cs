using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Keep:ObjectTransferAction
    {

        public Keep()
        {
            Name = new Verb {Infinitive = "To Keep", Past = "kept", Present = "keep"};
            ActionType = ActionType.Keep;
        }
        public override ActionResult Perform()
        {
            var result = base.Perform();
            result.ForAction = this;
            return result;
        }

        public override GameAction CreateNewInstance()
        {
            return new Keep();
        }


    }
}
