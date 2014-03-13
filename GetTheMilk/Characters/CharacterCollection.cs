using System.Collections.Generic;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using Newtonsoft.Json;

namespace GetTheMilk.Characters
{
    public class CharacterCollection
    {
        private List<Character> _characters;

        public List<Character> Characters
        {
            get { return _characters = (_characters) ?? new List<Character>(); }
        }

        public void LinkCharactersToInventory()
        {
            foreach (var o in _characters.Where(o => o.StorageContainer == null))
                o.StorageContainer = this;
        }

        [JsonIgnore]
        public IInventoryOwner Owner { get; set; }

        public void Add<T>(params T[] os) where T : Character
        {
            foreach (T o in os)
            {
                Characters.Add(o);
                if (o.StorageContainer != null)
                    o.StorageContainer.Remove(o);
                o.StorageContainer = this;
            }
        }

        public void Remove<T>(T o) where T : Character
        {
            if (Characters.Contains(o))
            {
                Characters.Remove(o);
            }
        }

    }
}
