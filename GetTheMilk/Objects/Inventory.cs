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

        public CollectionPackage Save()
        {
            return new CollectionPackage
                   {
                       Contents = JsonConvert.SerializeObject(this.Select(obj => obj.Save())),
                       InventoryType = JsonConvert.SerializeObject(InventoryType),
                       MaximumCapacity = JsonConvert.SerializeObject(MaximumCapacity)
                   };
        }

        public static Inventory Load(CollectionPackage packages)
        {

            var result = new Inventory();
            result.InventoryType = JsonConvert.DeserializeObject<InventoryType>(packages.InventoryType);
            result.MaximumCapacity = JsonConvert.DeserializeObject<int>(packages.MaximumCapacity);
            List<BasePackage> objs = JsonConvert.DeserializeObject<List<BasePackage>>(packages.Contents);
            foreach (var obj in objs)
            {
                result.Add(NonCharacterObject.Load<NonCharacterObject>(obj));
            }

            return result;
        }
    }
}
