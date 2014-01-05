using GetTheMilk.Abstracted.UI.Controllers;
using MonoCross.Navigation;

namespace GetTheMilk.Abstracted.UI
{
    public class App:MXApplication
    {
        public override void OnAppLoad()
        {
            GameController gameController = new GameController();
            NavigationMap.Add("Game", gameController);

            NavigationMap.Add("Story/{PageId}", new StoryController());
            NewGameController newGameController= new NewGameController();
            NavigationMap.Add("NewGame",newGameController);
            NavigationMap.Add("NewGame/{Action}",newGameController);
            GamePlayController gamePlayeController = new GamePlayController();
            NavigationMap.Add("GamePlay",gamePlayeController);
            NavigationMap.Add("GamePlay/{LevelNo}/{Action}",gamePlayeController);
            //NavigationMap.Add("Activity/{Action}", gameController);
            //NavigationMap.Add("Activity/{SeriesId}/{Action}", gameController);
            //NavigationMap.Add("Activity/{WeekId},{DayId}/{Action}", gameController);
            //NavigationMap.Add("Weeks", new WeeksListController());
            //NavigationMap.Add("Weeks/{WeekId}", new WeekController());
            //NavigationMap.Add("Day/{WeekId},{DayId}", new DayController());
            NavigateOnLoad = "Game";

        }
    }
}
