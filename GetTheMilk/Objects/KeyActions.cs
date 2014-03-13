using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.Translators.Common;

namespace GetTheMilk.Objects
{
    public class KeyActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return (a is Open);
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (o is ICharacter && (a is Keep || a is GiveTo || a is Buy || a is Sell));
        }

        public KeyActions()
        {
            ObjectTypeId = "Key";
        }
    }
}
