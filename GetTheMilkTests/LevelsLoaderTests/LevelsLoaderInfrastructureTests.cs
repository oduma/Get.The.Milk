using GetTheMilk.Factories;
using NUnit.Framework;

namespace GetTheMilkTests.LevelsLoaderTests
{
    [TestFixture]
    public class LevelsLoaderInfrastructureTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForLevels), "TestCases")]
        public int CreateLevelOk(int levelNumber,int expectedObjects, int expectedCharacters)
        {
            var level = (new LevelsFactory()).CreateLevel(levelNumber);
            Assert.IsNotNull(level);
            Assert.AreEqual(levelNumber,level.Number);
            Assert.AreEqual(expectedObjects,level.PositionableObjects.Objects.Count);
            Assert.AreEqual(expectedCharacters,level.Characters.Objects.Count);
            return level.Maps[0].Cells.Length;
        }

        [Test, TestCaseSource(typeof(DataGeneratorForLevels), "LoadTestCases")]
        public int LoadLevelOk(int levelNumber, string savedFileName)
        {
            var level = (new LevelsFactory()).CreateLevel(levelNumber);
            level.Load(savedFileName);
            Assert.IsNotNull(level);
            Assert.AreEqual(levelNumber, level.Number);
            return level.PositionableObjects.Objects.Count;
        }

    }
}
