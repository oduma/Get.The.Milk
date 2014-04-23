using System;
using System.Collections.Generic;
using System.IO;
using GetTheMilk.UI.Translators.Common;
using GetTheMilk.UI.Translators.MovementResultTemplates;
using GetTheMilk.Utils;
using GetTheMilk.Utils.IO;
using Newtonsoft.Json;

namespace GetTheMilk.Settings
{
    public class GameSettings
    {
        private static readonly GameSettings Instance = new GameSettings();

        public static GameSettings GetInstance()
        {
            if (Instance.MessagesForActionsResult == null || Instance.MovementExtraDataTemplate == null)
            {
                using (
                    Stream fs =
                        Instance.CurrentReadStrategy(Path.Combine(Instance.DefaultPaths.GameData,
                                                                  Instance.DefaultPaths.TemplatesFileName)))
                {
                    var tPackageContent = (new StreamReader(fs)).ReadToEnd();
                    var templatesPackage = JsonConvert.DeserializeObject<TemplatesPackage>(tPackageContent);
                    Instance.MessagesForActionsResult = templatesPackage.MessagesForActionResult;
                    Instance.MovementExtraDataTemplate = templatesPackage.MovementExtraDataTemplate;
                    Instance.ActionTypeMessages = templatesPackage.ActionTypeMessages;
                }
            }
            return Instance;
        }

        public List<Message> ActionTypeMessages  { get; private set; }

        public Func<string, Stream> CurrentReadStrategy { get { return ReadWriteStrategies.UncompressedReader; } }


        public Action<string, string> CurrentWriteStrategy { get { return ReadWriteStrategies.UncompressedWriter; } }

        public List<MessagesForActionResult> MessagesForActionsResult { get; private set; }

        public MovementExtraDataTemplate MovementExtraDataTemplate { get; private set; }

        public bool AllowChoiceOfDefensiveWeapons
        {
            get { return false; }
        }

        public bool FightersRightHanded
        {
            get { return true; }
        }

        public string InteractiveMode { get { return "TextBased"; } }

        public  int MaximumExperience { get { return 1000; } }

        public  int FullDefaultHealth
        {
            get { return 10; }
        }

        public  int DefaultRunDistance
        {
            get { return 3; }
        }

        public  int DefaultWalkDistance   
        {
            get { return 1; }
        }

        public  int DefaultWalletMaxCapacity
        {
            get { return 200; }
        }

        public  int MinimumStartingExperience
        {
            get { return 1; }
        }

        public  int MinimumStartingMoney
        {
            get { return 20; }
        }

        public  int MaximumAvailableBonusPoints
        {
            get { return 100; }
        }

        public  string DefaultNarratorAddressingForPlayer 
        {
            get { return "you"; }
        }

        public  Paths DefaultPaths
        {
            get
            {
                return new Paths();
            }
        }

        public  string TranslatorErrorMessage
        {
            get { return "Blah, blah, blah... the game has lost his ability to communicate with you."; }
        }

        public  string DefaultPlayerName
        {
            get { return "Player"; }
        }

        public  int DefaultRange
        {
            get { return 1; }
        }

        public  string DefaultGameName
        {
            get { return "Get the milk"; }
        }

        public  string Description
        {
            get { return "Some description here."; }
        }

        public int DefaulMaximumCapacity
        {
            get { return 20; }
        }

        public string GameFinishingMessage
        {
            get { return "The Game has finished."; }
        }

        public  int GetRandomMoneyBoost()
        {
            return Randomizer.GetRandom(5, 20);
        }

        public  int GetRandomExperienceBoost()
        {
            return Randomizer.GetRandom(10, 50);
        }
    }
}
