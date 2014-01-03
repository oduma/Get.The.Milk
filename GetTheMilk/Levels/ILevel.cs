using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;

namespace GetTheMilk.Levels
{
    public interface ILevel:IInventoryOwner
    {
        Map[] Maps { get; }

        int Number { get; }

        Inventory PositionableObjects { get; }

        Inventory Characters { get; }

        IPlayer Player { get; }

        void Save();

        void Load(string saveFile);

        void EnterLevel(IPlayer player,int mapNumber, int cellNumber);
    }
}
