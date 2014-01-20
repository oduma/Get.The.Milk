using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class RedKey : AnyKey
    {
        private string _approachingMessage;
        private string _closeUpMessage;

        public RedKey()
        {
            Name = new Noun { Main = "Red Key" ,Narrator = "the Red Key"};
            _approachingMessage = "A glint catches your eye.";
            _closeUpMessage = "the Red Key of Kirna and you wonder how did you knew what it was.";
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
