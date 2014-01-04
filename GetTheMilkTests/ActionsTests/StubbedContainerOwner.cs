using GetTheMilk.BaseCommon;

namespace GetTheMilkTests.ActionsTests
{
    public class StubbedContainerOwner:IInventoryOwner
    {
        public StubbedContainerOwner()
        {
            Name = "Some Name";
        }
        public string Name { get; private set; }
    }
}
