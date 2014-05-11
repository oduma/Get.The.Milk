using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class CanOpenerActions:ToolActions
    {

        public override bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (o is ICharacter && (a is Keep || a is GiveTo || a is Buy || a is Sell));
        }

        public CanOpenerActions()
        {
            ObjectTypeId = "CanOpener";
        }
    }
}
