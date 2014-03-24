using System.Collections.Generic;
using System.IO;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Settings;
using GetTheMilk.Utils;
using Newtonsoft.Json;

namespace GetTheMilk.Levels
{
    public class Level:IInventoryOwner
    {
        public Map CurrentMap { get; set; }
        public int Number { get; set; }

        [JsonIgnore]
        public IPlayer Player { get; set; }
        public  string Story { get; set; }

        public LevelPackages Save()
        {
            var levelPackages = new LevelPackages
                       {
                           LevelCore = JsonConvert.SerializeObject(this),
                           //LevelObjects = JsonConvert.SerializeObject(Objects),
                           LevelCharacters= new List<CharacterSavedPackages>()
                       };
            //foreach (var character in Characters)
            //    levelPackages.LevelCharacters.Add(character.Save());
            return levelPackages;
        }

        public static Level Create(int levelNo)
        {
            var gameSettings = GameSettings.GetInstance();
            using(TextReader tr = new StreamReader(gameSettings.CurrentReadStrategy(Path.Combine(
                gameSettings.DefaultPaths.GameData,
                string.Format(gameSettings.DefaultPaths.LevelsFileNameTemplate, levelNo)))))
            {
                return Create(JsonConvert.DeserializeObject<LevelPackages>(tr.ReadToEnd()));
            }
        }

        public static Level Create(LevelPackages levelPackages)
        {
            Level level = JsonConvert.DeserializeObject<Level>(levelPackages.LevelCore);
            //level.Objects = JsonConvert.DeserializeObject<Inventory>(levelPackages.LevelObjects,
            //                                                         new NonChracterObjectConverter());
            //level.Objects.LinkObjectsToInventory();
            //foreach(var characterPackage in levelPackages.LevelCharacters)
            //{
            //    level.Characters.Add(Character.Load<Character>(characterPackage));
            //}
            return level;
        }
        public  int StartingCell { get; set; }
        public  Noun Name { get; set; }
    }
}
