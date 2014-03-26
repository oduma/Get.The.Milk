using System.Collections.Generic;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Objects
{
    public class Inventory:List<NonCharacterObject>
    {
        public int MaximumCapacity { get; set; }
        public void LinkObjectsToInventory()
        {
            foreach (var o in this.Where(o => o.StorageContainer == null))
                o.StorageContainer = this;
        }

        public void FollowTheLeader()
        {
            if (Owner is ICharacter)
            {
                foreach (var o in this)
                {
                    o.CellNumber = ((ICharacter)Owner).CellNumber;
                }
            }
        }

        [JsonIgnore]
        public IInventoryOwner Owner { get; set; }

        public InventoryType InventoryType { get; set; }

        public bool Add<T>(params T[] os) where T : NonCharacterObject
        {
            foreach (T o in os)
            {
                if (o == null)
                    return false;
                if ((Count + 1 <= MaximumCapacity))
                {
                    base.Add(o);
                    if (o.StorageContainer != null)
                        o.StorageContainer.Remove(o);
                    o.StorageContainer = this;
                    if (Owner is ICharacter)
                    {
                        o.CellNumber = ((ICharacter)Owner).CellNumber;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void Remove<T>(T o) where T : NonCharacterObject
        {
            if (Contains(o))
            {
                base.Remove(o);
            }
        }
    }
}
