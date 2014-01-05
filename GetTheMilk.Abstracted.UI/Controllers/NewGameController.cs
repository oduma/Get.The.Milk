using System;
using System.Collections.Generic;
using GetTheMilk.Abstracted.UI.Models;
using GetTheMilk.Characters;
using GetTheMilk.Settings;
using MonoCross.Navigation;

namespace GetTheMilk.Abstracted.UI.Controllers
{
    public class NewGameController : MXController<GameStartViewModel>
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
                        Model = new GameStartViewModel
                        {
                            Name = Game.CreateGameInstance().Name,
                        };
                        break;
                    }
                case "PHASE2":
                    {
                        if(Model.CharacterSetup.MoneyOrExperience=="M")
                        {
                            Model.CharacterSetup.StartingExperience = GameSettings.MinimumStartingExperience;
                            Model.CharacterSetup.StartingMoney = GameSettings.MinimumStartingMoney + GameSettings.GetRandomMoneyBoost();
                        }
                        else if (Model.CharacterSetup.MoneyOrExperience == "E")
                        {
                            Model.CharacterSetup.StartingMoney = GameSettings.MinimumStartingMoney;
                            Model.CharacterSetup.StartingExperience = GameSettings.MinimumStartingExperience +
                                                                      GameSettings.GetRandomExperienceBoost();
                        }
                        Model.Activities[0] = new Activity
                                                  {
                                                      DisplayKeyTrigger = "S",
                                                      DisplayText = "ave and start",
                                                      InternalUrl = "NewGame/SAVE",
                                                      KeyTriggers = new char[] {'s', 'S'}
                                                  };
                        perspective = "PHASE2";
                        break;
                    }
                case "SAVE":
                    {
                        Player.Destroy();
                        Player player = Player.GetNewInstance();
                        player.Name = Model.CharacterSetup.Name;
                        player.Walet.CurrentCapacity = Model.CharacterSetup.StartingMoney;
                        player.Experience = Model.CharacterSetup.StartingExperience;
                        MXContainer.Instance.Redirect("GamePlay");
                        break;
                    }
            }

            return perspective;


        }
    }
}
