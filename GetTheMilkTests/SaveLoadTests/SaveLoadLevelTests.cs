using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilkTests.SaveLoadTests
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
                                    WeaponTypes = new WeaponType[] {WeaponType.Attack, WeaponType.Deffense},
                                    BuyPrice = 5,
                                    SellPrice = 2,
                                    Name = new Noun {Main = "Knife", Narrator = "the knife"},
                                    CloseUpMessage = "It is a small but very sharp knife.",
                                    ObjectTypeId = "Weapon"
                                },
                            new Tool
                                {
                                    BlockMovement = false,
                                    BuyPrice = 4,
                                    SellPrice = 2,
                                    Name = new Noun {Main = "Can Opener", Narrator = "the can opener"},
                                    CloseUpMessage = "In the grass right in front there is a can opener.",
                                    ObjectTypeId = "CanOpener"
                                });
            SortedList<string, ActionReaction[]> skInteractionRules = new SortedList<string, ActionReaction[]>();
            skInteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific,
                                   new ActionReaction[]
                                       {
                                           new ActionReaction
                                               {
                                                   Action = new Meet(),
                                                   Reaction =
                                                       new Communicate
                                                           {
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           }
                                               },
                                           new ActionReaction
                                               {
                                                   Action = new Communicate {Message = "Yes"},
                                                   Reaction =
                                                       new ExposeInventory
                                                           {AllowedNextActionTypes = new InventorySubActionType[] {new InventorySubActionType{ActionType=ActionType.Buy, FinishInventoryExposure=true}}}
                                               },
                                           new ActionReaction
                                               {
                                                   Action = new Communicate {Message = "No"},
                                                   Reaction =
                                                       new Communicate
                                                           {Message = "Why oh Why!?"}
                                               }
                                       });
            skInteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                                   new ActionReaction[]
                                       {
                                           new ActionReaction
                                               {
                                                   Action =
                                                       new Communicate
                                                           {
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           },
                                                   Reaction =
                                                       new Communicate {Message = "Yes"}
                                               },
                                           new ActionReaction
                                               {
                                                   Action =
                                                       new Communicate
                                                           {
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           },
                                                   Reaction =
                                                       new Communicate {Message = "No"}
                                               }

                                       });
            var skCharacter = new Character
                                  {
                                      CellNumber = 2,
                                      Walet = new Walet {MaxCapacity = 1000, CurrentCapacity = 100},
                                      BlockMovement = true,
                                      Name = new Noun {Main = "John the Shop Keeper", Narrator = "John the Shop Keeper"},
                                      Inventory=skInventory,
                                      InteractionRules=skInteractionRules,
                                      ObjectTypeId="NPCFriendly",
                                      CloseUpMessage="there is a shop keeper"
                                  };

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
                Name = new Noun { Main = "Knife", Narrator = "the knife" },
                CloseUpMessage = "It is a small but very sharp knife.",
                ObjectTypeId = "Weapon"
            });
            SortedList<string, ActionReaction[]> fInteractionRules=new SortedList<string, ActionReaction[]>();
            fInteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific, new ActionReaction[]
                                                                                     {
                                                                                         new ActionReaction
                                                                                             {
                                                                                                 Action = new Meet(),
                                                                                                 Reaction = new Attack()
                                                                                             },
                                                                                         new ActionReaction
                                                                                             {
                                                                                                 Action = new Quit(),
                                                                                                 Reaction = new Attack()
                                                                                             },
                                                                                         new ActionReaction
                                                                                             {
                                                                                                 Action = new Quit(),
                                                                                                 Reaction = new AcceptQuit()
                                                                                             },
                                                                                         new ActionReaction
                                                                                         {
                                                                                             Action= new InitiateHostilities(),
                                                                                             Reaction=new InitiateHostilities()
                                                                                         }
                                                                                     });
            fInteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                new ActionReaction[]
                {
                    new ActionReaction
                    {
                        Action = new Quit(),
                        Reaction = new Attack()
                    },
                    new ActionReaction
                    {
                        Action = new Quit(),
                        Reaction = new AcceptQuit()
                    },
                    new ActionReaction
                    {
                        Action = new InitiateHostilities(),
                        Reaction = new ExposeInventory
                            {AllowedNextActionTypes = new InventorySubActionType[] {new InventorySubActionType{ActionType = ActionType.SelectAttackWeapon},new InventorySubActionType{ActionType = ActionType.SelectDefenseWeapon},new InventorySubActionType{ActionType = ActionType.Attack, FinishInventoryExposure=true}},SelfInventory=true}

                    }
                });

            var fCharacter = new Character
                                 {
                                     CellNumber = 8,
                                     BlockMovement = true,
                                     Walet = new Walet {MaxCapacity = 2000, CurrentCapacity = 400},
                                     Name = new Noun {Main = "Baddie", Narrator = "the Baddie"},
                                     Inventory = fInventory,
                                     InteractionRules = fInteractionRules,
                                     ObjectTypeId="NPCFoe",
                                     CloseUpMessage = "there is a fierce warrior"

                                 };

            levelCharacters.Add(skCharacter,fCharacter);

            var levelInventory = new Inventory { InventoryType = InventoryType.LevelInventory, MaximumCapacity = 2000};
            levelInventory.Add(new Tool
                                   {
                                       CellNumber = 3,
                                       CloseUpMessage =
                                           "the Red Key of Kirna and you wonder how did you knew what it was.",
                                       Name = new Noun {Main = "Red Key", Narrator = "the Red Key"},
                                       ObjectTypeId = "Key"
                                   },
                               new Tool
                                   {
                                       CellNumber = 4,
                                       CloseUpMessage = "Upclose you realise it is a door and it seems to be locked.",
                                       Name = new Noun {Main = "Red Door", Narrator = "red door"},
                                       BlockMovement=true,
                                       ObjectTypeId = "RedDoor"
                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 1,
                                       CloseUpMessage = "The wall is solid stone, unpassable for sure.",
                                       Name = new Noun {Main = "Wall", Narrator = "wall"},
                                       BlockMovement=true,
                                       ObjectTypeId = "Decor"

                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 7,
                                       CloseUpMessage = "The wall is solid stone, unpassable for sure.",
                                       Name = new Noun {Main = "Wall", Narrator = "wall"},
                                       BlockMovement=true,
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
                                Characters=levelCharacters
                            };
            level.CurrentMap.LinkToParentLevel(level);
            var result = level.Save();
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
            Assert.IsNotNull(actual.Characters);
            Assert.AreEqual(level.Characters.Count, actual.Characters.Count);
            Assert.AreEqual(InventoryType.CharacterInventory,actual.Characters[0].Inventory.InventoryType);
        }

        [Test]
        public void LoadALevelAndCheckObjects()
        {
            Level level = Level.Create(1);
            Assert.IsNotNull(level);
        }
    }
}
