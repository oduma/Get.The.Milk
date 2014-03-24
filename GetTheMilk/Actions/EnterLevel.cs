using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class EnterLevel:Teleport
    {
        public int LevelNo { get; set; }
        public EnterLevel()
        {
            Name = new Verb {Infinitive = "To Enter", Past = "entered", Present = "enter"};
            DefaultDistance = 0;
            ActionType = ActionType.EnterLevel;
        }
        public override ActionResult Perform()
        {
            var result = base.Perform();
            result.ForAction = this;
            return result;
        }

        public override GameAction CreateNewInstance()
        {
            return new EnterLevel();
        }

    }
}
