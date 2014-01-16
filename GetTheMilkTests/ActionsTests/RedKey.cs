using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace GetTheMilkTests.ActionsTests
{
    public class RedKey : AnyKey
    {
        public RedKey()
        {
            Name = new Noun {Main = "Red Key", Narrator = "Red Key"};
        }

        public override string ApproachingMessage { get; set; }
        public override string CloseUpMessage { get; set; }
    }
}
