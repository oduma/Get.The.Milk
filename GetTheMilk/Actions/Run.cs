using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Settings;

namespace GetTheMilk.Actions
{
    public class Run:MovementAction
    {
        public override string Name
        {
            get { return "Run"; }
        }

        public override int DefaultDistance
        {
            get { return GameSettings.DefaultRunDistance; }
        }
    }
}
