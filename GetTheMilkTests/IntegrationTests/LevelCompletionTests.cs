using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Navigation;
using NUnit.Framework;

namespace GetTheMilkTests.IntegrationTests
{
    [TestFixture]
    public class LevelCompletionTests
    {
        [Test]
        public void SomeLevelComplete()
        {
            Game game = Game.CreateGameInstance();

            game.CurrentLevel.Characters.Remove(game.CurrentLevel.Characters[1]);

            var teleport = new Teleport
                               {
                                   ActiveCharacter = game.Player,
                                   CurrentMap = game.CurrentLevel.CurrentMap,
                                   TargetCell = 8
                               };
            var result = teleport.Perform();
            Assert.IsNotNull(result);

            Assert.AreEqual(ActionResultType.LevelCompleted, result.ResultType);
            Assert.IsNull(result.ExtraData);

            Assert.True(game.ProceedToNextLevel());

            Assert.IsNotNull(game.CurrentLevel);

            Assert.IsNotNull(game.CurrentLevel.Player);

            
        }
        [Test]
        public void FinalLevelComplete()
        {
            Game game = Game.CreateGameInstance();

            game.CurrentLevel.Characters.Remove(game.CurrentLevel.Characters[1]);

            var teleport = new Teleport
            {
                ActiveCharacter = game.Player,
                CurrentMap = game.CurrentLevel.CurrentMap,
                TargetCell = 8
            };
            teleport.Perform();
            game.ProceedToNextLevel();

            var walk = new Walk
                           {
                               ActiveCharacter = game.Player,
                               CurrentMap = game.CurrentLevel.CurrentMap,
                               Direction = Direction.South
                           };
            Assert.AreEqual(ActionResultType.LevelCompleted,walk.Perform().ResultType);
            Assert.False(game.ProceedToNextLevel());
            Assert.IsNull(game.CurrentLevel);


        }

    }
}
