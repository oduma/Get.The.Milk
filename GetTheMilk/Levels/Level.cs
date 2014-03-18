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
        private CharacterCollection _characters;
        private Inventory _objects;
        public Map[] Maps { get; set; }
        public int Number { get; set; }

        [JsonIgnore]
        public Inventory Objects    
        {
            get { return _objects; }
            set 
            { 
                _objects = value;
                if(_objects!=null)
                    _objects.Owner = this;

            }
        }

        [JsonIgnore]
        public  CharacterCollection Characters
        {
            get { return _characters=(_characters)??new CharacterCollection{Owner=this}; }
            set
            {
                _characters = value;
                if(_characters!=null)
                    _characters.Owner = this;
            }
        }

        [JsonIgnore]
        public IPlayer Player { get; set; }
        public  string Story { get; set; }

        public LevelPackages Save()
        {
            var levelPackages = new LevelPackages
                       {
                           LevelCore = JsonConvert.SerializeObject(this),
                           LevelObjects = JsonConvert.SerializeObject(Objects),
                           LevelCharacters= new List<CharacterSavedPackages>()
                       };
            foreach (var character in Characters.Characters)
                levelPackages.LevelCharacters.Add(character.Save());
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
            level.Objects = JsonConvert.DeserializeObject<Inventory>(levelPackages.LevelObjects,
                                                                     new NonChracterObjectConverter());
            level.Objects.LinkObjectsToInventory();
            foreach(var characterPackage in levelPackages.LevelCharacters)
            {
                level.Characters.Add(Character.Load<Character>(characterPackage));
            }
            return level;
        }

        public  int StartingMap { get; set; }
        public  int StartingCell { get; set; }
        public  Noun Name { get; set; }
    }
}
