﻿using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Objects.Base;

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
