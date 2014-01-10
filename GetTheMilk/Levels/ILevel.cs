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

        IPlayer Player { get; set; }

        string Story { get; }

        void Save();

        void Load(string saveFile);

        int StartingMap { get; }

        int StartingCell { get; }

    }
}
