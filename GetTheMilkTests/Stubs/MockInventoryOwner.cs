using GetTheMilk.BaseCommon;

namespace GetTheMilkTests.Stubs
{
    public class MockInventoryOwner:IInventoryOwner
    {
        public Noun Name { get; set; }

        public MockInventoryOwner()
        {
            Name = new Noun {Main = "I own you", Narrator = "I own you"};
        }
    }
}
