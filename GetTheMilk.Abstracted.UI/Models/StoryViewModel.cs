using System;

namespace GetTheMilk.Abstracted.UI.Models
{
    public class StoryViewModel:BaseActivityViewModel
    {
        private string[] _fullStoryLines;

        public StoryViewModel()
        {
            Activities = new Activity[]
                             {
                                 new Activity
                                     {
                                         DisplayKeyTrigger = "N",
                                         DisplayText = "ext page",
                                         InternalUrl = @"Story/{0}",
                                         KeyTriggers = new[] {'N','n'},
                                         ActivityType=ActivityType.Paging
                                     },
                               new Activity {DisplayKeyTrigger = "ESC", DisplayText = " Go Back", InternalUrl = "",KeyTriggers=new char[0]}

                             };
        }

        public string[] FullStoryLines
        {
            get { return _fullStoryLines; }
            set { _fullStoryLines = value;
                PagingInfo.TotalNumberOfRecords = value.Length;
            }
        }
    }
}
