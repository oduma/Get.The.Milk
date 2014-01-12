using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Test.Level.Level1Characters;
using GetTheMilk.Test.Level.Level1Objects;

namespace GetTheMilk.TestLevel
{
    public class TestLevel1:Level
    {
        private Inventory _positionableObjects;
        private Inventory _characters;
        //| 1   |   2W  |   3SC |
        //-----------------------
        //| 4RK |   5RD |   6   |
        //-----------------------
        //| 7   |   8W  |   9FC |

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
                                           new Cell[9]
                                               {
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 0,
                                                           Number = 1,
                                                           SouthCell = 4,
                                                           TopCell = 0,
                                                           EastCell = 2
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 1,
                                                           NorthCell = 0,
                                                           Number = 2,
                                                           SouthCell = 5,
                                                           TopCell = 0,
                                                           EastCell = 3
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 2,
                                                           NorthCell = 0,
                                                           Number = 3,
                                                           SouthCell = 6,
                                                           TopCell = 0,
                                                           EastCell = 0
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 1,
                                                           Number = 4,
                                                           SouthCell = 7,
                                                           TopCell = 0,
                                                           EastCell = 5
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 4,
                                                           NorthCell = 2,
                                                           Number = 5,
                                                           SouthCell = 8,
                                                           TopCell = 0,
                                                           EastCell = 6
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 5,
                                                           NorthCell = 3,
                                                           Number = 6,
                                                           SouthCell = 9,
                                                           TopCell = 0,
                                                           EastCell = 0
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 0,
                                                           NorthCell = 4,
                                                           Number = 7,
                                                           SouthCell = 0,
                                                           TopCell = 0,
                                                           EastCell = 8
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 7,
                                                           NorthCell = 5,
                                                           Number = 8,
                                                           SouthCell = 0,
                                                           TopCell = 0,
                                                           EastCell = 9
                                                       },
                                                   new Cell
                                                       {
                                                           BottomCell = 0,
                                                           WestCell = 8,
                                                           NorthCell = 6,
                                                           Number = 9,
                                                           SouthCell = 0,
                                                           TopCell = 0,
                                                           EastCell = 0
                                                       }
                                               }
                                   }
                           };
            }
        }

        public override int Number { get { return 1; } }

        public override Inventory PositionableObjects
        {
            get
            {
                if(_positionableObjects==null)
                {
                    _positionableObjects=(new Inventory{InventoryType=InventoryType.LevelInventory,MaximumCapacity=1000,Owner=this});
                    _positionableObjects.Add(new IPositionableObject[]
                                                 {
                                                     new RedKey
                                                         {
                                                             CellNumber = 4,
                                                             MapNumber = 1
                                                         },
                                                     new RedDoor
                                                         {
                                                             CellNumber = 5,
                                                             MapNumber = 1
                                                         },
                                                     new Wall
                                                         {
                                                             CellNumber = 2,
                                                             MapNumber = 1
                                                         },
                                                     new Wall
                                                         {
                                                             CellNumber = 7,
                                                             MapNumber = 1
                                                         }
                                                 });
                }

                return _positionableObjects;
            }
        }

        public override Inventory Characters
        {
            get
            {
                if (_characters != null)
                    return _characters;

                var skCharacter = new ShopKeeperCharacter {CellNumber = 3, MapNumber = 1};
                skCharacter.WeaponInventory.Add(new Knife());
                skCharacter.ToolInventory.Add(new CanOpener());
                var fCharacter = new FighterCharacter {CellNumber = 9, MapNumber = 1};
                fCharacter.WeaponInventory.Add(new Knife());
                _characters=new Inventory{InventoryType=InventoryType.LevelInventory,MaximumCapacity=10,Owner=this};
                _characters.Add(new ICharacter[]{skCharacter,fCharacter});
                return _characters;
            }
        }

        public override string Story
        {
            get { return @"You open your eyes in a strange but very small test world.
As you are a curious one you think about exploring it."; }
        }

        public override int StartingMap
        {
            get { return 1; }
        }

        public override int StartingCell
        {
            get { return 1; }
        }

        public override Noun Name
        {
            get { return new Noun{Main="The Dark Side of Level 1"};} 
        }
    }
}
