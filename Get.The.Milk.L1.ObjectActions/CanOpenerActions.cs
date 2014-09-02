using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class CanOpenerActions:ToolActions
    {

        public override bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return (o is ICharacter && (a.Category=="ObjectTransferActionTemplate"));
        }

        public CanOpenerActions()
        {
            ObjectTypeId = "CanOpener";
        }
    }
}
