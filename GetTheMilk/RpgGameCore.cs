using System.IO;
using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Common;
using GetTheMilk.Levels;
using GetTheMilk.Settings;
using GetTheMilk.Utils;
using Newtonsoft.Json;
using GetTheMilk.Factories;

namespace GetTheMilk
{
    public class RpgGameCore
    {

        [JsonIgnore]
        public Level CurrentLevel { get; set; }

        [JsonIgnore]
        public Player Player { get; set; }


        private static readonly RpgGameCore Instance = new RpgGameCore();

        public static RpgGameCore Load(string fullPath)
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

        private RpgGameCore()
        {
        }

        public static RpgGameCore GetGameInstance() 
        {
            if (Instance.Player == null)
            {
                Instance.Player = new Player();

                var objAction = ObjectActionsFactory.CreateObjectAction("Player");
                Instance.Player.AllowsTemplateAction = objAction.AllowsTemplateAction;
                Instance.Player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            }
            if(Instance.CurrentLevel==null)
            {
                Instance.CurrentLevel = Level.Create(0);
            }
            return Instance;
        }

        public static RpgGameCore CreateNewGameInstance()
        {
                Instance.Player = new Player();

                var objAction = ObjectActionsFactory.CreateObjectAction("Player");
                Instance.Player.AllowsTemplateAction = objAction.AllowsTemplateAction;
                Instance.Player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
                Instance.CurrentLevel = Level.Create(0);
            return Instance;
        }

        public bool ProceedToNextLevel()
        {
            CurrentLevel = Level.Create(CurrentLevel.Number + 1);
            Player.CellNumber = 0;
            if (CurrentLevel == null)
                return false;
            Player.EnterLevel(CurrentLevel);
            Save(string.Format("StartOfLevel{0}.gsu", CurrentLevel.Number));
            return true;
        }

        public void Save(string fileName)
        {
            ContainerNoActionsPackage levelPackages = CurrentLevel.PackageForSave();

            ContainerWithActionsPackage characterPackages=Player.Save();

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