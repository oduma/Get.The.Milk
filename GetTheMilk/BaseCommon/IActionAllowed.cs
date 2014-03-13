using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.BaseCommon
{
    public interface IActionAllowed
    {
        string ObjectTypeId { get; set; }

        bool AllowsAction(GameAction a);

        bool AllowsIndirectAction(GameAction a, IPositionable o);

    }
}
