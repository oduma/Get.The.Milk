using System;
using System.Collections.Generic;
using GetTheMilk.Abstracted.UI.Models;
using MonoCross.Navigation;

namespace GetTheMilk.Abstracted.UI.Controllers
{
    public class StoryController : MXController<StoryViewModel>
    {
        public override string Load(Dictionary<string, string> parameters)
        {
            string perspective = ViewPerspective.Default;
            // get the action, assumes 
            int pageId;
            string pId = null;
            parameters.TryGetValue("PageId", out pId);
            int.TryParse(pId, out pageId);

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
                        Model = new StoryViewModel
                                    {
                                        Name = Game.CreateGameInstance().Name,
                                        FullStoryLines =
                                            Game.CreateGameInstance().FullDescription.Split(new string[] {"\r\n"},
                                                                                            StringSplitOptions.
                                                                                                RemoveEmptyEntries),
                                       
                                    };
                        Model.PagingInfo.CurrentPage = pageId;
                        break;
                    }
            }

            return perspective;

        }
    }
}
