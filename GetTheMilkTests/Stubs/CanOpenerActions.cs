using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilkTests.Stubs
{
    public class CanOpenerActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return false;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (o is ICharacter && (a is Keep || a is GiveTo || a is Buy || a is Sell));
        }

        public CanOpenerActions()
        {
            ObjectTypeId = "CanOpener";
        }
    }
}
