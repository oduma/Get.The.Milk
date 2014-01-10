using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions
{
    public class Teleport:MovementAction
    {
        public override string Name
        {
            get { return "Teleport"; }
        }

        public override int DefaultDistance
        {
            get { return 0; }
        }
    }
}
