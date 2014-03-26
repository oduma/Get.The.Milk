using System.Linq;
using GetTheMilk.Levels;
using Newtonsoft.Json;

namespace GetTheMilk.Navigation
{
    public class Map
    {
        [JsonIgnore]
        public Level Parent { get; set; }
        public Cell[] Cells { get; set; }
        public bool AreInRange(int activeCellNumber, int passiveCellNumber)
        {
            var activeCell = Cells.FirstOrDefault(c => c.Number == activeCellNumber);
            if (activeCell == null)
                return false;
            return activeCell.IsANeighbourOfOrSelf(passiveCellNumber);

        }

        public void LinkToParentLevel(Level level)
        {
            Parent = level;
            foreach (var cell in Cells)
            {
                cell.LinkToParent(this);
            }
        }
    }
}
