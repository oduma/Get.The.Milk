using System.Collections.Generic;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using System.Linq;

namespace GetTheMilk.NewActions.Tests
{

    

    public static class TestHelper
    {


        public static void CheckAllActionsAfterInteractionsLoad(IActionEnabled actionInitiator, string actionInitiatorInteractionKey, IActionEnabled reactor, string reactorInteractionKey)
        {
            Assert.True(actionInitiator.AllActions.Any(a => a.Key == actionInitiator.Interactions[actionInitiatorInteractionKey][0].Action.Name.UniqueId));
            Assert.True(actionInitiator.AllActions.Any(a => a.Key == actionInitiator.Interactions[actionInitiatorInteractionKey][1].Action.Name.UniqueId));

            Assert.True(reactor.AllActions.Any(a => a.Key == reactor.Interactions[reactorInteractionKey][0].Reaction.Name.UniqueId));
            Assert.True(reactor.AllActions.Any(a => a.Key == reactor.Interactions[reactorInteractionKey][1].Reaction.Name.UniqueId));
        }

        public static void ValidateAnyCharacterLoadedInteractions(IActionEnabled o, int keyIndex, string interactionKey)
        {
            Assert.AreEqual(interactionKey, o.Interactions.Keys[keyIndex]);
            Assert.AreEqual(2, o.Interactions[interactionKey].Length);
            Assert.IsNotNull(o.Interactions[interactionKey][0].Action);
            Assert.IsNotNull(o.Interactions[interactionKey][0].Reaction);
            Assert.IsNotNull(o.Interactions[interactionKey][1].Action);
            Assert.IsNotNull(o.Interactions[interactionKey][1].Reaction);
            Assert.AreEqual(o.Interactions[interactionKey][0].Action.Name.UniqueId, "Interaction1-Action");
            Assert.AreEqual(o.Interactions[interactionKey][0].Reaction.Name.UniqueId, "Interaction1-Reaction");
            Assert.AreEqual(o.Interactions[interactionKey][1].Action.Name.UniqueId, "Interaction2-Action");
            Assert.AreEqual(o.Interactions[interactionKey][1].Reaction.Name.UniqueId, "Interaction2-Reaction");
        }

        public static void ValidateAnyCharacterResponsesInteractions(IActionEnabled o, int keyIndex,string interactionKey)
        {
            Assert.AreEqual(interactionKey, o.Interactions.Keys[keyIndex]);
            Assert.AreEqual(2, o.Interactions[interactionKey].Length);
            Assert.IsNotNull(o.Interactions[interactionKey][0].Action);
            Assert.IsNotNull(o.Interactions[interactionKey][0].Reaction);
            Assert.IsNotNull(o.Interactions[interactionKey][1].Action);
            Assert.IsNotNull(o.Interactions[interactionKey][1].Reaction);
            Assert.AreEqual(o.Interactions[interactionKey][0].Action.Name.UniqueId, "Interaction1-Reaction");
            Assert.AreEqual(o.Interactions[interactionKey][0].Reaction.Name.UniqueId, "Interaction1-ReReaction1");
            Assert.AreEqual(o.Interactions[interactionKey][1].Action.Name.UniqueId, "Interaction1-Reaction");
            Assert.AreEqual(o.Interactions[interactionKey][1].Reaction.Name.UniqueId, "Interaction1-ReReaction2");
        }

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

            var objAction = ObjectActionsFactory.CreateObjectAction("NPCFriendly");
            var objEAction = ObjectActionsFactory.CreateObjectAction("NPCFoe");

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
                Name = new Noun { Main = "Knife", Narrator = "the knife", Description = "It is a small but very sharp knife." },
                ObjectTypeId = "Weapon"
            },
            new Tool
            {
                BlockMovement = false,
                BuyPrice = 4,
                SellPrice = 2,
                Name = new Noun { Main = "Can Opener", Narrator = "the can opener", Description = "In the grass right in front there is a can opener." },
                ObjectTypeId = "CanOpener"
            });
            var skInteractionRules = 
                new SortedList<string, Interaction[]>
                {
                    {
                        GenericInteractionRulesKeys.AnyCharacter, new Interaction[]
                        {
                            new Interaction
                            {
                                Action =new TwoCharactersActionTemplate
                                {
                                    PerformerType = typeof(TwoCharactersActionTemplatePerformer),
                                    Name = new Verb
                                    {
                                        UniqueId="Meet",
                                        Past="met",
                                        Present="meet"
                                    },
                                    StartingAction=true
                                },
                                Reaction=new TwoCharactersActionTemplate
                                {
                                    Message="How are you? Beautifull day out there better buy something!",
                                    Name= new Verb{UniqueId="AskForSale",Past="said",Present="say"},
                                    PerformerType= typeof(CommunicateActionPerformer)
                                }
                            },

                            new Interaction
                            {
                                Action = new TwoCharactersActionTemplate
                                {
                                    Message = "Yes",
                                    Name= new Verb{UniqueId="SayYes",Past="said",Present="say"}
                                },
                                Reaction = new ExposeInventoryActionTemplate
                                {
                                    FinishingAction = ExposeInventoryFinishingAction.CloseInventory,
                                    PerformerType=typeof( ExposeInventoryActionTemplatePerformer),
                                    Name= new Verb{UniqueId="ExposeForSale",Past="exposed",Present="exposed"}
                                }
                            },
                            new Interaction
                            {
                                Action = new TwoCharactersActionTemplate
                                {
                                    Message = "No",
                                    Name= new Verb{UniqueId="SayNo",Past="said",Present="say"}
                                },
                                Reaction = new TwoCharactersActionTemplate
                                {
                                    Message = "Why oh Why!?",
                                    PerformerType=typeof( CommunicateActionPerformer),
                                    Name= new Verb{UniqueId="SayWhyOhWhy",Past="said",Present="say"}
                                }
                            }
                        }
                    },
                    {
                        GenericInteractionRulesKeys.AnyCharacterResponses, new Interaction[]
                        {
                            new Interaction
                            {
                                Action = new TwoCharactersActionTemplate
                                {
                                    Message = "Please give me a tool!",
                                    Name= new Verb{UniqueId="AskForTool",Past="said",Present="say"}
                                },
                                Reaction = new ObjectTransferActionTemplate
                                {
                                    PerformerType=typeof( GiveToActionPerformer),
                                    Name = new Verb{UniqueId="GiveTo",Past="gave to",Present="give to"},
                                }
                            },
                            new Interaction
                            {
                                Action=new TwoCharactersActionTemplate
                                {
                                    Message = "Please give me a tool!",
                                    Name= new Verb{UniqueId="AskForTool",Past="said",Present="say"}
                                },
                                Reaction = new ObjectTransferActionTemplate
                                {
                                    PerformerType=typeof( SellActionPerformer),
                                    Name = new Verb{UniqueId="Sell",Past="sold",Present="sell"},
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
                Name = new Noun { Main = "John the Shop Keeper", Narrator = "John the Shop Keeper",Description="there is a shop keeper" },
                Inventory = skInventory,
                ObjectTypeId = "NPCFriendly"
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
                Name = new Noun { Main = "Knife", Narrator = "the knife", Description = "It is a small but very sharp knife." },
                ObjectTypeId = "Weapon"
            });
            SortedList<string, Interaction[]> fInteractionRules = new SortedList<string, Interaction[]>();
            fInteractionRules.Add(GenericInteractionRulesKeys.AnyCharacter, new Interaction[]
                                                                                     {
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{
                PerformerType=typeof( TwoCharactersActionTemplatePerformer),
                Name = new Verb
                {
                    UniqueId="Meet",Past="met",Present="meet"},StartingAction=true},
                                                                                                 Reaction = new TwoCharactersActionTemplate{
                PerformerType=typeof( AttackActionPerformer),
                Name = new Verb
                {
                    UniqueId="Attack"}}
                                                                                             },
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{
                PerformerType=typeof( TwoCharactersActionTemplatePerformer),
                Name = new Verb
                {
                    UniqueId="Quit"}},
                                                                                                 Reaction = new TwoCharactersActionTemplate{
                PerformerType=typeof( AttackActionPerformer),
                Name = new Verb
                {
                    UniqueId="Attack"}}
                                                                                             },
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{
                PerformerType=typeof( TwoCharactersActionTemplatePerformer),
                Name = new Verb
                {
                    UniqueId="Quit"}},
                                                                                                 Reaction = new TwoCharactersActionTemplate{
                PerformerType=typeof( AcceptQuitActionPerformer),
                Name = new Verb
                {
                    UniqueId="AcceptQuit"}}
                                                                                             },
                                                                                         new Interaction
                                                                                         {
                                                                                             Action= new TwoCharactersActionTemplate{
                PerformerType=typeof( InitiateHostilitiesActionPerformer),
                Name = new Verb
                {
                    UniqueId="InitiateHostilities",Past="attacked",Present="attack"},StartingAction=true},
                                                                                             Reaction=new TwoCharactersActionTemplate{
                PerformerType=typeof( InitiateHostilitiesActionPerformer),
                Name = new Verb
                {
                    UniqueId="InitiateHostilities"}}
                                                                                         }
                                                                                     });

            
            var fCharacter = new Character
            {
                CellNumber = 8,
                BlockMovement = true,
                Walet = new Walet { MaxCapacity = 2000, CurrentCapacity = 400 },
                Name = new Noun { Main = "Baddie", Narrator = "the Baddie", Description = "there is a fierce warrior" },
                Inventory = fInventory,
                Interactions = fInteractionRules,
                ObjectTypeId = "NPCFoe"
                
            };

            fCharacter.AllowsTemplateAction = objEAction.AllowsTemplateAction;
            fCharacter.AllowsIndirectTemplateAction = objEAction.AllowsIndirectTemplateAction;

            levelCharacters.Add(skCharacter, fCharacter);

            var levelInventory = new Inventory { InventoryType = InventoryType.LevelInventory, MaximumCapacity = 2000 };
            levelInventory.Add(new Tool
                                   {
                                       CellNumber = 3,
                                       Name = new Noun {Main = "Red Key", Narrator = "the Red Key",Description= "the Red Key of Kirna and you wonder how did you knew what it was."},
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
                PerformerType=typeof( ObjectTransferToActiveCharacterPerformer),
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

                                       Name = new Noun { Main = "Red Door", Narrator = "red door", Description = "Upclose you realise it is a door and it seems to be locked." },
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
                PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
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
                                       Name = new Noun {Main = "Wall", Narrator = "wall",
                                       Description = "The wall is solid stone, unpassable for sure."
                                       },
                                       BlockMovement = true,
                                       ObjectTypeId = "Decor"

                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 7,
                                       Name = new Noun
                                       {
                                           Main = "Wall",
                                           Narrator = "wall",
                                           Description = "The wall is solid stone, unpassable for sure."
                                       },
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
                    Name = new Noun { Main = "testReactor", Narrator = "test reactor" },
                    AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                    AllowsTemplateAction = TestHelper.AllowsEverything,
                    Interactions = new SortedList<string, Interaction[]>
                    {
                        {
                            GenericInteractionRulesKeys.AnyCharacter, new Interaction[]
                            {
                                new Interaction
                                {
                                    Action =new ObjectUseOnObjectActionTemplate
                                    {
                                        PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
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
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
                                        Name = new Verb
                                        {
                                            UniqueId="Pong",
                                            Past="tested reaction",
                                            Present="test reaction"
                                        },
                                        StartingAction = false
                                    }
                                },
                                new Interaction
                                {
                                    Action =new OneObjectActionTemplate
                                    {
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
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
                                        PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
                                        Name = new Verb
                                        {
                                            UniqueId="Pong1",
                                            Past="tested reaction",
                                            Present="test reaction"
                                        },
                                        StartingAction = false,
                                        ChanceOfSuccess = ChanceOfSuccess.Full,
                                        PercentOfHealthFailurePenalty = 0
                                    }
                                },
                                new Interaction
                                {
                                    Action =new ObjectUseOnObjectActionTemplate
                                    {
                                        PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
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
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
                                        Name = new Verb
                                        {
                                            UniqueId="DoublePong",
                                            Past="tested reaction",
                                            Present="test reaction"
                                        },
                                        StartingAction = false
                                    }
                                },
                                new Interaction
                                {
                                    Action =new OneObjectActionTemplate
                                    {
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
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
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
                                        Name = new Verb
                                        {
                                            UniqueId="DoublePong1",
                                            Past="tested reaction",
                                            Present="test reaction"
                                        },
                                        StartingAction = false,
                                        ChanceOfSuccess = ChanceOfSuccess.Full,
                                        PercentOfHealthFailurePenalty = 0
                                    }
                                }
                        }
                    },
                    {
                        GenericInteractionRulesKeys.AnyCharacterResponses, new Interaction[]
                        {
                                new Interaction
                                {
                                    Action =new OneObjectActionTemplate
                                    {
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
                                        Name = new Verb
                                        {
                                            UniqueId="DoublePong",
                                            Past="tested reaction",
                                            Present="test reaction"
                                        },
                                        StartingAction = false
                                    },
                                    Reaction =new ObjectUseOnObjectActionTemplate
                                    {
                                        PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
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
                                    Action =new ObjectUseOnObjectActionTemplate
                                    {
                                        PerformerType=typeof( ObjectUseOnObjectActionTemplatePerformer),
                                        Name = new Verb
                                        {
                                            UniqueId="DoublePong1",
                                            Past="tested reaction",
                                            Present="test reaction"
                                        },
                                        StartingAction = false,
                                        ChanceOfSuccess = ChanceOfSuccess.Full,
                                        PercentOfHealthFailurePenalty = 0
                                    },
                                    Reaction =new OneObjectActionTemplate
                                    {
                                        PerformerType=typeof( OneObjectActionTemplatePerformer),
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


        public static Level GetToTheFight(out PerformActionResult movementResult, int levelNumber = 0)
        {
            var level = GetTheMilk.Levels.Level.Create(levelNumber);

            Assert.IsNotNull(level);
            Assert.AreEqual(0, level.Number);
            Assert.AreEqual(4, level.Inventory.Count);
            Assert.AreEqual(2, level.Characters.Count);

            //create a new player
            var player = new Player();

            var objAction = ObjectActionsFactory.CreateObjectAction("Player");
            player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
            player.Health = 10;
            player.Experience = 50;

            player.SetPlayerName("Me");
            player.Walet.CurrentCapacity = 20;

            level.Player = null;
            //The player enters level 1
            var enterResult = player.EnterLevel(level);
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(ActionResultType.Ok, enterResult.ResultType);

            //The player walks to the east
            var walk = player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            walk.Direction = Direction.East;
            walk.CurrentMap = level.CurrentMap;

            movementResult = level.Player.PerformAction(walk);
            Assert.AreEqual(level.StartingCell, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionTemplateExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //The player walks to the south
            walk.Direction = Direction.South;
            movementResult = level.Player.PerformAction(walk);

            Assert.AreEqual(3, player.CellNumber);
            Assert.AreEqual(0, ((MovementActionTemplateExtraData)movementResult.ExtraData).CharactersInRange.Count());
            Assert.AreEqual(0, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsBlocking.Count());
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsInRange.Count());

            //the player has found something
            //pick it up and keep it

            var actionResult =
                level.Player.PerformAction(
                    ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0]);

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(1, level.Player.Inventory.Count);
            Assert.AreEqual(player.Name, level.Player.Inventory[0].StorageContainer.Owner.Name);
            Assert.AreEqual(3, level.Inventory.Count);

            //the user checks his inventory

            var exposeInventory = level.Player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");
            actionResult = level.Player.PerformAction(exposeInventory);

            Assert.AreEqual(1, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.IsEmpty(((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses);

            //the user closes his inventory
            var closeInventory = level.Player.CreateNewInstanceOfAction(exposeInventory.FinishingAction.ToString());
            actionResult = level.Player.PerformAction(closeInventory);
            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            //the user runs to the east
            var run = level.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Run");
            run.Direction = Direction.East;
            run.CurrentMap = level.CurrentMap;
            movementResult = level.Player.PerformAction(run);
            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            Assert.AreEqual(1, ((MovementActionTemplateExtraData)movementResult.ExtraData).ObjectsBlocking.Count());

            //the player uses the key to open the door
            actionResult =
                level.Player.PerformAction(
                    ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.ToArray()[0]);

            Assert.AreEqual(ActionResultType.Ok, actionResult.ResultType);
            Assert.AreEqual(2, level.Inventory.Count);
            Assert.AreEqual(0, player.Inventory.Count);

            //the userruns to the east
            movementResult = level.Player.PerformAction(run);

            Assert.AreEqual(ActionResultType.OutOfTheMap, movementResult.ResultType);
            Assert.AreEqual(5, player.CellNumber);
            Assert.AreEqual(2, ((MovementActionTemplateExtraData)movementResult.ExtraData).CharactersInRange.Count());

            ////the user decides to meet the shop keeper

            var meet =
                ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates.FirstOrDefault(
                    a => a.Name.UniqueId == "Meet" && a.TargetCharacter.ObjectTypeId == "NPCFriendly");
            actionResult = level.Player.PerformAction(meet);

            Assert.AreEqual("Talk", actionResult.ForAction.Name.UniqueId);
            Assert.AreEqual("How are you? Beautifull day out there better buy something!",
                            ((TwoCharactersActionTemplate)actionResult.ForAction).Message);

            Assert.AreEqual(2, ((List<BaseActionTemplate>)actionResult.ExtraData).Count);
            Assert.AreEqual("Responde", ((List<BaseActionTemplate>)actionResult.ExtraData)[0].Name.UniqueId);
            Assert.AreEqual("RespondeNO", ((List<BaseActionTemplate>)actionResult.ExtraData)[1].Name.UniqueId);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"),
                            ((List<BaseActionTemplate>)actionResult.ExtraData)[0].TargetCharacter);
            Assert.AreEqual(level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly"),
                            ((List<BaseActionTemplate>)actionResult.ExtraData)[1].TargetCharacter);

            //the player choses to continue interaction with shopkeeper
            actionResult = level.Player.PerformAction(((List<BaseActionTemplate>)actionResult.ExtraData)[0]);

            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents.Count());
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[0].Name.Main,
                ((InventoryExtraData)actionResult.ExtraData).Contents[0].Object.Name.Main);
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.Count());
            Assert.AreEqual("Buy",
                            ((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First().Name.UniqueId);
            Assert.AreEqual(
                level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory[1].Name.Main,
                ((InventoryExtraData)actionResult.ExtraData).Contents[1].Object.Name.Main);
            Assert.AreEqual(2, ((InventoryExtraData)actionResult.ExtraData).Contents[1].PossibleUsses.Count());
            Assert.AreEqual("Buy",
                            ((InventoryExtraData)actionResult.ExtraData).Contents[1].PossibleUsses.First().Name.UniqueId);

            //the player buys the knife from the shopkeeper

            var transferresult =
                level.Player.PerformAction(((InventoryExtraData)actionResult.ExtraData).Contents[0].PossibleUsses.First());
            Assert.AreEqual(1, player.Inventory.Count());
            Assert.AreEqual(1, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Inventory.Count());
            Assert.AreEqual(15, player.Walet.CurrentCapacity);
            Assert.AreEqual(105, level.Characters.FirstOrDefault(c => c.ObjectTypeId == "NPCFriendly").Walet.CurrentCapacity);
            Assert.AreEqual(player.Name, player.Inventory[0].StorageContainer.Owner.Name);

            //the player tries to run south

            run.Direction = Direction.South;
            movementResult = level.Player.PerformAction(run);

            Assert.AreEqual(ActionResultType.Blocked, movementResult.ResultType);
            return level;
        }


    }
}
