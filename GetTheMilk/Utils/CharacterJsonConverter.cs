using System;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetTheMilk.Utils
{
    public class CharacterJsonConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
                var item = JObject.Load(reader);
                var factory = ObjectActionsFactory.GetFactory();
                var objectTypeId = item["ObjectTypeId"].Value<string>();
            if (objectTypeId == "Player")
            {
                var obj = item.ToObject<Player>();
                var objAction = factory.CreateObjectAction(objectTypeId);
                obj.AllowsTemplateAction = objAction.AllowsTemplateAction;
                obj.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
                return obj;
            }
            else
            {
                var obj = item.ToObject<Character>();
                var objAction = factory.CreateObjectAction(objectTypeId);
                obj.AllowsTemplateAction = objAction.AllowsTemplateAction;
                obj.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
                return obj;                
            }
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(Character).IsAssignableFrom(objectType);
        }
    }
}
