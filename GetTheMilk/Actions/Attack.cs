using System.Collections.Generic;
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
            var result = DetermineWinLoseSituations();
            if (result != null)
                return result;
            if (ActiveCharacter is IPlayer)
            {
                return PerformResponseAction(ActionType);
            }
            return new ActionResult {ResultType = ActionResultType.Ok,ForAction=this,ExtraData=GetAvailableActions()};
        }

        private ActionResult DetermineWinLoseSituations()
        {
            if ((ActiveCharacter.Health <= 0 && ActiveCharacter is IPlayer)
                ||(TargetCharacter.Health <= 0 && TargetCharacter is IPlayer))
            {
                return new ActionResult
                           {
                               ResultType = ActionResultType.Lost,
                               ForAction = this,
                               ExtraData =
                                   new List<GameAction>
                                       {
                                           new Kill
                                               {
                                                   ActiveCharacter =
                                                       (ActiveCharacter is IPlayer) ?TargetCharacter:ActiveCharacter,
                                                    TargetCharacter=(ActiveCharacter is IPlayer) ?ActiveCharacter:TargetCharacter
                                               }
                                       }
                           };
            }
            if ((ActiveCharacter.Health <= 0 && !(ActiveCharacter is IPlayer))
                || (TargetCharacter.Health <= 0 && !(TargetCharacter is IPlayer)))
            {
                return new ActionResult
                {
                    ResultType = ActionResultType.Win,
                    ForAction = this,
                    ExtraData =
                        new List<GameAction>
                                       {
                                           new Kill
                                               {
                                                   ActiveCharacter =
                                                       (ActiveCharacter is IPlayer) ?ActiveCharacter:TargetCharacter,
                                                    TargetCharacter=(ActiveCharacter is IPlayer) ?TargetCharacter:ActiveCharacter
                                               }
                                       }
                };
            }
            return null;
        }

        public override GameAction CreateNewInstance()
        {
            return new Attack();
        }

    }
}
