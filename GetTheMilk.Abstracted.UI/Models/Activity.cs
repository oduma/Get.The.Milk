namespace GetTheMilk.Abstracted.UI.Models
{
    public class Activity
    {
        public string DisplayKeyTrigger { get; set; }

        public string DisplayText { get; set; }

        public string InternalUrl { get; set; }

        public char[] KeyTriggers { get; set; }

        public ActivityType ActivityType { get; set; }

    }
}
