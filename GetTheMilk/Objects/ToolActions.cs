using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Objects
{
    public class ToolActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }
        public virtual bool AllowsAction(GameAction a)
        {
            return false;
        }

        public virtual bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return ((a is Buy) || (a is Sell) || (a is GiveTo) || (a is TakeFrom)) && (o is ICharacter);

        }
        public ToolActions()
        {
            ObjectTypeId = "Tool";
            ObjectCategory = ObjectCategory.Tool;

        }
    }
}
