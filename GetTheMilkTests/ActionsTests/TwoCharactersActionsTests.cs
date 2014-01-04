using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class TwoCharactersActionsTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCases2CM")]
        public void MeetACharacter()
        {
            ;
        }
    }
}
