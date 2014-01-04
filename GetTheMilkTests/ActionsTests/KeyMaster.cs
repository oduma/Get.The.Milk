using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class KeyMaster:Character
    {
        private Personality _personality;

        public override string Name
        {
            get { return "Key Master"; }
        }

        public override bool AllowsAction(GameAction a)
        {
                return (a is GiveTo);
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            if (a is GiveTo && o is AnyKey)
                return false;
            if (a is GiveTo)
                return true;
            if (a is CommunicateAction && ((CommunicateAction)a).Message == "Give me the red key." && o is ICharacter)
                return true;
            return false;
        }

       public KeyMaster()
        {
            BlockMovement = false;
        }

        public override Personality Personality
        {
            get { return _personality = (_personality) ?? new Personality { Type = PersonalityType.Neutral }; }
        }
    }
}
