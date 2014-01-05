namespace GetTheMilk.Abstracted.UI.Models
{
    public class BaseActivityViewModel
    {
        public BaseActivityViewModel()
        {
            PagingInfo=new PagingInformation{PageSize=23};
        }
        public string Name { get; set; }

        public Activity[] Activities { get; set; }

        public PagingInformation PagingInfo { get; set; }
    }
}
