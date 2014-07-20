using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
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

            var teleport = new MovementActionTemplate
                               {
                                   ActiveCharacter = game.Player,
                                   CurrentMap = game.CurrentLevel.CurrentMap,
                                   TargetCell = 8
                               };
            var result = game.Player.PerformAction(teleport);
            Assert.IsNotNull(result);

            Assert.AreEqual(ActionResultType.LevelCompleted, result.ResultType);
            Assert.IsNull(result.ExtraData);

            Assert.True(game.ProceedToNextLevel());

            Assert.IsNotNull(game.CurrentLevel);

            
        }
        [Test]
        public void FinalLevelComplete()
        {
            Game game = Game.CreateGameInstance();

            game.CurrentLevel.Characters.Remove(game.CurrentLevel.Characters[1]);

            var teleport = new MovementActionTemplate
            {
                ActiveCharacter = game.Player,
                CurrentMap = game.CurrentLevel.CurrentMap,
                TargetCell = 8
            };
            game.Player.PerformAction(teleport);
            game.ProceedToNextLevel();

            var walk = new MovementActionTemplate
            {
                Name = new Verb { PerformerId = "Walk" },
                ActiveCharacter = game.Player,
                CurrentMap = game.CurrentLevel.CurrentMap,
                Direction = Direction.South
            };
            Assert.AreEqual(ActionResultType.LevelCompleted,game.Player.PerformAction(walk).ResultType);
            Assert.False(game.ProceedToNextLevel());
            Assert.IsNull(game.CurrentLevel);


        }

    }
}
