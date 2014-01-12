using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class EnterLevel:MovementAction
    {
        public EnterLevel()
        {
            Name = new Verb {Infinitive = "To Enter", Past = "entered", Present = "enter"};
        }
        public override int DefaultDistance
        {
            get { return 0; }
        }

        public int LevelNo { get; set; }

    }
}
