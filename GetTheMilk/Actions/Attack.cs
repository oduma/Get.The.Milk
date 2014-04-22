using System.Collections.Generic;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Fight;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
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
            var characterPlayer = (ActiveCharacter is IPlayer) ? ActiveCharacter : TargetCharacter;
            var oponentCharacter = (ActiveCharacter is IPlayer) ? TargetCharacter : ActiveCharacter;

            if (characterPlayer.Health<=0)
            {
                ((Level)oponentCharacter.StorageContainer.Owner).Player = null;
                characterPlayer = null;

                //player has lost
                return new ActionResult
                           {
                               ResultType = ActionResultType.Lost,
                               ForAction = this
                           };
            }
            if (oponentCharacter.Health<=0)
            {
                characterPlayer.Experience += CalculationStrategies.CalculateWinExperience(characterPlayer, oponentCharacter);
                PileageCharacter(characterPlayer, oponentCharacter);
                if (oponentCharacter.StorageContainer != null && oponentCharacter.StorageContainer.Owner != null)
                    oponentCharacter.StorageContainer.Remove(oponentCharacter as Character);

                oponentCharacter = null;
                //player won
                return new ActionResult
                {
                    ResultType = ActionResultType.Win,
                    ForAction = this
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
