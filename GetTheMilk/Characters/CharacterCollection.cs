using System.Collections.Generic;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using Newtonsoft.Json;

namespace GetTheMilk.Characters
{
    public class CharacterCollection:List<Character>
    {
        public void LinkCharactersToInventory()
        {
            foreach (var o in this.Where(o => o.StorageContainer == null))
                o.StorageContainer = this;
        }

        [JsonIgnore]
        public IInventoryOwner Owner { get; set; }

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

    }
}
