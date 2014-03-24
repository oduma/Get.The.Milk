using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Fight;
using GetTheMilk.BaseCommon;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions
{
    public class Attack : TwoCharactersAction
    {
        public Attack()
        {
            Name = new Verb { Infinitive = "To Attack", Past = "attacked", Present = "attack" };
            ActionType = ActionType.Attack;
        }

        public Hit Hit { get; set; }

        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};

            EstablishInteractionRules();

            var counterHit = TargetCharacter.PrepareDefenseHit();
            CalculationStrategies.CalculateDamages(Hit, counterHit,ActiveCharacter,TargetCharacter);
            if (TargetCharacter.Health <= 0)
                return new ActionResult {ResultType = ActionResultType.Win,ForAction=this};
            if (ActiveCharacter.Health <= 0)
                return new ActionResult {ResultType = ActionResultType.Lost,ForAction=this};
            return new ActionResult {ResultType = ActionResultType.Ok,ForAction=this};
        }

        public override GameAction CreateNewInstance()
        {
            return new Attack();
        }

    }
}
