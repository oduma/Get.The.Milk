using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Objects;
using Newtonsoft.Json;

namespace GetTheMilk.Characters
{
    public class CharacterCollection:List<Character>
    {
        public int MaximumCapacity { get; set; }

        public void LinkCharactersToInventory()
        {
            foreach (var o in this.Where(o => o.StorageContainer == null))
                o.StorageContainer = this;
        }

        [JsonIgnore]
        public IInventoryOwner Owner { get; set; }


        public InventoryType InventoryType { get; set; }

        public void Add<T>(params T[] os) where T : Character
        {
            foreach (T o in os)
            {
                base.Add(o);
                if (o.StorageContainer != null)
                    o.StorageContainer.Remove(o);
                o.StorageContainer = this;
            }
        }

        public void Remove<T>(T o) where T : Character
        {
            if (Contains(o))
            {
                base.Remove(o);
                o.StorageContainer = null;
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

        public static CharacterCollection Load(CollectionPackage packages)
        {

            var result = new CharacterCollection();
            result.InventoryType = JsonConvert.DeserializeObject<InventoryType>(packages.InventoryType);
            result.MaximumCapacity = JsonConvert.DeserializeObject<int>(packages.MaximumCapacity);
            List<ContainerWithActionsPackage> objs = JsonConvert.DeserializeObject<List<ContainerWithActionsPackage>>(packages.Contents);
            foreach (var obj in objs)
            {
                result.Add(Character.Load<Character>(obj));
            }

            return result;
        }

    }
}
