using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class InitiateHostilitiesActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            var result = base.Perform(actionTemplate);
            if (result.ResultType == ActionResultType.Ok)
            {
                ActionsHelper.SelectWeapon(actionTemplate.ActiveCharacter, CalculationStrategies.SelectAWeapon(actionTemplate.ActiveCharacter,WeaponType.Attack),
                                           WeaponType.Attack);
                ActionsHelper.SelectWeapon(actionTemplate.ActiveCharacter, CalculationStrategies.SelectAWeapon(actionTemplate.ActiveCharacter,WeaponType.Deffense),
                                           WeaponType.Deffense);
            }
            return result;

        }
    }
}
