using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions
{
    public class InitiateHostilities: TwoCharactersAction
    {
        public InitiateHostilities()
        {
            Name = new Verb { Infinitive = "To Attack", Past = "attacked", Present = "attack" };
            ActionType = ActionType.InitiateHostilities;
            StartingAction = true;

        }

        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};

            EstablishInteractionRules();
            if (ActiveCharacter is IPlayer)
            {
                //the target character does something and passes the initiative to the player
                return PerformResponseAction(ActionType);
            }
            ActionsHelper.SelectWeapon(ActiveCharacter, CalculationStrategies.SelectAnAttackWeapon(ActiveCharacter), WeaponType.Attack);
            ActionsHelper.SelectWeapon(ActiveCharacter, CalculationStrategies.SelectADefenseWeapon(ActiveCharacter), WeaponType.Deffense);
            return new ActionResult { ForAction = this, ResultType = ActionResultType.Ok, ExtraData = GetAvailableActions() };
        }


        public override GameAction CreateNewInstance()
        {
            return new InitiateHostilities();
        }


    }
}
