using GetTheMilk.BaseCommon;
using Newtonsoft.Json;

namespace GetTheMilk.Objects.BaseObjects
{
    public interface INonCharacterObject:IPositionable,IObjectHumanInterface
    {
        [JsonIgnore]
        Inventory StorageContainer { get; set; }

        ObjectCategory ObjectCategory { get; }

        BasePackage Save();

    }
}
