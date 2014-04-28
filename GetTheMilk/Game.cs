﻿using System.IO;
using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Settings;
using Newtonsoft.Json;
using GetTheMilk.Factories;

namespace GetTheMilk
{
    public class Game
    {

        [JsonIgnore]
        public Level CurrentLevel { get; set; }

        [JsonIgnore]
        public Player Player { get; set; }


        private static readonly Game Instance = new Game();

        public static Game Load(string fullPath)
        {
            var gameSettings = GameSettings.GetInstance();
            using (TextReader textReader = new StreamReader(gameSettings.CurrentReadStrategy(fullPath)))
            {

                var gamePackages = JsonConvert.DeserializeObject<GamePackages>(textReader.ReadToEnd());
                Instance.Player = Player.Load<Player>(gamePackages.PlayerPackages);
                Instance.CurrentLevel = Level.Create(gamePackages.LevelPackages);
                Instance.CurrentLevel.Player = Instance.Player;
            }
            return Instance;
        }

        private Game()
        {
        }

        public static Game CreateGameInstance()
        {
            if (Instance.Player == null)
            {
                Instance.Player = new Player();
                var factory = ObjectActionsFactory.GetFactory();

                var objAction = factory.CreateObjectAction("Player");
                Instance.Player.AllowsAction = objAction.AllowsAction;
                Instance.Player.AllowsIndirectAction = objAction.AllowsIndirectAction;

            }
            if(Instance.CurrentLevel==null)
            {
                Instance.CurrentLevel = Level.Create(0);
            }
            return Instance;
        }


        public bool ProceedToNextLevel()
        {
            CurrentLevel = Level.Create(CurrentLevel.Number + 1);
            if (CurrentLevel == null)
                return false;
            Save(string.Format("StartOfLevel{0}.gsu",CurrentLevel.Number));
            return true;
        }

        public void Save(string fileName)
        {
            LevelPackages levelPackages = CurrentLevel.PackageForSave();

            CharacterSavedPackages characterPackages=Player.Save();

            var gamePackages =
                JsonConvert.SerializeObject(new GamePackages
                                                {LevelPackages = levelPackages, PlayerPackages = characterPackages});
            GameSettings.GetInstance().CurrentWriteStrategy(gamePackages,
                                                            Path.Combine(
                                                                GameSettings.GetInstance().DefaultPaths.SaveDefaultPath,
                                                                fileName));

        }
    }
}