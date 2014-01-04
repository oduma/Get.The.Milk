using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class KeylessChild:Character
    {
        private Personality _personality;

        public override string Name
        {
            get { return "Keyless Child"; }
        }

        public override bool AllowsAction(GameAction a)
        {
            return (a is CommunicateAction); 
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            if (a is GiveTo && o is AnyKey)
                return true;
            if (a is Sell && o is ITransactionalObject)
                return true;
            if (a is CommunicateAction)
                return true;
            return false;
        }

        public KeylessChild()
        {
            BlockMovement = true;
        }

        public override Personality Personality
        {
            get { return _personality = (_personality) ?? new Personality { Type = PersonalityType.Neutral }; }
        }
    }
}
