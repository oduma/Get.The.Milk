using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Settings;

namespace GetTheMilk.Actions
{
    public class Run:MovementAction
    {
        public Run()
        {
            Name = new Verb {Infinitive = "To Run", Past = "ran", Present = "run"};
            DefaultDistance = GameSettings.GetInstance().DefaultRunDistance;
            ActionType = ActionType.Run;
            StartingAction = true;
        }

        public override GameAction CreateNewInstance()
        {
            return new Run();
        }

    }
}
