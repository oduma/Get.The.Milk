using System.Collections.Generic;
using System.IO;
using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Settings;
using NUnit.Framework;
using Newtonsoft.Json;
using GetTheMilk.Navigation;

namespace GetTheMilk.NewActions.Tests.SaveLoadTests
{
    [TestFixture]
    public class GameSaveLoadTests
    {
        [Test]
        public void LoadGameSettings()
        {
            var gameSettings = GameSettings.GetInstance();
            Assert.AreEqual(gameSettings.DefaultGameName, "Get the milk");
            Assert.AreEqual(gameSettings.Description,"Some description here.");
            Assert.AreEqual(8,gameSettings.ActionTemplateMessages.Count);
        }


        [Test]
        public void LoadANewGame()
        {
            RpgGameCore game = RpgGameCore.GetGameInstance();
            Assert.IsNotNull(game);
            Assert.IsNotNull(game.Player);
            Assert.IsNotNull(game.CurrentLevel);
           
        }

        [Test]
        public void SaveAndLoadGameToUncompressedFile()
        {
            RpgGameCore game = RpgGameCore.CreateNewGameInstance();
            Assert.IsNotNull(game);
            Assert.IsNotNull(game.Player);
            Assert.IsNotNull(game.CurrentLevel);
            game.Player.CellNumber = 0;
            game.Player.EnterLevel(game.CurrentLevel);
            game.Player.SetPlayerName("my own name");
            var walk = game.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            walk.Direction = Direction.South;
            walk.CurrentMap = game.CurrentLevel.CurrentMap;
            game.Player.PerformAction(walk);
            game.Save("gamesavedtest.gsu");
            Assert.True(File.Exists(@"Saved\gamesavedtest.gsu"));

            var gameLoaded = RpgGameCore.Load(@"Saved\gamesavedtest.gsu");
            Assert.IsNotNull(gameLoaded);
            Assert.IsNotNull(gameLoaded.Player);
            Assert.IsNotNull(gameLoaded.CurrentLevel);
            Assert.AreEqual(3,gameLoaded.Player.CellNumber); 
            Assert.AreEqual("my own name",gameLoaded.Player.Name.Main);

        }
    }
}
