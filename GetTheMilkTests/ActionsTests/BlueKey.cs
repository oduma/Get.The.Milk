using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace GetTheMilkTests.ActionsTests
{
    public class BlueKey:AnyKey
    {
        public BlueKey()
        {
            Name = new Noun {Main = "BlueKey"};
        }

        public override string ApproachingMessage { get; set; }
        public override string CloseUpMessage { get; set; }
    }
}
