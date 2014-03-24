using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.BaseActions
{
    public class TwoCharactersAction : GameAction
    {
        public override bool CanPerform()
        {
            return ActiveCharacter.AllowsAction(this) && TargetCharacter.AllowsIndirectAction(this, ActiveCharacter);
        }

        protected void EstablishInteractionRules()
        {
            if (ActiveCharacter is IPlayer)
                ((IPlayer)ActiveCharacter).LoadInteractionsWithPlayer(TargetCharacter);
            else if (TargetCharacter is IPlayer)
                ((IPlayer)TargetCharacter).LoadInteractionsWithPlayer(ActiveCharacter);

        }

        public override GameAction CreateNewInstance()
        {
            return new TwoCharactersAction();
        }

    }
}
