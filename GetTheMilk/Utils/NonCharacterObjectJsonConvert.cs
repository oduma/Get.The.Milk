using System;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetTheMilk.Utils
{
    public class NonChracterObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(NonCharacterObject).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var item = JObject.Load(reader);
                var factory = ObjectActionsFactory.GetFactory();
                if (item["ObjectCategory"].Value<int>() == 0)
                {
                    var obj = item.ToObject<NonCharacterObject>();
                    var objAction = factory.CreateObjectAction(item["ObjectTypeId"].Value<string>());
                    obj.AllowsTemplateAction = objAction.AllowsTemplateAction;
                    obj.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
                    return obj;
                }
                if (item["ObjectCategory"].Value<int>() == 1)
                {
                    var obj = item.ToObject<Tool>();
                    var objAction = factory.CreateObjectAction(item["ObjectTypeId"].Value<string>());
                    obj.AllowsTemplateAction = objAction.AllowsTemplateAction;
                    obj.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
                    return obj;
                }
                if (item["ObjectCategory"].Value<int>() == 2)
                {
                    var obj = item.ToObject<Weapon>();
                    var objAction = factory.CreateObjectAction(item["ObjectTypeId"].Value<string>());
                    obj.AllowsTemplateAction = objAction.AllowsTemplateAction;
                    obj.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
                    return obj;
                }

            }
            catch{}
            return null;
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
                throw new NotImplementedException();
        }
    }
}
