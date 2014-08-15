using System;
using System.Collections.Generic;
using System.IO;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
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
            if (Instance.ActionTemplateMessages == null)
            {
                using (
                    Stream fs =
                        Instance.CurrentReadStrategy(Path.Combine(Instance.DefaultPaths.GameData,
                                                                  Instance.DefaultPaths.TemplatesFileName)))
                {
                    var tPackageContent = (new StreamReader(fs)).ReadToEnd();
                    Instance.ActionTemplateMessages = JsonConvert.DeserializeObject<List<Message>>(tPackageContent);
                }
            }
            return Instance;
        }

        public List<Message> ActionTemplateMessages  { get; private set; }

        public Func<string, Stream> CurrentReadStrategy { get { return ReadWriteStrategies.UncompressedReader; } }


        public Action<string, string> CurrentWriteStrategy { get { return ReadWriteStrategies.UncompressedWriter; } }

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

        public List<BaseActionTemplate> AllCharactersActions
        {
            get
            {
                return new List<BaseActionTemplate>
                           {

                                   new MovementActionTemplate
                                   {
                                       PerformerType=typeof( WalkActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "Walk",
                                                      Past = "walked",
                                                      Present = "walk"
                                                  },
                                       DefaultDistance = 1
                                   },
                               new MovementActionTemplate
                                   {
                                       PerformerType=typeof( RunActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "Run",
                                                      Past = "ran",
                                                      Present = "run"
                                                  },
                                       DefaultDistance = 3
                                   },
                               new MovementActionTemplate
                                   {
                                       PerformerType=typeof( TeleportActionPerformer),
                                       Name =
                                           new Verb
                                               {
                                                   UniqueId = "Teleport",
                                                   Past = "teleported",
                                                   Present = "teleport"
                                               }
                                   },
                                new ObjectTransferActionTemplate
                                    {
                                        PerformerType=typeof(TakeFromActionPerformer),
                                        Name=
                                            new Verb
                                                {
                                                    UniqueId="TakeFrom",
                                                    Past="took",
                                                    Present="take"
                                                }
                                    }

                           };
            }
        }
        public List<BaseActionTemplate> StandardPlayerActions
        {
            get
            {
                var result=new List<BaseActionTemplate>
                           {
                               new MovementActionTemplate
                                   {
                                       PerformerType=typeof( TeleportActionPerformer),
                                       Name =
                                           new Verb
                                               {
                                                   UniqueId = "EnterLevel",
                                                   Past = "entered level",
                                                   Present = "enter level"
                                               }
                                   },
                               new ExposeInventoryActionTemplate
                                   {
                                       PerformerType=typeof( ExposeInventoryActionTemplatePerformer),
                                       Name =
                                           new Verb
                                               {
                                                   UniqueId = "ExposeInventory",
                                                   Past = "exposed inventory",
                                                   Present = "expose inventory"
                                               },
                                       StartingAction = true,
                                       SelfInventory = true,
                                       FinishingAction = ExposeInventoryFinishingAction.CloseInventory
                                   },
                               new NoObjectActionTemplate
                                   {
                                       PerformerType=typeof( NoObjectActionTemplatePerformer),
                                       Name =
                                           new Verb
                                               {
                                                   UniqueId = "CloseInventory",
                                                   Past = "closed inventory",
                                                   Present = "close inventory"
                                               }
                                   }
                           };
                result.AddRange(AllCharactersActions);
                return result;
            }

        }

        public List<BaseActionTemplate> SelfInventoryActions
        {
            get
            {
                return new List<BaseActionTemplate>
                           {

                               new OneObjectActionTemplate
                                   {
                                       PerformerType=typeof( SelectAttackWeaponActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "SelectAttackWeapon",
                                                      Past = "selected attack weapon",
                                                      Present = "select attack weapon"
                                                  }
                                   },
                               new OneObjectActionTemplate
                                   {
                                       PerformerType=typeof( SelectDefenseWeaponActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "SelectDefenseWeapon",
                                                      Past = "selected defense weapon",
                                                      Present = "select defense weapon"
                                                  }
                                   },
                               new ObjectTransferActionTemplate
                                   {
                                       PerformerType=typeof( ObjectTransferFromActiveCharacterPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "Discard",
                                                      Past = "dsicarded",
                                                      Present = "discard"
                                                  }
                                   }
                           };
            }
        } 
        public int MinimumAttackPower
        {
            get { return 8; }
        }

        public int MinimumDefensePower
        {
            get { return 1; }
        }

        public int MaximumDurability
        {
            get { return int.MaxValue; }
        }

        public IEnumerable<BaseActionTemplate> FriendlyContentActions
        {
            get
            {
                return new List<BaseActionTemplate>
                           {

                               new ObjectTransferActionTemplate
                                   {
                                       PerformerType=typeof( BuyActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "Buy",
                                                      Past = "bought",
                                                      Present = "buy"
                                                  }
                                   },
                               new ObjectTransferActionTemplate
                                   {
                                       PerformerType=typeof( TakeFromActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "TakeFrom",
                                                      Past = "took from",
                                                      Present = "take from"
                                                  }
                                   }
                           };
            }
        }

        public IEnumerable<BaseActionTemplate> FoeContentActions
        {
            get
            {
                return new List<BaseActionTemplate>
                           {

                               new ObjectTransferActionTemplate
                                   {
                                       PerformerType=typeof( TakeFromActionPerformer),
                                       Name = new Verb
                                                  {
                                                      UniqueId = "TakeFrom",
                                                      Past = "took from",
                                                      Present = "take from"
                                                  }
                                   }
                           };
            }
        }

        public  int GetRandomMoneyBoost()
        {
            return Randomizer.GetRandom(5, 20);
        }

        public  int GetRandomExperienceBoost()
        {
            return Randomizer.GetRandom(10, 50);
        }

        public string MessageForObjectsInRange { get { return "Looking {0} {1}."; } }
    }
}
