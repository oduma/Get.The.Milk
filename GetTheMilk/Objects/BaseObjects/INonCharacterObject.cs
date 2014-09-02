using GetTheMilk.Common;
using Newtonsoft.Json;

namespace GetTheMilk.Objects.BaseObjects
{
    public interface INonCharacterObject:IPositionable
    {
        [JsonIgnore]
        Inventory StorageContainer { get; set; }

        ObjectCategory ObjectCategory { get; }

        BasePackage Save();

    }
}
