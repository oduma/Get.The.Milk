using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Navigation
{
    public partial class Cell
    {
        [JsonIgnore]
        public IEnumerable<NonCharacterObject> AllObjects
        {
            get { return Parent.Parent.Inventory.Where(o => o.CellNumber == Number); }
        }

        [JsonIgnore]
        public IEnumerable<Character> AllCharacters
        {
            get { return Parent.Parent.Characters.Where(c => c.CellNumber == Number); }
        }


        [JsonIgnore]
        public Map Parent { get; private set; }

        public int Number { get; set; }

        public int NorthCell { get; set; }

        public int SouthCell { get; set; }

        public int EastCell { get; set; }

        public int WestCell { get; set; }

        public int TopCell { get; set; }

        public int BottomCell { get; set; }

        public int Floor { get; set; }

        public int GetNeighbourCellNumber(Direction direction)
        {
            switch(direction)
            {
                case Direction.Bottom:
                    return BottomCell;
                case Direction.West:
                    return WestCell;
                case Direction.North:
                    return NorthCell;
                case Direction.Top:
                    return TopCell;
                case Direction.East:
                    return EastCell;
                case Direction.South:
                    return SouthCell;
                default:
                    return -1;
            }
        }

        public bool IsANeighbourOfOrSelf(int cellNumber)
        {
            return (cellNumber == NorthCell || cellNumber == WestCell || cellNumber == SouthCell ||
                    cellNumber == EastCell || cellNumber == TopCell || cellNumber == BottomCell);
        }

        public Direction GetDirectionToCell(int cellNumber)
        {
            if(cellNumber==NorthCell)
                return Direction.North;
            if(cellNumber==WestCell)
                return Direction.West;
            if(cellNumber==SouthCell)
                return Direction.South;
            if(cellNumber==EastCell)
                return Direction.East;
            if(cellNumber==BottomCell)
                return Direction.Bottom;
            if(cellNumber==TopCell)
                return Direction.Top;
            return Direction.None;
        }

        public void LinkToParent(Map map)
        {
            Parent = map;
        }
    }
}
