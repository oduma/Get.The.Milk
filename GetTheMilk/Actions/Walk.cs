using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Settings;

namespace GetTheMilk.Actions
{
    public class Walk:MovementAction
    {
        public override string Name
        {
            get { return "Walk"; }
        }

        public override int DefaultDistance
        {
            get { return GameSettings.DefaultWalkDistance; }
        }
    }
}
