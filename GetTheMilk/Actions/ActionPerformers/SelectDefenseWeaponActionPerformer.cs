using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class SelectDefenseWeaponActionPerformer:OneObjectActionTemplatePerformer
    {
        public override PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            var result = base.Perform(actionTemplate);
            if(result.ResultType==ActionResultType.Ok)
                ActionsHelper.SelectWeapon(actionTemplate.ActiveCharacter, 
                    actionTemplate.TargetObject as Weapon, WeaponType.Deffense);
            return result;
        }

    }
}
