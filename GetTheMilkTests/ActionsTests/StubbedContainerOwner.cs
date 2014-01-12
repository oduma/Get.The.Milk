using GetTheMilk.BaseCommon;

namespace GetTheMilkTests.ActionsTests
{
    public class StubbedContainerOwner:IInventoryOwner
    {
        public StubbedContainerOwner()
        {
            Name = new Noun {Main = "Some Name"};
        }

        public Noun Name { get; private set; }
    }
}
