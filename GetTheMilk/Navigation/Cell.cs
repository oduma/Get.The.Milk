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
                    cellNumber == EastCell || cellNumber == TopCell || cellNumber == BottomCell || cellNumber==Number);
        }
    }
}
