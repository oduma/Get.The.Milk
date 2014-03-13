using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Settings;

namespace GetTheMilk.Actions
{
    public class Walk:MovementAction
    {
        public Walk()
        {
            Name = new Verb {Infinitive = "To Walk", Past = "walked", Present = "walk"};
            DefaultDistance = GameSettings.GetInstance().DefaultWalkDistance;
            ActionType = ActionType.Walk;
        }
    }
}
