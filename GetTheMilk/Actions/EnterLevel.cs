using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class EnterLevel:MovementAction
    {
        public EnterLevel()
        {
            Name = new Verb {Infinitive = "To Enter", Past = "entered", Present = "enter"};
            DefaultDistance = 0;
            ActionType = ActionType.EnterLevel;
        }

        public int LevelNo { get; set; }

    }
}
