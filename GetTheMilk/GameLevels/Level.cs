using System.IO;
using GetTheMilk.Characters;
using GetTheMilk.Common;
using GetTheMilk.Objects;
using GetTheMilk.Utils;
using Newtonsoft.Json;

namespace GetTheMilk.GameLevels
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
        public Player Player { get; set; }
        public  string Story { get; set; }

        public ContainerNoActionsPackage PackageForSave()
        {
            var levelPackages = new ContainerNoActionsPackage
                       {
                           Core = JsonConvert.SerializeObject(this),
                           InventoryCore = JsonConvert.SerializeObject(Inventory.Save()),
                           LevelCharacters= JsonConvert.SerializeObject(Characters.Save())
                       };
            return levelPackages;
        }

        public static Level Create(int levelNo)
        {
            var gameSettings = GameSettings.GetInstance();
            var levelDefFileName = Path.Combine(
                gameSettings.DefaultPaths.GameData,
                string.Format(gameSettings.DefaultPaths.LevelsFileNameTemplate, levelNo));
            
            if(!File.Exists(levelDefFileName))
                return null;
            using(TextReader tr = new StreamReader(gameSettings.CurrentReadStrategy(levelDefFileName)))
            {
                return Create(JsonConvert.DeserializeObject<ContainerNoActionsPackage>(tr.ReadToEnd()));
            }
        }

        public static Level Create(ContainerNoActionsPackage levelPackages)
        {
            Level level = JsonConvert.DeserializeObject<Level>(levelPackages.Core);
            if(level.CurrentMap!=null)
                level.CurrentMap.LinkToParentLevel(level);
            level.Inventory = (levelPackages.InventoryCore!=null)?Inventory.Load(JsonConvert.DeserializeObject<CollectionPackage>(levelPackages.InventoryCore,
                new NonChracterObjectConverter())):new Inventory();
            level.Inventory.LinkObjectsToInventory();
            level.Characters = (levelPackages.LevelCharacters != null) ? CharacterCollection.Load(JsonConvert.DeserializeObject<CollectionPackage>(levelPackages.LevelCharacters,
    new CharacterJsonConverter())) : new CharacterCollection();
            level.Characters.LinkCharactersToInventory();

            return level;
        }
        public  int StartingCell { get; set; }
        public  Noun Name { get; set; }

        public string FinishMessage { get; set; }

        public SizeOfLevel SizeOfLevel { get; set; }

        public int ObjectiveCell { get; set; }

        public Level()
        {
            SizeOfLevel = SizeOfLevel.VerySmall;
        }
    }
}
