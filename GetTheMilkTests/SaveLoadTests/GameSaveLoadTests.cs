using System.Collections.Generic;
using System.IO;
using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.Settings;
using GetTheMilk.UI.Translators.Common;
using GetTheMilk.UI.Translators.MovementResultTemplates;
using NUnit.Framework;
using Newtonsoft.Json;
using GetTheMilk.Navigation;

namespace GetTheMilkTests.SaveLoadTests
{
    [TestFixture]
    public class GameSaveLoadTests
    {
        [Test]
        public void LoadGameSettings()
        {
            var gameSettings = GameSettings.GetInstance();
            Assert.AreEqual(gameSettings.DefaultGameName, "Get the milk");
            Assert.AreEqual(gameSettings.Description,"Some description here.");
            Assert.AreEqual(gameSettings.MessagesForActionsResult.Count,6);
            Assert.AreEqual(gameSettings.ActionTypeMessages.Count,9);
        }


        [Test]
        public void LoadANewGame()
        {
            Game game = Game.CreateGameInstance();
            Assert.IsNotNull(game);
            Assert.IsNotNull(game.Player);
            Assert.IsNotNull(game.CurrentLevel);
           
        }

        [Test]
        public void SaveAndLoadGameToUncompressedFile()
        {
            Game game = Game.CreateGameInstance();
            Assert.IsNotNull(game);
            Assert.IsNotNull(game.Player);
            Assert.IsNotNull(game.CurrentLevel);
            game.Player.CellNumber = 0;
            game.Player.EnterLevel(game.CurrentLevel);
            game.Player.SetPlayerName("my own name");
            Walk walk = new Walk();
            walk.Direction = Direction.South;
            walk.ActiveCharacter = game.Player;
            walk.CurrentMap = game.CurrentLevel.CurrentMap;
            walk.Perform();
            game.Save("gamesavedtest.gsu");
            Assert.True(File.Exists(@"Saved\gamesavedtest.gsu"));

            var gameLoaded = Game.Load(@"Saved\gamesavedtest.gsu");
            Assert.IsNotNull(gameLoaded);
            Assert.IsNotNull(gameLoaded.Player);
            Assert.IsNotNull(gameLoaded.CurrentLevel);
            Assert.AreEqual(3,gameLoaded.Player.CellNumber); 
            Assert.AreEqual("my own name",gameLoaded.Player.Name.Main);

        }

        [Test]
        public void LoadGameFromUncompressedFile()
        {
            var gameLoaded = Game.Load(@"Saved\previouslySavedgame.gsu");
            Assert.IsNotNull(gameLoaded);
            Assert.IsNotNull(gameLoaded.Player);
            Assert.IsNotNull(gameLoaded.CurrentLevel);
            Assert.AreEqual(3, gameLoaded.Player.CellNumber);
            Assert.AreEqual("my own name", gameLoaded.Player.Name.Main);

        }

        [Test]
        public void SaveTemplates()
        {
            TemplatesPackage templatesPackage = new TemplatesPackage
                                                    {
                                                        MessagesForActionResult = new List<MessagesForActionResult>
                                                                          {
                                                                              new MessagesForActionResult
                                                                                  {
                                                                                      ResultType = ActionResultType.Ok,
                                                                                      Messages = new List<Message>
                                                                                                     {
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Walk",
                                                                                                                 Value =
                                                                                                                     "{0} {1} {3}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Run",
                                                                                                                 Value =
                                                                                                                     "{0} {1} {3}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Enter",
                                                                                                                 Value =
                                                                                                                     "{0} {1} level {4}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Pick",
                                                                                                                 Value =
                                                                                                                     "{0} {1} {3}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Kick",
                                                                                                                 Value =
                                                                                                                     "{0} {1} {3}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Keep",
                                                                                                                 Value =
                                                                                                                     "{0} decided to {2} {3}."
                                                                                                             }


                                                                                                     }
                                                                                  },
                                                                              new MessagesForActionResult
                                                                                  {
                                                                                      ResultType =
                                                                                          ActionResultType.
                                                                                          OriginNotOnTheMap,
                                                                                      Messages = new List<Message>
                                                                                                     {
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Walk",
                                                                                                                 Value =
                                                                                                                     "Error {0} cannot {2}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Run",
                                                                                                                 Value =
                                                                                                                     "Error {0} cannot {2}."
                                                                                                             }
                                                                                                     }
                                                                                  },
                                                                              new MessagesForActionResult
                                                                                  {
                                                                                      ResultType =
                                                                                          ActionResultType.OutOfTheMap,
                                                                                      Messages = new List<Message>
                                                                                                     {
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Walk",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but the world has limits."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Run",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but the world has limits."
                                                                                                             }
                                                                                                     }
                                                                                  },
                                                                              new MessagesForActionResult
                                                                                  {
                                                                                      ResultType =
                                                                                          ActionResultType.
                                                                                          Blocked,
                                                                                      Messages = new List<Message>
                                                                                                     {
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Walk",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but is impossible, blocked by a {5}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Run",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but is impossible, blocked by a {5}."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Enter",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} level {4} but is impossible, blocked by a {5}."
                                                                                                             }
                                                                                                     }
                                                                                  },
                                                                              new MessagesForActionResult
                                                                                  {
                                                                                      ResultType =
                                                                                          ActionResultType.
                                                                                          Blocked,
                                                                                      Messages = new List<Message>
                                                                                                     {
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Walk",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but {6} is on the way."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Run",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but {6} is on the way."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Enter",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} level {4} but {6} is on the way."
                                                                                                             }
                                                                                                     }
                                                                                  },
                                                                              new MessagesForActionResult
                                                                                  {
                                                                                      ResultType =
                                                                                          ActionResultType.NotOk,
                                                                                      Messages = new List<Message>
                                                                                                     {
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Pick",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but couldn't."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Kick",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but couldn't."
                                                                                                             },
                                                                                                         new Message
                                                                                                             {
                                                                                                                 Id =
                                                                                                                     "To Keep",
                                                                                                                 Value =
                                                                                                                     "{0} tried to {2} {3} but couldn't."
                                                                                                             }
                                                                                                     }
                                                                                  }

                                                                          },
                                                        MovementExtraDataTemplate = new MovementExtraDataTemplate
                                                                                        {
                                                                                            MessageForObjectsInCell =
                                                                                                "You see in front of you {0}.",
                                                                                            MessageForObjectsInRange =
                                                                                                "Looking {0} {1}."
                                                                                        }
                                                    };
            var result = JsonConvert.SerializeObject(templatesPackage);

            var actual = JsonConvert.DeserializeObject<TemplatesPackage>(result);

            Assert.IsNotNull(actual);
        }

    }
}
