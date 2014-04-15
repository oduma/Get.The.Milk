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
        private Inventory _inventory;
        private CharacterCollection _characters;
        public Map CurrentMap { get; set; }
        public int Number { get; set; }

        [JsonIgnore]
        public Inventory Inventory
        {
            get { return _inventory=(_inventory)??new Inventory(); }
            set
            {
                _inventory = value;
                if (_inventory != null)
                {
                    _inventory.Owner = this;
                }
            }
        }

        [JsonIgnore]
        public CharacterCollection Characters
        {
            get { return _characters=(_characters)??new CharacterCollection{Owner=this}; }
            set
            {
                _characters = value;
                if (_characters != null)
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
                           LevelObjects = JsonConvert.SerializeObject(Inventory.Save()),
                           LevelCharacters= new List<CharacterSavedPackages>()
                       };
            foreach (var character in Characters)
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
            level.CurrentMap.LinkToParentLevel(level);
            level.Inventory = Inventory.Load(JsonConvert.DeserializeObject<InventoryPackages>(levelPackages.LevelObjects,
                                                                     new NonChracterObjectConverter()));
            level.Inventory.LinkObjectsToInventory();
            foreach (var characterPackage in levelPackages.LevelCharacters)
            {
                level.Characters.Add(Character.Load<Character>(characterPackage));
            }
            level.Characters.LinkCharactersToInventory();
            return level;
        }
        public  int StartingCell { get; set; }
        public  Noun Name { get; set; }

    }
}
