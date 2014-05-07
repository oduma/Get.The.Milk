using System;

namespace GetTheMilk.Actions
{
    public class LevelBuilderAccesiblePropertyAttribute:Attribute
    {
        public Type SourceType { get; private set; }

        public LevelBuilderAccesiblePropertyAttribute(Type sourceType)
        {
            SourceType = sourceType;
        }
    }
}
