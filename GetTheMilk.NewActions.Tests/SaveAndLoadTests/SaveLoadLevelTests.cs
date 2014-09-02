using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using Newtonsoft.Json;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests.SaveLoadTests
{
    [TestFixture]
    public class SaveLoadLevelTests
    {
        [Test]
        public void SaveAndLoadLevelWithoutPlayer()
        {
            var levelCharacters = new CharacterCollection();

            Inventory skInventory= new Inventory{InventoryType=InventoryType.CharacterInventory,MaximumCapacity=200};
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
            SortedList<string, Interaction[]> skInteractionRules = new SortedList<string, Interaction[]>();

            skInteractionRules.Add(GenericInteractionRulesKeys.AnyCharacter,
                                   new Interaction[]
                                       {
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate
                                                   {
                                                       Name=new Verb{UniqueId="Meet",Past="met",Present="meet"},
                                                       Message="Hi",
                                                       PerformerType=typeof(CommunicateActionPerformer),
                                                       StartingAction=true
                                                   },
                                                   Reaction =
                                                       new TwoCharactersActionTemplate
                                                           {

                                                               Name= new Verb{UniqueId="Talk",Past="talked", Present="talk"}, 
                                                               Message = "How are you? Beautifull day out there better buy something!",
                                                               PerformerType=typeof(CommunicateActionPerformer)
                                                           }
                                               },
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate 
                                                   {
                                                       Name= new Verb{UniqueId="Responde",Past="talked", Present="talk"}, 
                                                       Message = "Yes",
                                                       PerformerType=typeof(CommunicateActionPerformer)},
                                                   Reaction =
                                                       new ExposeInventoryActionTemplate
                                                           {Name=new Verb{UniqueId="ExposeInventory",Past="exposed inventory",Present="expose inventory"},
                                                               FinishingAction = ExposeInventoryFinishingAction.CloseInventory}
                                               },
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate 
                                                   {
                                                       Name= new Verb{UniqueId="RespondeNo", Present="talk",Past="talked"},
                                                       Message = "No",
                                                       PerformerType=typeof(CommunicateActionPerformer)},
                                                   Reaction =
                                                       new TwoCharactersActionTemplate
                                                           {
                                                               Name= new Verb{UniqueId="Lamment",Past="said",Present="say"}, 
                                                               Message = "Why oh Why!?",
                                                               PerformerType=typeof(CommunicateActionPerformer)}
                                               }
                                       });
            skInteractionRules.Add(GenericInteractionRulesKeys.AnyCharacterResponses,
                                   new Interaction[]
                                       {
                                           new Interaction
                                               {
                                                   Action =
                                                       new TwoCharactersActionTemplate
                                                           {
                                                               Name= new Verb{UniqueId="Talk"}, 
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!",
                                                               PerformerType=typeof(CommunicateActionPerformer)
                                                           },
                                                   Reaction =
                                                       new TwoCharactersActionTemplate 
                                                       {
                                                           Name= new Verb{UniqueId="Responde",Past="talked",Present="talk"}, 
                                                           Message = "Yes",
                                                               PerformerType=typeof(CommunicateActionPerformer)}
                                               },
                                           new Interaction
                                               {
                                                   Action =
                                                       new TwoCharactersActionTemplate
                                                           {
                                                               Name= new Verb{UniqueId="Talk",Past="said", Present="say"}, 
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!",
                                                               PerformerType=typeof(CommunicateActionPerformer)

                                                           },
                                                   Reaction =
                                                       new TwoCharactersActionTemplate 
                                                       {
                                                           Name= new Verb{UniqueId="RespondeNO",Past="talked",Present="talk"}, 
                                                           Message = "No",
                                                               PerformerType=typeof(CommunicateActionPerformer)}
                                               }

                                       });

            var skCharacter = new Character
                                  {
                                      CellNumber = 2,
                                      Walet = new Walet {MaxCapacity = 1000, CurrentCapacity = 100},
                                      BlockMovement = true,
                                      Name = new Noun { Main = "John the Shop Keeper", Narrator = "John the Shop Keeper", Description = "there is a shop keeper" },
                                      Inventory=skInventory,
                                      ObjectTypeId="NPCFriendly"
                                  };
            skCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter,skInteractionRules[GenericInteractionRulesKeys.AnyCharacter]);
            skCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, skInteractionRules[GenericInteractionRulesKeys.AnyCharacterResponses]);

            Inventory fInventory=new Inventory{InventoryType=InventoryType.CharacterInventory,MaximumCapacity=20};
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
            SortedList<string, Interaction[]> fInteractionRules=new SortedList<string, Interaction[]>();
            fInteractionRules.Add(GenericInteractionRulesKeys.AnyCharacter, new Interaction[]
                                                                                     {
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Meet",Present="say", Past="said"},Message="Hi",PerformerType=typeof(CommunicateActionPerformer),StartingAction=true},
                                                                                                 Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},PerformerType=typeof(AttackActionPerformer)}
                                                                                             },
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},PerformerType=typeof(TwoCharactersActionTemplatePerformer)},
                                                                                                 Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},PerformerType=typeof(AttackActionPerformer)}
                                                                                             },
                                                                                         new Interaction
                                                                                             {
                                                                                                 Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},PerformerType=typeof(TwoCharactersActionTemplatePerformer)},
                                                                                                 Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="AcceptQuit",Past="accepted quit",Present="accept quit"},PerformerType=typeof(AcceptQuitActionPerformer)}
                                                                                             },
                                                                                         new Interaction
                                                                                         {
                                                                                             Action= new TwoCharactersActionTemplate{Name=new Verb{UniqueId="InitiateHostilities",Past="attacked",Present="attack"},PerformerType=typeof(InitiateHostilitiesActionPerformer),StartingAction=true},
                                                                                             Reaction=new TwoCharactersActionTemplate{Name=new Verb{UniqueId="InitiateHostilities",Past="attacked",Present="attack"},PerformerType=typeof(InitiateHostilitiesActionPerformer)}
                                                                                         }
                                                                                     });
            fInteractionRules.Add(GenericInteractionRulesKeys.AnyCharacterResponses,
                new Interaction[]
                {
                    new Interaction
                        {
                            Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},PerformerType=typeof(AttackActionPerformer)},
                            Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},PerformerType=typeof(AttackActionPerformer)}
                        },
                    new Interaction
                        {
                            Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},PerformerType=typeof(AttackActionPerformer)},
                            Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},PerformerType=typeof(TwoCharactersActionTemplatePerformer)}
                        },
                    new Interaction
                    {
                        Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},PerformerType=typeof(TwoCharactersActionTemplatePerformer)},
                        Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},PerformerType=typeof(AttackActionPerformer)}
                    },
                    new Interaction
                    {
                        Action = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},PerformerType=typeof(TwoCharactersActionTemplatePerformer)},
                        Reaction = new TwoCharactersActionTemplate{Name=new Verb{UniqueId="AcceptQuit",Past="accepted quit",Present="accept quit"},PerformerType=typeof(AcceptQuitActionPerformer)}
                    },
                    new Interaction
                    {
                        Action= new TwoCharactersActionTemplate{Name=new Verb{UniqueId="InitiateHostilities",Past="attacked",Present="attack"},PerformerType=typeof(InitiateHostilitiesActionPerformer)},
                        Reaction = new ExposeInventoryActionTemplate
                            {Name=new Verb{UniqueId="PrepareForBattle",Past="attacked",Present="attack"}, FinishingAction = ExposeInventoryFinishingAction.Attack,SelfInventory=true}

                    }
                });

            var fCharacter = new Character
                                 {
                                     CellNumber = 8,
                                     BlockMovement = true,
                                     Walet = new Walet {MaxCapacity = 2000, CurrentCapacity = 400},
                                     Name = new Noun {Main = "Baddie", Narrator = "the Baddie",Description="there is a fierce warrior"},
                                     Inventory = fInventory,
                                     ObjectTypeId="NPCFoe"
                                 };
            fCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, fInteractionRules[GenericInteractionRulesKeys.AnyCharacter]);
            fCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, fInteractionRules[GenericInteractionRulesKeys.AnyCharacterResponses]);
            levelCharacters.Add(skCharacter, fCharacter);

            var levelInventory = new Inventory { InventoryType = InventoryType.LevelInventory, MaximumCapacity = 2000};
            levelInventory.Add(new Tool
                                   {
                                       CellNumber = 3,
                                       Name = new Noun {Main = "Red Key", Narrator = "the Red Key",Description="the Red Key of Kirna and you wonder how did you knew what it was."},
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
                                                                               {Name = new Verb {UniqueId = "Keep", Past="kept",Present="keep"},PerformerType=typeof(ObjectTransferToActiveCharacterPerformer),StartingAction=true}
                                                                   }
                                                           }
                                                   }
                                               }
                                   },
                               new Tool
                                   {
                                       CellNumber = 4,
                                       Name = new Noun {Main = "Red Door", Narrator = "red door",
                                           Description="Upclose you realise it is a door and it seems to be locked."},
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
                                                                               {Name = new Verb {UniqueId = "Open", Past="opened",Present="open"}, 
                                                                                   ChanceOfSuccess=ChanceOfSuccess.Full,
                                                                                   PercentOfHealthFailurePenalty=0,
                                                                                   StartingAction=true,
                                                                                   DestroyActiveObject=true,
                                                                                   DestroyTargetObject=true, 
                                                                                   PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer)}
                                                                   }
                                                           }
                                                   }
                                               }
                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 1,
                                       Name = new Noun { Main = "Wall", Narrator = "wall", Description= "The wall is solid stone, unpassable for sure." },
                                       BlockMovement = true,
                                       ObjectTypeId = "Decor"

                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 7,
                                       Name = new Noun { Main = "Wall", Narrator = "wall", Description = "The wall is solid stone, unpassable for sure." },
                                       BlockMovement = true,
                                       ObjectTypeId = "Decor"
                                   }
                );

            //| 0   |   1W  |   2SC |
            //-----------------------
            //| 3RK |   4RD |   5   |
            //-----------------------
            //| 6   |   7W  |   8FC |

            var level = new Level
                            {
                                CurrentMap = new Map
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
                                                   },
                                Name = new Noun { Main = "Test Level1", Narrator = "The light side" },
                                Number = 0,
                                StartingCell = 0,
                                Story = "Some story",
                                Inventory= levelInventory,
                                Characters=levelCharacters,
                                FinishMessage="Now that you have finished the light side, go to the wild side",
                                ObjectiveCell=8
                            };
            level.CurrentMap.LinkToParentLevel(level);
            var result = level.PackageForSave();
            var r1 = JsonConvert.SerializeObject(result);
            Assert.IsNotNull(result);
            var actual = Level.Create(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual(level.Number, actual.Number);
            Assert.AreEqual(level.Name.Main, actual.Name.Main);
            Assert.AreEqual(level.Name.Narrator, actual.Name.Narrator);
            Assert.AreEqual(level.StartingCell, actual.StartingCell);
            Assert.AreEqual(level.Story, actual.Story);
            Assert.AreEqual(level.CurrentMap.Cells.Length, actual.CurrentMap.Cells.Length);
            Assert.IsNotNull(actual.Inventory);
            Assert.AreEqual(level.Inventory.InventoryType,actual.Inventory.InventoryType);
            Assert.AreEqual(level.Name.Main, actual.Inventory.Owner.Name.Main);
            Assert.AreEqual(level.Inventory.Count,actual.Inventory.Count);
            Assert.False(actual.Inventory.Any(o=>o.StorageContainer.Owner.Name.Main!=actual.Name.Main));
            Assert.AreEqual(1,actual.CurrentMap.Cells[3].AllObjects.Count());
            Assert.IsNotNull(actual.CurrentMap.Cells[3].AllObjects.First().Interactions);
            Assert.AreEqual(1, actual.CurrentMap.Cells[3].AllObjects.First().Interactions.Keys.Count);
            Assert.AreEqual(GenericInteractionRulesKeys.AnyCharacter, actual.CurrentMap.Cells[3].AllObjects.First().Interactions.Keys[0]);
            Assert.AreEqual(1, actual.CurrentMap.Cells[3].AllObjects.First().Interactions[GenericInteractionRulesKeys.AnyCharacter].Count());
            Assert.AreEqual("Keep", actual.CurrentMap.Cells[3].AllObjects.First().Interactions[GenericInteractionRulesKeys.AnyCharacter].First().Action.Name.UniqueId);
            Assert.IsNull(actual.CurrentMap.Cells[3].AllObjects.First().Interactions[GenericInteractionRulesKeys.AnyCharacter].First().Reaction);
            Assert.IsNotNull(actual.Characters);
            Assert.AreEqual(level.Characters.Count, actual.Characters.Count);
            Assert.IsNotNull(actual.Characters.First().Interactions);
            Assert.AreEqual(2, actual.Characters.First().Interactions.Keys.Count);
           
            Assert.AreEqual(InventoryType.CharacterInventory,actual.Characters[0].Inventory.InventoryType);
            Assert.AreEqual(2,actual.Characters.Count(c => c.StorageContainer.Owner != null));
        }

    }
}
