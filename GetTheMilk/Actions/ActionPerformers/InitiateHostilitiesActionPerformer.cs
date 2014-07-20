using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class InitiateHostilitiesActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override PerformActionResult Perform(ActionTemplates.TwoCharactersActionTemplate actionTemplate)
        {
            var result = base.Perform(actionTemplate);
            if (result.ResultType == ActionResultType.Ok)
            {
                ActionsHelper.SelectWeapon(actionTemplate.ActiveCharacter, CalculationStrategies.SelectAnAttackWeapon(actionTemplate.ActiveCharacter),
                                           WeaponType.Attack);
                ActionsHelper.SelectWeapon(actionTemplate.ActiveCharacter, CalculationStrategies.SelectADefenseWeapon(actionTemplate.ActiveCharacter),
                                           WeaponType.Deffense);
            }
            return result;

        }
    }
}
