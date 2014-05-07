using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Settings;

namespace GetTheMilk.Actions
{
    public class Walk:MovementAction
    {
        public override GameAction CreateNewInstance()
        {
            return new Walk();
        }

        public Walk()
        {
            Name = new Verb {Infinitive = "To Walk", Past = "walked", Present = "walk"};
            try
            {
                DefaultDistance = GameSettings.GetInstance().DefaultWalkDistance;
            }
            catch{}
            ActionType = ActionType.Walk;
            StartingAction = true;

        }
    }
}
