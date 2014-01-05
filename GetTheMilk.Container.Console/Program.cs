using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Abstracted.UI.Models;
using MonoCross.Console;
using MonoCross.Navigation;

namespace GetTheMilk.Container.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize container
            MXConsoleContainer.Initialize(new GetTheMilk.Abstracted.UI.App());

            // game view
            MXConsoleContainer.AddView<GameViewModel>(new Views.GameView(), ViewPerspective.Default);

            // story view
            MXConsoleContainer.AddView<StoryViewModel>(new Views.StoryView(), ViewPerspective.Default);

            //new game view
            MXConsoleContainer.AddView<GameStartViewModel>(new Views.GameStartView(), ViewPerspective.Default);

            //new game view
            MXConsoleContainer.AddView<GameStartViewModel>(new Views.GameStartViewValidation(),"PHASE2");

            // game play view
            MXConsoleContainer.AddView<GamePlayViewModel>(new Views.GamePlayView(), ViewPerspective.Default);

            ////weeks list view
            //MXConsoleContainer.AddView<List<WeekSummary>>(new Views.WeeksListView(), ViewPerspective.Default);

            ////week view
            //MXConsoleContainer.AddView<WeekDetail>(new Views.WeekView(), ViewPerspective.Default);

            ////day view
            //MXConsoleContainer.AddView<Day>(new Views.DayView(), ViewPerspective.Default);

            // navigate to first view
            MXConsoleContainer.Navigate(MXContainer.Instance.App.NavigateOnLoad);


        }
    }
}
