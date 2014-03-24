using System.Linq;

namespace GetTheMilk.Navigation
{
    public class Map
    {
        public int LevelNo { get; set; }
        public Cell[] Cells { get; set; }
        public bool AreInRange(int activeCellNumber, int passiveCellNumber)
        {
            var activeCell = Cells.FirstOrDefault(c => c.Number == activeCellNumber);
            if (activeCell == null)
                return false;
            return activeCell.IsANeighbourOfOrSelf(passiveCellNumber);

        }
    }
}
