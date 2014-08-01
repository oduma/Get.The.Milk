using System;
using System.Collections;
using System.Collections.Generic;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Utils;
using Newtonsoft.Json;

namespace GetTheMilk.Objects.BaseObjects
{
    public class NonCharacterObject : BaseActionEnabledObject,INonCharacterObject
    {
        public string ObjectTypeId { get; set; }
        public Noun Name { get; set; }
        public int CellNumber { get; set; }
        public bool BlockMovement { get; set; }

        [JsonIgnore]
        public Func<BaseActionTemplate, bool> AllowsTemplateAction { get; set; }
        [JsonIgnore]
        public Func<BaseActionTemplate, IPositionable, bool> AllowsIndirectTemplateAction { get; set; }

        public ObjectCategory ObjectCategory { get; protected set; }

        public BasePackage Save()
        {
            return new BasePackage
                       {
                           Core = JsonConvert.SerializeObject(this),
                           ActionTemplates = JsonConvert.SerializeObject(AllActions),
                           Interactions=JsonConvert.SerializeObject(Interactions)
                       };
        }

        public Inventory StorageContainer { get; set; }

        public NonCharacterObject()
        {
            ObjectCategory = ObjectCategory.Decor;
        }

        public static T Load<T>(BasePackage package) where T: NonCharacterObject, new()
        {
            var obj = JsonConvert.DeserializeObject<T>(package.Core, new NonChracterObjectConverter());
            if(package.ActionTemplates!=null)
            {
                var objActions = JsonConvert.DeserializeObject<Dictionary<string,BaseActionTemplate>>(package.ActionTemplates,
                                                                                     new ActionTemplateJsonConverter());
                foreach (var objAction in objActions)
                    obj.AddAvailableAction(objAction.Value);
            }
            if(package.Interactions!=null)
                obj.Interactions = JsonConvert.DeserializeObject<SortedList<string, Interaction[]>>(package.Interactions,
                                                                                                new ActionTemplateJsonConverter
                                                                                                    ());
            return obj;
        }
    }
}
