using System.Collections.Generic;
using GetTheMilk;
using GetTheMilk.Actions;
using GetTheMilk.UI.Translators;
using GetTheMilk.UI.Translators.MovementResultTemplates;
using NUnit.Framework;
using Sciendo.Common.Serialization;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class LoaderTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForLoader), "TestCasesForLoader")]
        public int LoadDefaultMessages(string directory)
        {
            var loader= new ActionResultsTemplatesLoader();
            var templates = loader.LoadActionResultsTemplates(directory);
            Assert.IsNotNull(templates);
            return templates.Count;
        }

        [Test]
        public void LoadingGameGenericTemplates()
        {
            Game myGame = Game.CreateGameInstance();
            Assert.IsNotNull(myGame.MessagesFor);
            Assert.AreEqual(5,myGame.MessagesFor.Count);
        }

        [Test]
        [Ignore("This is not a test")]
        public void SaveDefaultMessagesTemplates()
        {
            var messagesFor = new List<MessagesFor>
                                                {
                                                    new MessagesFor
                                                        {
                                                            ResultType = ActionResultType.Ok,
                                                            Messages =
                                                                new List<Message>
                                                                    {
                                                                        new Message
                                                                            {
                                                                                Id = new Walk().Name.Infinitive,
                                                                                Value = "{0} {1} {2}."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new Run().Name.Infinitive,
                                                                                Value = "{0} {1} {2}."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new EnterLevel().Name.Infinitive,
                                                                                Value = "{0} {1} level {2}."
                                                                            }
                                                                    }
                                                        },
                                                    new MessagesFor
                                                        {
                                                            ResultType = ActionResultType.OriginNotOnTheMap,
                                                            Messages = new List<Message> {new Message
                                                                            {
                                                                                Id = new Walk().Name.Infinitive,
                                                                                Value = "{0} is completely confused and cannot {1}."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new Run().Name.Infinitive,
                                                                                Value = "{0} is completely confused and cannot {1}."
                                                                            }}
                                                        },
                                                    new MessagesFor
                                                        {
                                                            ResultType = ActionResultType.OutOfTheMap,
                                                            Messages = new List<Message> {new Message
                                                                            {
                                                                                Id = new Walk().Name.Infinitive,
                                                                                Value = "{0} tried to {1} {2} but he hit the end of the world."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new Run().Name.Infinitive,
                                                                                Value = "{0} tried to {1} {2} but he hit the end of the world."
                                                                            }}
                                                        },
                                                    new MessagesFor
                                                        {
                                                            ResultType = ActionResultType.BlockedByObject,
                                                            Messages =
                                                                new List<Message>
                                                                    {
                                                                        new Message
                                                                            {
                                                                                Id = new Walk().Name.Infinitive,
                                                                                Value = "{0}  tried to {1} {2}  but is impossible because of the {3}."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new Run().Name.Infinitive,
                                                                                Value = "{0}  tried to {1} {2}  but is impossible because of the {3}."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new EnterLevel().Name.Infinitive,
                                                                                Value = "{0}  tried to {1} level {2}  but is impossible because of the {3}."
                                                                            }
                                                                    }
                                                        },
                                                    new MessagesFor
                                                        {
                                                            ResultType = ActionResultType.BlockedByCharacter,
                                                            Messages =
                                                                new List<Message>
                                                                    {
                                                                        new Message
                                                                            {
                                                                                Id = new Walk().Name.Infinitive,
                                                                                Value = "{0}  tried to {1} {2} but {3} is on the way."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new Run().Name.Infinitive,
                                                                                Value = "{0}  tried to {1} {2} but {3} is on the way."
                                                                            },
                                                                        new Message
                                                                            {
                                                                                Id = new EnterLevel().Name.Infinitive,
                                                                                Value = "{0}  tried to {1} level {2} but {3} is on the way."
                                                                            }
                                                                    }
                                                        }
                                                };
            Serializer.SerializeToFile<MessagesFor>(messagesFor,"test.xml");
        }

    }
}
