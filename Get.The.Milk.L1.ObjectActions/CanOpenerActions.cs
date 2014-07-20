using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class CanOpenerActions:ToolActions
    {

        public override bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return (o is ICharacter && (a.Name.Category=="ObjectTransferActionTemplate"));
        }

        public CanOpenerActions()
        {
            ObjectTypeId = "CanOpener";
        }
    }
}
