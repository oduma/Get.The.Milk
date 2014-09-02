using System;
using GetTheMilk.Actions.ActionTemplates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetTheMilk.Utils
{
    public class ActionTemplateJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var item = JObject.Load(reader);
                var templateCategory = item["Category"].Value<string>();
                if (templateCategory == CategorysCatalog.ExposeInventoryCategory)
                    return item.ToObject(typeof(ExposeInventoryActionTemplate));
                if (templateCategory == CategorysCatalog.MovementCategory)
                    return item.ToObject<MovementActionTemplate>();
                if (templateCategory == CategorysCatalog.NoObjectCategory)
                    return item.ToObject<NoObjectActionTemplate>();
                if (templateCategory == CategorysCatalog.ObjectTransferCategory)
                    return item.ToObject<ObjectTransferActionTemplate>();
                if (templateCategory == CategorysCatalog.ObjectUseOnObjectCategory)
                    return item.ToObject<ObjectUseOnObjectActionTemplate>();
                if (templateCategory == CategorysCatalog.OneObjectCategory)
                    return item.ToObject<OneObjectActionTemplate>();
                if (templateCategory == CategorysCatalog.TwoCharactersCategory)
                    return item.ToObject<TwoCharactersActionTemplate>();
                return null;
            }
            catch { return null; }
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(BaseActionTemplate).IsAssignableFrom(objectType);
        }
    }
}
