using System.Collections.Generic;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.NewActions.Tests
{
    public static class TestHelper
    {
        public static Map GenerateSmallMap()
        {
            return new Map
                       {
                           Cells =
                               new Cell[9]
                                   {
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = -1,
                                               NorthCell = -1,
                                               Number = 0,
                                               SouthCell = 3,
                                               TopCell = -1,
                                               EastCell = 1
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = 0,
                                               NorthCell = -1,
                                               Number = 1,
                                               SouthCell = 4,
                                               TopCell = -1,
                                               EastCell = 2,
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = 1,
                                               NorthCell = -1,
                                               Number = 2,
                                               SouthCell = 5,
                                               TopCell = -1,
                                               EastCell = -1
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = -1,
                                               NorthCell = 0,
                                               Number = 3,
                                               SouthCell = 6,
                                               TopCell = -1,
                                               EastCell = 4
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = 3,
                                               NorthCell = 1,
                                               Number = 4,
                                               SouthCell = 7,
                                               TopCell = -1,
                                               EastCell = 5
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = 4,
                                               NorthCell = 2,
                                               Number = 5,
                                               SouthCell = 8,
                                               TopCell = -1,
                                               EastCell = -1
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = -1,
                                               NorthCell = 3,
                                               Number = 6,
                                               SouthCell = -1,
                                               TopCell = -1,
                                               EastCell = 7
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = 6,
                                               NorthCell = 4,
                                               Number = 7,
                                               SouthCell = -1,
                                               TopCell = -1,
                                               EastCell = 8
                                           },
                                       new Cell
                                           {
                                               BottomCell = -1,
                                               WestCell = 7,
                                               NorthCell = 5,
                                               Number = 8,
                                               SouthCell = -1,
                                               TopCell = -1,
                                               EastCell = -1
                                           }
                                   }
                       };
        }

        public static Level GenerateALevel()
        {
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("NPCFriendly");
            var objEAction = factory.CreateObjectAction("NPCFoe");

            var levelCharacters = new CharacterCollection();

            Inventory skInventory = new Inventory { InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 200 };
            skInventory.Add(new Weapon
            {
                Durability = 1000,
                AttackPower = 3,
                DefensePower = 1,
                BlockMovement = false,
                WeaponTypes = new WeaponType[] { WeaponType.Attack, WeaponType.Deffense },
                BuyPrice = 5,
                SellPrice = 2,
                Name = new Noun { Main = "Knife", Narrator = "the knife" },
                CloseUpMessage = "It is a small but very sharp knife.",
                ObjectTypeId = "Weapon"
            },
                            new Tool
                            {
                                BlockMovement = false,
                                BuyPrice = 4,
                                SellPrice = 2,
                                Name = new Noun { Main = "Can Opener", Narrator = "the can opener" },
                                CloseUpMessage = "In the grass right in front there is a can opener.",
                                ObjectTypeId = "CanOpener"
                            });
            var skInteractionRules = new SortedList<string, Interaction[]>
                                         {
                                             {
                                                 GenericInteractionRulesKeys.CharacterSpecific, new Interaction[]
                                                                                                    {
                                                                                                        new Interaction
                                                                                                            {
                                                                                                                Action =
                                                                                                                    new TwoCharactersActionTemplate
                                                                                                                        {
                CurrentPerformer = new TwoCharactersActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId
                                                                                                                                            =
                                                                                                                                            "Meet",
                                                                                                                                        Past
                                                                                                                                            =
                                                                                                                                            "met",
                                                                                                                                        Present
                                                                                                                                            =
                                                                                                                                            "meet"
                                                                                                                                    },
                                                                                                                            StartingAction
                                                                                                                                =
                                                                                                                                true
                                                                                                                        },
                                                                                                                Reaction
                                                                                                                    =
                                                                                                                    new TwoCharactersActionTemplate
                                                                                                                        {

                                                                                                                            Message
                                                                                                                                =
                                                                                                                                "How are you? Beautifull day out there better buy something!"
                                                                                                                        }
                                                                                                            },
                                                                                                        new Interaction
                                                                                                            {
                                                                                                                Action =
                                                                                                                    new TwoCharactersActionTemplate
                                                                                                                        {
                                                                                                                            Message
                                                                                                                                =
                                                                                                                                "Yes",
                                                                                                                            FinishTheInteractionOnExecution
                                                                                                                                =
                                                                                                                                true
                                                                                                                        },
                                                                                                                Reaction
                                                                                                                    =
                                                                                                                    new ExposeInventoryActionTemplate
                                                                                                                        {
                                                                                                                            FinishActionType
                                                                                                                                =
                                                                                                                                "CloseInventory"
                                                                                                                        }
                                                                                                            },
                                                                                                        new Interaction
                                                                                                            {
                                                                                                                Action =
                                                                                                                    new TwoCharactersActionTemplate
                                                                                                                        {
                                                                                                                            Message
                                                                                                                                =
                                                                                                                                "No",
                                                                                                                            FinishTheInteractionOnExecution
                                                                                                                                =
                                                                                                                                true
                                                                                                                        },
                                                                                                                Reaction
                                                                                                                    =
                                                                                                                    new TwoCharactersActionTemplate
                                                                                                                        {
                                                                                                                            Message
                                                                                                                                =
                                                                                                                                "Why oh Why!?"
                                                                                                                        }
                                                                                                            }
                                                                                                    }
                                             },
                                             {
                                                 GenericInteractionRulesKeys.PlayerResponses, new Interaction[]
                                                                                                  {
                                                                                                      new Interaction
                                                                                                          {
                                                                                                              Action =
                                                                                                                  new TwoCharactersActionTemplate
                                                                                                                      {
                                                                                                                          Message
                                                                                                                              =
                                                                                                                              "How are you? Beautifull day out there better buy something!"
                                                                                                                      },
                                                                                                              Reaction =
                                                                                                                  new TwoCharactersActionTemplate
                                                                                                                      {
                                                                                                                          Message
                                                                                                                              =
                                                                                                                              "Yes",
                                                                                                                          FinishTheInteractionOnExecution
                                                                                                                              =
                                                                                                                              true
                                                                                                                      }
                                                                                                          },
                                                                                                      new Interaction
                                                                                                          {
                                                                                                              Action =
                                                                                                                  new TwoCharactersActionTemplate
                                                                                                                      {
                                                                                                                          Message
                                                                                                                              =
                                                                                                                              "How are you? Beautifull day out there better buy something!"
                                                                                                                      },
                                                                                                              Reaction =
                                                                                                                  new TwoCharactersActionTemplate
                                                                                                                      {
                                                                                                                          Message
                                                                                                                              =
                                                                                                                              "No",
                                                                                                                          FinishTheInteractionOnExecution
                                                                                                                              =
                                                                                                                              true
                                                                                                                      }
                                                                                                          }

                                                                                                  }
                                             },
                                             {
                                                 GenericInteractionRulesKeys.AnyCharacterResponses, new Interaction[]
                                                                                                     {
                                                                                                         new Interaction
                                                                                                             {
                                                                                                                 Action
                                                                                                                     =
                                                                                                                     new TwoCharactersActionTemplate
                                                                                                                         {
                                                                                                                             Message
                                                                                                                                 =
                                                                                                                                 "Please give me a tool!"
                                                                                                                         },
                                                                                                                 Reaction
                                                                                                                     =
                                                                                                                     new ObjectTransferActionTemplate
                                                                                                                         {
                CurrentPerformer = new GiveToActionPerformer(),
                Name = new Verb
                {
                    UniqueId
                                                                                                                                             =
                                                                                                                                             "GiveTo",
                                                                                                                                         Past
                                                                                                                                             =
                                                                                                                                             "gave to",
                                                                                                                                         Present
                                                                                                                                             =
                                                                                                                                             "give to"
                                                                                                                                     },
                                                                                                                             FinishTheInteractionOnExecution
                                                                                                                                 =
                                                                                                                                 true
                                                                                                                         }
                                                                                                             },
                                                                                                                                                                                                                      new Interaction
                                                                                                             {
                                                                                                                 Action
                                                                                                                     =
                                                                                                                     new TwoCharactersActionTemplate
                                                                                                                         {
                                                                                                                             Message
                                                                                                                                 =
                                                                                                                                 "Please give me a tool!"
                                                                                                                         },
                                                                                                                 Reaction
                                                                                                                     =
                                                                                                                     new ObjectTransferActionTemplate
                                                                                                                         {
                CurrentPerformer = new SellActionPerformer(),
                Name = new Verb
                {
                    UniqueId
                                                                                                                                             =
                                                                                                                                             "Sell",
                                                                                                                                         Past
                                                                                                                                             =
                                                                                                                                             "sold",
                                                                                                                                         Present
                                                                                                                                             =
                                                                                                                                             "sell"
                                                                                                                                     },
                                                                                                                             FinishTheInteractionOnExecution
                                                                                                                                 =
                                                                                                                                 true
                                                                                                                         }
                                                                                                             }
                                                                                                     }
                                             }

                                         };
            var skCharacter = new Character
            {
                CellNumber = 2,
                Walet = new Walet { MaxCapacity = 1000, CurrentCapacity = 100 },
                BlockMovement = true,
                Name = new Noun { Main = "John the Shop Keeper", Narrator = "John the Shop Keeper" },
                Inventory = skInventory,
                ObjectTypeId = "NPCFriendly",
                CloseUpMessage = "there is a shop keeper"
            };
            skCharacter.Interactions = skInteractionRules;
            skCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            skCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;


            Inventory fInventory = new Inventory { InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 20 };
            fInventory.Add(new Weapon
            {
                Durability = 1000,
                AttackPower = 3,
                DefensePower = 1,
                BlockMovement = false,
                WeaponTypes = new WeaponType[] { WeaponType.Attack, WeaponType.Deffense },
                BuyPrice = 50,
                SellPrice = 2,
                Name = new Noun { Main = "Knife", Narrator = "the knife" },
                CloseUpMessage = "It is a small but very sharp knife.",
                ObjectTypeId = "Weapon"
            });
            SortedList<string, Interaction[]> fInteractionRules = new SortedList<string, Interaction[]>();
            fInteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific, new Interaction[]
                                                                                     {
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{
                CurrentPerformer = new TwoCharactersActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Meet",Past="met",Present="meet"},StartingAction=true},
                                                                                                 Reaction = new TwoCharactersActionTemplate{
                CurrentPerformer = new AttackActionPerformer(),
                Name = new Verb
                {
                    UniqueId="Attack"}}
                                                                                             },
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{
                CurrentPerformer = new TwoCharactersActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Quit"}},
                                                                                                 Reaction = new TwoCharactersActionTemplate{
                CurrentPerformer = new AttackActionPerformer(),
                Name = new Verb
                {
                    UniqueId="Attack"}}
                                                                                             },
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{
                CurrentPerformer = new TwoCharactersActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Quit"}},
                                                                                                 Reaction = new TwoCharactersActionTemplate{
                CurrentPerformer = new AcceptQuitActionPerformer(),
                Name = new Verb
                {
                    UniqueId="AcceptQuit"},FinishTheInteractionOnExecution=true}
                                                                                             },
                                                                                         new Interaction
                                                                                         {
                                                                                             Action= new TwoCharactersActionTemplate{
                CurrentPerformer = new InitiateHostilitiesActionPerformer(),
                Name = new Verb
                {
                    UniqueId="InitiateHostilities",Past="attacked",Present="attack"},StartingAction=true},
                                                                                             Reaction=new TwoCharactersActionTemplate{
                CurrentPerformer = new InitiateHostilitiesActionPerformer(),
                Name = new Verb
                {
                    UniqueId="InitiateHostilities"}}
                                                                                         }
                                                                                     });
            fInteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                new Interaction[]
                {
                    new Interaction
                    {
                        Action = new TwoCharactersActionTemplate{
                CurrentPerformer = new TwoCharactersActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Quit"}},
                        Reaction = new TwoCharactersActionTemplate{
                CurrentPerformer = new AttackActionPerformer(),
                Name = new Verb
                {
                    UniqueId="Attack"}}
                    },
                    new Interaction
                    {
                        Action = new TwoCharactersActionTemplate{
                CurrentPerformer = new TwoCharactersActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Quit"}},
                        Reaction = new TwoCharactersActionTemplate{
                CurrentPerformer = new AcceptQuitActionPerformer(),
                Name = new Verb
                {
                    UniqueId="AcceptQuit"},FinishTheInteractionOnExecution=true}
                    },
                    new Interaction
                    {
                        Action = new TwoCharactersActionTemplate{
                CurrentPerformer = new InitiateHostilitiesActionPerformer(),
                Name = new Verb
                {
                    UniqueId="InitiateHostilities"}},
                        Reaction = new ExposeInventoryActionTemplate
                            {FinishActionType = "Attack",SelfInventory=true}

                    }
                });

            var fCharacter = new Character
            {
                CellNumber = 8,
                BlockMovement = true,
                Walet = new Walet { MaxCapacity = 2000, CurrentCapacity = 400 },
                Name = new Noun { Main = "Baddie", Narrator = "the Baddie" },
                Inventory = fInventory,
                Interactions = fInteractionRules,
                ObjectTypeId = "NPCFoe",
                CloseUpMessage = "there is a fierce warrior"
            };

            fCharacter.AllowsTemplateAction = objEAction.AllowsTemplateAction;
            fCharacter.AllowsIndirectTemplateAction = objEAction.AllowsIndirectTemplateAction;

            levelCharacters.Add(skCharacter, fCharacter);

            var levelInventory = new Inventory { InventoryType = InventoryType.LevelInventory, MaximumCapacity = 2000 };
            levelInventory.Add(new Tool
                                   {
                                       CellNumber = 3,
                                       CloseUpMessage =
                                           "the Red Key of Kirna and you wonder how did you knew what it was.",
                                       Name = new Noun {Main = "Red Key", Narrator = "the Red Key"},
                                       ObjectTypeId = "Key",
                                       Interactions =
                                           new SortedList<string, Interaction[]>
                                               {
                                                   {
                                                       GenericInteractionRulesKeys.AnyCharacter,
                                                       new Interaction[]
                                                           {
                                                               new Interaction
                                                                   {
                                                                       Action =
                                                                           new ObjectTransferActionTemplate
                                                                               {
                CurrentPerformer = new ObjectTransferToActiveCharacterPerformer(),
                Name = new Verb
                {
                    UniqueId = "Keep",
                                                                                               Past = "kept",
                                                                                               Present = "keep"
                                                                                           },
                                                                                   StartingAction = true
                                                                               }
                                                                   }
                                                           }
                                                   }
                                               }
                                   },
                               new Tool
                                   {
                                       CellNumber = 4,
                                       CloseUpMessage = "Upclose you realise it is a door and it seems to be locked.",
                                       Name = new Noun {Main = "Red Door", Narrator = "red door"},
                                       BlockMovement = true,
                                       ObjectTypeId = "RedDoor",
                                       Interactions =
                                           new SortedList<string, Interaction[]>
                                               {
                                                   {
                                                       GenericInteractionRulesKeys.AnyCharacter,
                                                       new Interaction[]
                                                           {
                                                               new Interaction
                                                                   {
                                                                       Action =
                                                                           new ObjectUseOnObjectActionTemplate
                                                                               {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId = "Open",
                                                                                               Past = "opened",
                                                                                               Present = "open"
                                                                                           },
                                                                                   StartingAction = true,
                                                                                   DestroyActiveObject = true,
                                                                                   DestroyTargetObject = true,
                                                                                   ChanceOfSuccess =
                                                                                       ChanceOfSuccess.Full,
                                                                                   PercentOfHealthFailurePenalty = 0
                                                                               }
                                                                   }
                                                           }
                                                   }
                                               }

                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 1,
                                       CloseUpMessage = "The wall is solid stone, unpassable for sure.",
                                       Name = new Noun {Main = "Wall", Narrator = "wall"},
                                       BlockMovement = true,
                                       ObjectTypeId = "Decor"

                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 7,
                                       CloseUpMessage = "The wall is solid stone, unpassable for sure.",
                                       Name = new Noun {Main = "Wall", Narrator = "wall"},
                                       BlockMovement = true,
                                       ObjectTypeId = "Decor"
                                   }
                );
            levelInventory[0].AllowsTemplateAction = new KeyActions().AllowsTemplateAction;
            levelInventory[0].AllowsIndirectTemplateAction = new KeyActions().AllowsIndirectTemplateAction;

            levelInventory[1].AllowsTemplateAction = new DecorActions().AllowsTemplateAction;
            levelInventory[1].AllowsIndirectTemplateAction = AllowSpecificIndirectAction;

            levelInventory[2].AllowsTemplateAction = new DecorActions().AllowsTemplateAction;
            levelInventory[2].AllowsIndirectTemplateAction = new DecorActions().AllowsIndirectTemplateAction;

            //| 0   |   1W  |   2SC |
            //-----------------------
            //| 3RK |   4RD |   5   |
            //-----------------------
            //| 6   |   7W  |   8FC |

            var level = new Level
            {
                CurrentMap = GenerateSmallMap(),
                Name = new Noun { Main = "Test Level1", Narrator = "The light side" },
                Number = 0,
                StartingCell = 0,
                Story = "Some story",
                Inventory = levelInventory,
                Characters = levelCharacters,
                FinishMessage = "Now that you have finished the light side, go to the wild side",
                ObjectiveCell = 8
            };
            level.CurrentMap.LinkToParentLevel(level);

            return level;
        }

        private static bool AllowSpecificIndirectAction(BaseActionTemplate a, IPositionable o)
        {
            return (a.Name.UniqueId == "Open" && o.Name.Main == "Red Key");

        }

        public static bool AllowsIndirectNothing(BaseActionTemplate arg1, IPositionable arg2)
        {
            return false;
        }

        public static bool AllowsIndirectEverything(BaseActionTemplate arg1, IPositionable arg2)
        {
            return true;
        }

        public static bool AllowsEverything(BaseActionTemplate arg)
        {
            return true;
        }

        public static bool AllowsNothing(BaseActionTemplate arg)
        {
            return false;
        }

        public static NonCharacterObject InteractionObject
        {
            get
            {
                return new NonCharacterObject
                           {
                               ObjectTypeId = "Decor",
                               Name =
                                   new Noun {Main = "testReactor", Narrator = "test reactor"},
                               AllowsIndirectTemplateAction =
                                   TestHelper.AllowsIndirectEverything,
                               AllowsTemplateAction = TestHelper.AllowsEverything,
                               Interactions =
                                   new SortedList<string, Interaction[]>
                                       {
                                           {
                                               GenericInteractionRulesKeys.AnyCharacter,
                                               new Interaction[]
                                                   {
                                                       new Interaction
                                                           {
                                                               Action =new ObjectUseOnObjectActionTemplate
                                                                       {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                                                                                       UniqueId="Ping",
                                                                                       Past="tested",
                                                                                       Present="test"
                                                                                   },
                                                                           StartingAction = true,
                                                                           ChanceOfSuccess = ChanceOfSuccess.Full,
                                                                           PercentOfHealthFailurePenalty = 0
                                                                       },
                                                               Reaction =new OneObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Pong",
                                                                                       Past="tested reaction",
                                                                                       Present="test reaction"
                                                                                   },
                                                                           StartingAction = false,
                                                                           FinishTheInteractionOnExecution = true
                                                                       }
                                                           },
                                                           new Interaction
                                                           {
                                                               Action =new OneObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Ping1",
                                                                                       Past="tested",
                                                                                       Present="test"
                                                                                   },
                                                                           StartingAction = true
                                                                       },
                                                               Reaction =new ObjectUseOnObjectActionTemplate
                                                                       {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="Pong1",
                                                                                       Past="tested reaction",
                                                                                       Present="test reaction"
                                                                                   },
                                                                           StartingAction = false,
                                                                           FinishTheInteractionOnExecution = true,
                                                                           ChanceOfSuccess = ChanceOfSuccess.Full,
                                                                           PercentOfHealthFailurePenalty = 0

                                                                       }
                                                           },
                                                           new Interaction
                                                           {
                                                               Action =new ObjectUseOnObjectActionTemplate
                                                                       {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="DoublePing",
                                                                                       Past="tested",
                                                                                       Present="test"
                                                                                   },
                                                                           StartingAction = true,
                                                                           ChanceOfSuccess = ChanceOfSuccess.Full,
                                                                           PercentOfHealthFailurePenalty = 0
                                                                       },
                                                               Reaction =new OneObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="DoublePong",
                                                                                       Past="tested reaction",
                                                                                       Present="test reaction"
                                                                                   },
                                                                           StartingAction = false,
                                                                           FinishTheInteractionOnExecution = true
                                                                       }
                                                           },
                                                           new Interaction
                                                           {
                                                               Action =new OneObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="DoublePing1",
                                                                                       Past="tested",
                                                                                       Present="test"
                                                                                   },
                                                                           StartingAction = true
                                                                       },
                                                               Reaction =new ObjectUseOnObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="DoublePong1",
                                                                                       Past="tested reaction",
                                                                                       Present="test reaction"
                                                                                   },
                                                                           StartingAction = false,
                                                                           FinishTheInteractionOnExecution = true,
                                                                           ChanceOfSuccess = ChanceOfSuccess.Full,
                                                                           PercentOfHealthFailurePenalty = 0

                                                                       }
                                                           }
                                                   }
                                           },
                                           {
                                               GenericInteractionRulesKeys.AnyCharacterResponses,
                                               new Interaction[]
                                                   {
                                                           new Interaction
                                                           {
                                                               Action =new OneObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="DoublePong",
                                                                                       Past="tested reaction",
                                                                                       Present="test reaction"
                                                                                   },
                                                                           StartingAction = false,
                                                                           FinishTheInteractionOnExecution = true
                                                                       },
                                                               Reaction =new ObjectUseOnObjectActionTemplate
                                                                       {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="PingPong",
                                                                                       Past="tested",
                                                                                       Present="test"
                                                                                   },
                                                                           StartingAction = true,
                                                                           ChanceOfSuccess = ChanceOfSuccess.Full,
                                                                           PercentOfHealthFailurePenalty = 0
                                                                       },
                                                           },
                                                           new Interaction
                                                           {
                                                               Reaction =new ObjectUseOnObjectActionTemplate
                                                                       {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="DoublePong1",
                                                                                       Past="tested reaction",
                                                                                       Present="test reaction"
                                                                                   },
                                                                           StartingAction = false,
                                                                           FinishTheInteractionOnExecution = true,
                                                                           ChanceOfSuccess = ChanceOfSuccess.Full,
                                                                           PercentOfHealthFailurePenalty = 0

                                                                       },
                                                               Action =new OneObjectActionTemplate
                                                                       {
                CurrentPerformer = new OneObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId="PingPong1",
                                                                                       Past="tested",
                                                                                       Present="test"
                                                                                   }
                                                                       }
                                                           }
                                                   }
                                           }
                                       }
                           };
            }
        }
    }
}
