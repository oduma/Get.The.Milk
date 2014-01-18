using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class MovementActionExtraData
    {
        public IPositionableObject[] ObjectsBlocking { get; set; }

        public IPositionableObject[] CharactersInRange { get; set; }

        public int MoveToCell { get; set; }

        public IPositionableObject[] ObjectsInRange { get; set; }

        public IPositionableObject[] ObjectsInCell { get; set; }
    }
}
