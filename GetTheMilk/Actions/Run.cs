using System;
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
            try
            {
                DefaultDistance = GameSettings.GetInstance().DefaultRunDistance;
            }
            catch {}
            ActionType = ActionType.Run;
            StartingAction = true;
        }

        public override GameAction CreateNewInstance()
        {
            return new Run();
        }

    }
}
