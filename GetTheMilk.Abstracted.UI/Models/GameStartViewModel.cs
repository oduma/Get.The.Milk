namespace GetTheMilk.Abstracted.UI.Models
{
    public class GameStartViewModel:BaseActivityViewModel
    {
        public GameStartViewModel()
        {
            Activities = new Activity[]
                             {
                                 new Activity
                                 {
                                     DisplayKeyTrigger = "C",
                                     DisplayText = "ontinue",
                                     InternalUrl = "NewGame/PHASE2",
                                     KeyTriggers = new char[]{'c','C'}}, 
                                 new Activity
                                     {
                                         DisplayKeyTrigger = "ESC",
                                         DisplayText = " Go Back",
                                         InternalUrl = "",
                                         KeyTriggers = new char[0]
                                     }
                             };
            CharacterSetup=new CharacterModel();
        }

        public CharacterModel CharacterSetup { get; set; }

    }
}
