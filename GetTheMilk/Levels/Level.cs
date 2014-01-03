using System.IO;
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
        public IPlayer Player { get; private set; }

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

        public void EnterLevel(IPlayer player, int mapNumber, int cellNumber)
        {
            Player = player;
            Player.MapNumber = mapNumber;
            Player.CellNumber = cellNumber;
        }

        public abstract string Name { get; }
    }
}
