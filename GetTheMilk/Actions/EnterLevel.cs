using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions
{
    public class EnterLevel:MovementAction
    {
        public override string Name
        {
            get { return "Enter"; }
        }

        public override int DefaultDistance
        {
            get { return 0; }
        }

        public int LevelNo { get; set; }

    }
}
