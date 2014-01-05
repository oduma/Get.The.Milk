using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Abstracted.UI.Models;
using MonoCross.Console;
using MonoCross.Navigation;

namespace GetTheMilk.Container.Console.Views
{
    public class StoryView:BaseActivityView<StoryViewModel>
    {
        public override void Render()
        {
            System.Console.Clear();
            RenderHeader();
            for (int i = Model.PagingInfo.CurrentPage * Model.PagingInfo.PageSize; i < (Model.PagingInfo.CurrentPage + 1)*Model.PagingInfo.PageSize;i++ )
            {
                if(i>=Model.PagingInfo.TotalNumberOfRecords)
                    System.Console.WriteLine();
                else
                    System.Console.WriteLine(Model.FullStoryLines[i]);
            }
            RenderActivities();
            CollectInput();
        }

    }
}
