using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetTheMilk.Utils
{
    public class ActionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(GameAction).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var item = JObject.Load(reader);
                var actionCategory = (ActionType)(item["ActionType"].Value<int>());
                switch(actionCategory)
                {
                    case ActionType.Open:
                        return item.ToObject<Open>();
                        case ActionType.Default:
                        return item.ToObject<GameAction>();
                        case ActionType.Attack:
                        return item.ToObject<Attack>();
                        case ActionType.Buy:
                        return item.ToObject<Buy>();
                        case ActionType.EnterLevel:
                        return item.ToObject<EnterLevel>();
                        case ActionType.ExposeInventory:
                        return item.ToObject<ExposeInventory>();
                        case ActionType.GiveTo:
                        return item.ToObject<GiveTo>();
                        case ActionType.Keep:
                        return item.ToObject<Keep>();
                        case ActionType.Kick:
                        return item.ToObject<Kick>();
                        case ActionType.Kill:
                        return item.ToObject<Kill>();
                        case ActionType.Communicate:
                        return item.ToObject<CommunicateAction>();
                        case ActionType.Quit:
                        return item.ToObject<Quit>();
                        case ActionType.Meet:
                        return item.ToObject<Meet>();
                        case ActionType.Teleport:
                        return item.ToObject<Teleport>();
                        case ActionType.TakeMoneyFrom:
                        return item.ToObject<TakeMoneyFrom>();
                        case ActionType.TakeFrom:
                        return item.ToObject<TakeFrom>();
                        case ActionType.Walk:
                        return item.ToObject<Walk>();
                        case ActionType.Run:
                        return item.ToObject<Run>();
                        case ActionType.Sell:
                        return item.ToObject<Sell>();
                }
            }
            catch { }
            return null;
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
