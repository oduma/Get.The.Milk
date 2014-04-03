using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Fight;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions
{
    public class Attack : TwoCharactersAction
    {
        public Attack()
        {
            Name = new Verb { Infinitive = "To Attack", Past = "attacked", Present = "attack" };
            ActionType = ActionType.Attack;
            StartingAction = false;
        }

        public Hit Hit { get; set; }

        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};

            var hit = ActiveCharacter.PrepareAttackHit();
            var counterHit = TargetCharacter.PrepareDefenseHit();
            CalculationStrategies.CalculateDamages(hit, counterHit,ActiveCharacter,TargetCharacter);
            if (TargetCharacter.Health <= 0)
                return new ActionResult {ResultType = ActionResultType.Win,ForAction=this};
            if (ActiveCharacter.Health <= 0)
                return new ActionResult {ResultType = ActionResultType.Lost,ForAction=this};
            if (ActiveCharacter is IPlayer)
                return PerformResponseAction(ActionType);
            return new ActionResult {ResultType = ActionResultType.Ok,ForAction=this,ExtraData=GetAvailableActions()};
        }

        public override GameAction CreateNewInstance()
        {
            return new Attack();
        }

    }
}
