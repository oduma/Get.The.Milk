using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Objects
{
    public class WeaponActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public virtual bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return (a.Name.UniqueId == "Attack");

        }

        public virtual bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return ((a.Category == CategorysCatalog.ObjectTransferCategory) || (a.Name.UniqueId == "SelectAttackWeapon") ||
                    (a.Name.UniqueId == "SelectDefenseWeapon")) && (o is ICharacter);
        }

        public WeaponActions()
        {
            ObjectTypeId = "Weapon";
            ObjectCategory = ObjectCategory.Weapon;
        }
    }
}
