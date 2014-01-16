using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class CanOpener:Tool
    {
        private string _approachingMessage;
        private string _closeUpMessage;

        public CanOpener()
        {
            BlockMovement = false;
            BuyPrice = 5;
            SellPrice = 1;
            Name = new Noun { Main = "Can Opener" };
            _approachingMessage = "Some small tool.";
            _closeUpMessage = "In the grass right in front there is a can opener.";

        }

        public override string ApproachingMessage
        {
            get { return _approachingMessage; }
            set { _approachingMessage = value; }
        }

        public override string CloseUpMessage
        {
            get { return _closeUpMessage; }
            set { _closeUpMessage = value; }
        }
    }
}
