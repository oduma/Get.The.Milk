using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectWithPossibleActionTemplates
    {
        public NonCharacterObject Object { get; set; }

        public BaseActionTemplate[] PossibleUsses { get; set; }
    }
}
