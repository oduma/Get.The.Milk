using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using Newtonsoft.Json;

namespace GetTheMilk.Objects.BaseObjects
{
    public class NonCharacterObject : INonCharacterObject
    {
        public string ObjectTypeId { get; set; }
        public Noun Name { get; set; }
        public int CellNumber { get; set; }
        public bool BlockMovement { get; set; }
        [JsonIgnore]
        public Func<GameAction, bool> AllowsAction { get; set; }

        [JsonIgnore]
        public Func<GameAction, IPositionable, bool> AllowsIndirectAction { get; set; }

        public ObjectCategory ObjectCategory { get; protected set; }

        public string CloseUpMessage { get; set; }
        public Inventory StorageContainer { get; set; }

        public NonCharacterObject()
        {
            ObjectCategory = ObjectCategory.Decor;
        }
    }
}
