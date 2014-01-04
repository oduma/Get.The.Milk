using System.Collections.Generic;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Objects
{
    public class Inventory
    {
        public int MaximumCapacity { get; set; }

        private List<IPositionableObject> _objects;

        public List<IPositionableObject> Objects
        {
            get { return _objects = (_objects) ?? new List<IPositionableObject>(); }
        }

        public void FollowTheLeader()
        {
            if (Owner is ICharacter)
            {
                foreach (var o in Objects)
                {
                    o.CellNumber = ((ICharacter)Owner).CellNumber;
                    o.MapNumber = ((ICharacter)Owner).MapNumber;
                }
            }
        }

        public IInventoryOwner Owner { get; set; }

        public InventoryType InventoryType { get; set; }

        public bool Add<T>(params T[] os) where T:IPositionableObject
        {
            foreach (T o in os)
            {
                if ((Objects.Count + 1 <= MaximumCapacity))
                {
                    Objects.Add(o);
                    if(o.StorageContainer!=null)
                        o.StorageContainer.Remove(o);
                    o.StorageContainer=this;
                    if (Owner is ICharacter)
                    {
                        o.MapNumber = ((ICharacter) Owner).MapNumber;
                        o.CellNumber = ((ICharacter) Owner).CellNumber;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void Remove<T>(T o) where T:IPositionableObject
        {
            if(Objects.Contains(o))
            {
                Objects.Remove(o);
            }
        }
    }
}
