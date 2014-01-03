using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions
{
    public class Attack : FightAction
    {
        public override string Name
        {
            get { return "Attack"; }
        }

        public Hit Hit { get; set; }

        public ActionResult Perform(ICharacter character, ICharacter targetCharacter)
        {
            if (character is IPlayer)
                ((IPlayer)character).LoadInteractionsWithPlayer(targetCharacter);
            else if (targetCharacter is IPlayer)
                ((IPlayer)targetCharacter).LoadInteractionsWithPlayer(character);

            var counterHit = targetCharacter.PrepareDefenseHit();
            CalculationStrategies.CalculateDamages(Hit, counterHit,character,targetCharacter);
            if (targetCharacter.Health <= 0)
                return new ActionResult {ResultType = ActionResultType.Win};
            if (character.Health <= 0)
                return new ActionResult {ResultType = ActionResultType.Lost};
            return new ActionResult {ResultType = ActionResultType.Ok};
        }

    }
}
