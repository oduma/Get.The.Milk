using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.GameLevels;
using GetTheMilk.Navigation;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.IntegrationTests
{
    [TestFixture]
    public class LevelCompletionTests
    {
        [Test]
        public void SomeLevelComplete()
        {
            RpgGameCore game = RpgGameCore.GetGameInstance();

            //take the foe out for quick completion
            game.CurrentLevel.Characters.Remove(game.CurrentLevel.Characters[1]);


            var teleport = game.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            teleport.TargetCell = 8;
            teleport.CurrentMap = game.CurrentLevel.CurrentMap;
            
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
            RpgGameCore game = RpgGameCore.GetGameInstance();

            //take the foe out for quick completion
            game.CurrentLevel.Characters.Remove(game.CurrentLevel.Characters[1]);


            var teleport = game.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            teleport.TargetCell = 8;
            teleport.CurrentMap = game.CurrentLevel.CurrentMap;

            game.Player.PerformAction(teleport);
            
            game.ProceedToNextLevel();

            var walk = game.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            walk.CurrentMap = game.CurrentLevel.CurrentMap;
            walk.Direction = Direction.South;

            Assert.AreEqual(ActionResultType.LevelCompleted,game.Player.PerformAction(walk).ResultType);
            Assert.False(game.ProceedToNextLevel());
            Assert.IsNull(game.CurrentLevel);


        }

    }
}
