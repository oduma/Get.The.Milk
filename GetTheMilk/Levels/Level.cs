using System.IO;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;

namespace GetTheMilk.Levels
{
    public abstract class Level:ILevel
    {
        public abstract Map[] Maps { get; }
        public abstract int Number { get; }
        public abstract Inventory PositionableObjects { get; }
        public abstract Inventory Characters { get; }
        public IPlayer Player { get; set; }
        public abstract string Story { get; }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Load(string saveFile)
        {
            if(!string.IsNullOrEmpty(saveFile) || File.Exists(saveFile))
            {
                //Load from file;
            }
        }

        public abstract int StartingMap { get; }
        public abstract int StartingCell { get; }
        public abstract Noun Name { get; }
    }
}
