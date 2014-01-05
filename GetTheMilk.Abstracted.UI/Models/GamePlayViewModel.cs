namespace GetTheMilk.Abstracted.UI.Models
{
    public class GamePlayViewModel:BaseActivityViewModel
    {
        public GamePlayViewModel()
        {
            Activities = new Activity[]
                             {
                                 new Activity
                                     {
                                         DisplayKeyTrigger = "ESC",
                                         DisplayText = " Abandon",
                                         InternalUrl = "",
                                         KeyTriggers = new char[0]
                                     }
                             };

        }

        public int LevelNo { get; set; }
    }
}
