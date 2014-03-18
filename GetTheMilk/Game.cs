using System.IO;
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

        public static void Load(string fullPath)
        {
            var gameSettings = GameSettings.GetInstance();
            using (TextReader textReader = new StreamReader(gameSettings.CurrentReadStrategy(fullPath)))
            {

                var gamePackages = JsonConvert.DeserializeObject<GamePackages>(textReader.ReadToEnd());
                Instance.Player = Player.Load<Player>(gamePackages.PlayerPackages);
                Instance.CurrentLevel = Level.Create(gamePackages.LevelPackages);
            }
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
            return Instance;
        }


        public void ProceedToNextLevel()
        {
            CurrentLevel = Level.Create(CurrentLevel.Number + 1);
        }

        public void Save(string fileName)
        {
            LevelPackages levelPackages = CurrentLevel.Save();

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