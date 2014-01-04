using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;

namespace GetTheMilk.TestLevel
{
    public class TestLevel2:Level
    {
        //| 1   |   2   |
        //---------------
        //| 3   |   4^  |
        //===============
        //| 5v   |   6  |
        //---------------
        //| 7   |   8   |

        public override Map[] Maps
        {
            get
            {
                return new Map[]
                           {
                               new Map
                                   {
                                       Number = 1,
                                       Cells =
                                           new Cell[]
                                               {
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 0,
                                                           Number = 1,
                                                           SouthCell = 3,
                                                           TopCell = 0,
                                                           EastCell = 2
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 1,
                                                           NorthCell = 0,
                                                           Number = 2,
                                                           SouthCell = 4,
                                                           TopCell = 0,
                                                           EastCell = 0
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 1,
                                                           Number = 3,
                                                           SouthCell = 0,
                                                           TopCell = 0,
                                                           EastCell = 4
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 2,
                                                           Number = 4,
                                                           SouthCell = 0,
                                                           TopCell = 5,
                                                           EastCell = 3
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 4,
                                                           WestCell = 0,
                                                           NorthCell = 0,
                                                           Number = 5,
                                                           SouthCell = 7,
                                                           TopCell = 0,
                                                           EastCell = 6
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 5,
                                                           NorthCell = 0,
                                                           Number = 6,
                                                           SouthCell = 8,
                                                           TopCell = 0,
                                                           EastCell = 0
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 5,
                                                           Number = 7,
                                                           SouthCell = 0,
                                                           TopCell = 0,
                                                           EastCell = 8
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 7,
                                                           NorthCell = 6,
                                                           Number = 8,
                                                           SouthCell = 0,
                                                           TopCell = 0,
                                                           EastCell = 0
                                                       }
                                               }
                                   }
                           };
            }
        }


        public override int Number { get { return 2; } }

        public override Inventory PositionableObjects
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Inventory Characters
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string Name
        {
            get { return "The Light side of Level 2"; }
        }
    }
}
