using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.Translators.Common;

namespace GetTheMilk.Objects
{
    public class WeaponActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return (a.ActionType==ActionType.Attack);

        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return ((a is Buy) || (a is Sell) || (a is GiveTo) || (a is TakeFrom)) && (o is ICharacter);
        }

        public WeaponActions()
        {
            ObjectTypeId = "Weapon";
        }
    }
}
