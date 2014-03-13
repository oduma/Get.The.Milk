using System.Collections.Generic;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Objects
{
    public class Inventory
    {
        public int MaximumCapacity { get; set; }

        private List<NonCharacterObject> _objects;

        public List<NonCharacterObject> Objects
        {
            get { return _objects = (_objects) ?? new List<NonCharacterObject>(); }
        }

        public void LinkObjectsToInventory()
        {
            foreach (var o in _objects.Where(o => o.StorageContainer == null))
                o.StorageContainer = this;
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

        [JsonIgnore]
        public IInventoryOwner Owner { get; set; }

        public InventoryType InventoryType { get; set; }

        public bool Add<T>(params T[] os) where T : NonCharacterObject
        {
            foreach (T o in os)
            {
                if (o == null)
                    return false;
                if ((Objects.Count + 1 <= MaximumCapacity))
                {
                    Objects.Add(o);
                    if (o.StorageContainer != null)
                        o.StorageContainer.Remove(o);
                    o.StorageContainer = this;
                    if (Owner is ICharacter)
                    {
                        o.MapNumber = ((ICharacter)Owner).MapNumber;
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
            if (Objects.Contains(o))
            {
                Objects.Remove(o);
            }
        }
    }
}
