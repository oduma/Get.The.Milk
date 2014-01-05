namespace GetTheMilk.Abstracted.UI.Models
{
    public class GameViewModel:BaseActivityViewModel
    {

        public GameViewModel()
        {
            Activities = new Activity[]
                           {
                               new Activity
                                   {DisplayKeyTrigger = "T", DisplayText = "ell whole story", InternalUrl = @"Story/0",KeyTriggers=new []{'t','T'}},
                               new Activity {DisplayKeyTrigger = "N", DisplayText = "ew Game", InternalUrl = @"NewGame",KeyTriggers=new []{'n','N'}},
                               new Activity {DisplayKeyTrigger = "L", DisplayText = "oad Game", InternalUrl = @"LoadGame",KeyTriggers=new []{'l','L'}},
                               new Activity {DisplayKeyTrigger = "S", DisplayText = "etup", InternalUrl = @"Setup",KeyTriggers=new []{'s','S'}},
                               new Activity {DisplayKeyTrigger = "ESC", DisplayText = " Exit Game", InternalUrl = "",KeyTriggers=new char[0]}
                           };
        }
        public string Description { get; set; }

        public string Story { get; set; }
    }
}
