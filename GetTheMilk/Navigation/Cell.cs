namespace GetTheMilk.Navigation
{
    public class Cell
    {
        public int Number { get; set; }

        public int NorthCell { get; set; }

        public int SouthCell { get; set; }

        public int EastCell { get; set; }

        public int WestCell { get; set; }

        public int TopCell { get; set; }

        public int BottomCell { get; set; }

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
                    return 0;
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
        public bool IsObjective { get; set; }
    }
}
