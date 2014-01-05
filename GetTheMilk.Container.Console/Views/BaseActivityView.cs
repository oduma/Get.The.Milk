using System;
using System.Linq;
using GetTheMilk.Abstracted.UI.Models;
using MonoCross.Console;
using MonoCross.Navigation;

namespace GetTheMilk.Container.Console.Views
{
    public class BaseActivityView<T>:MXConsoleView<T> where T:BaseActivityViewModel
    {
        protected void RenderHeader()
        {
            System.Console.WriteLine(Model.Name);
        }

        protected void RenderActivities()
        {
            
            foreach(var activity in Model.Activities)
            {
                if (activity.ActivityType == ActivityType.Normal ||IsNotTheLastPage(activity))
                {
                    System.Console.Write(" | ");
                    System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                    System.Console.Write("({0})", activity.DisplayKeyTrigger);
                    System.Console.ResetColor();
                    System.Console.Write("{0}", activity.DisplayText);
                }
            }
        }

        private bool IsNotTheLastPage(Activity activity)
        {
            if (activity.ActivityType == ActivityType.Normal)
                return true;
            if (Model.PagingInfo.TotalNumberOfRecords > (Model.PagingInfo.CurrentPage + 1) * Model.PagingInfo.PageSize)
                return true;
            return false;
        }

        protected void CollectInput()
        {
            do
            {
                var input = System.Console.ReadKey();

                if (input.Key == ConsoleKey.Escape)
                {
                    this.Back();
                    return;
                }
                var selectActivity = Model.Activities.FirstOrDefault(a => a.KeyTriggers.Contains(input.KeyChar));
                if(selectActivity!=null)
                {
                    this.Navigate(selectActivity.ActivityType == ActivityType.Paging
                                      ? string.Format(selectActivity.InternalUrl, Model.PagingInfo.CurrentPage + 1)
                                      : selectActivity.InternalUrl);
                }
            } while (true);

        }
    }
}
