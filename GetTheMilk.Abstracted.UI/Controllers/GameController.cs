using System.Collections.Generic;
using GetTheMilk.Abstracted.UI.Models;
using MonoCross.Navigation;

namespace GetTheMilk.Abstracted.UI.Controllers
{
    public class GameController:MXController<GameViewModel>
    {
        public override string Load(Dictionary<string, string> parameters)
        {
            string perspective = ViewPerspective.Default;
                        // get the action, assumes 
            string action;
            if (!parameters.TryGetValue("Action", out action))
            {
                // set default action if none specified
                action = "GET";
            }

            switch (action)
            {
                case "GET":
                    {
                        Model = new GameViewModel{Name=Game.CreateGameInstance().Name,Description = Game.CreateGameInstance().Description};
                        break;
                    }
            }

            return perspective;
        }
    }
}
