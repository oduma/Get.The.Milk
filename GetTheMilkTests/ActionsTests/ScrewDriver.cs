using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class ScrewDriver:Tool
    {
        public ScrewDriver()
        {
            BlockMovement = false;
            BuyPrice = 5;
            SellPrice = 1;
            Name = new Noun {Main = "SkrewDriver", Narrator = "Skrew Driver"};
        }

        public override bool AllowsAction(GameAction a)
        {
                return (a is Pick || a is Keep);
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return ((o is ICharacter) && (a is GiveTo || a is Buy || a is Sell || a is Pick || a is Keep));
        }

        public override string ApproachingMessage { get; set; }
        public override string CloseUpMessage { get; set; }
    }
}
