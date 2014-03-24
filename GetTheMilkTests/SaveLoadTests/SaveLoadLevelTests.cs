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
                                    AttackPower = 4,
                                    DefensePower = 1,
                                    BlockMovement = false,
                                    WeaponTypes = new WeaponType[] {WeaponType.Attack, WeaponType.Deffense},
                                    BuyPrice = 10,
                                    SellPrice = 3,
                                    Name = new Noun {Main = "Knife", Narrator = "the knife"},
                                    ApproachingMessage = "In the distance a knife smiles as you.",
                                    CloseUpMessage = "It is a small but very sharp knife.",
                                    ObjectTypeId = "Weapon"
                                },
                            new Tool
                                {
                                    BlockMovement = false,
                                    BuyPrice = 5,
                                    SellPrice = 1,
                                    Name = new Noun {Main = "Can Opener", Narrator = "the can opener"},
                                    ApproachingMessage = "Some small tool.",
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
                                                       new CommunicateAction
                                                           {
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           }
                                               },
                                           new ActionReaction
                                               {
                                                   Action = new CommunicateAction {Message = "Yes"},
                                                   Reaction =
                                                       new ExposeInventory
                                                           {AllowedNextActions = new GameAction[] {new Buy()}}
                                               },
                                           new ActionReaction
                                               {
                                                   Action = new CommunicateAction {Message = "No"},
                                                   Reaction =
                                                       new CommunicateAction
                                                           {Message = "Why oh Why!?"}
                                               }
                                       });
            skInteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                                   new ActionReaction[]
                                       {
                                           new ActionReaction
                                               {
                                                   Action =
                                                       new CommunicateAction
                                                           {
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           },
                                                   Reaction =
                                                       new CommunicateAction {Message = "Yes"}
                                               },
                                           new ActionReaction
                                               {
                                                   Action =
                                                       new CommunicateAction
                                                           {
                                                               Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           },
                                                   Reaction =
                                                       new CommunicateAction {Message = "No"}
                                               }

                                       });
            var skCharacter = new Character
                                  {
                                      CellNumber = 3,
                                      Walet = new Walet {MaxCapacity = 1000, CurrentCapacity = 200},
                                      BlockMovement = true,
                                      Name = new Noun {Main = "John the Shop Keeper", Narrator = "John the Shop Keeper"},
                                      Inventory=skInventory,
                                      InteractionRules=skInteractionRules,
                                      ObjectTypeId="NPCFriendly"
                                  };

            Inventory fInventory=new Inventory{InventoryType=InventoryType.CharacterInventory,MaximumCapacity=20};
            fInventory.Add(new Weapon
            {
                Durability = 1000,
                AttackPower = 4,
                DefensePower = 1,
                BlockMovement = false,
                WeaponTypes = new WeaponType[] { WeaponType.Attack, WeaponType.Deffense },
                BuyPrice = 10,
                SellPrice = 3,
                Name = new Noun { Main = "Knife", Narrator = "the knife" },
                ApproachingMessage = "In the distance a knife smiles as you.",
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
                                                                                                 Reaction = new Quit()
                                                                                             }
                                                                                     });
            fInteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                                  new ActionReaction[]
                                      {
                                          new ActionReaction
                                              {
                                                  Action = new Quit(),
                                                  Reaction = new Attack()
                                              }
                                      });

            var fCharacter = new Character
                                 {
                                     CellNumber = 9,
                                     BlockMovement = true,
                                     Walet = new Walet {MaxCapacity = 1000, CurrentCapacity = 500},
                                     Name = new Noun {Main = "Baddie", Narrator = "the Baddie"},
                                     Inventory = fInventory,
                                     InteractionRules = fInteractionRules,
                                     ObjectTypeId="NPCFoe"
                                 };

            levelCharacters.Add(skCharacter,fCharacter);

            var levelInventory = new Inventory { InventoryType = InventoryType.LevelInventory, MaximumCapacity = 1000};
            levelInventory.Add(new Tool
                                   {
                                       CellNumber = 4,
                                       ApproachingMessage = "A glint catches your eye.",
                                       CloseUpMessage =
                                           "the Red Key of Kirna and you wonder how did you knew what it was.",
                                       Name = new Noun {Main = "Red Key", Narrator = "the Red Key"},
                                       ObjectTypeId = "Key"
                                   },
                               new Tool
                                   {
                                       CellNumber = 5,
                                       ApproachingMessage = "There is a red door in the distance, or is it a wall?",
                                       CloseUpMessage = "Upclose you realise it is a door and it seems to be locked.",
                                       Name = new Noun {Main = "Red Door", Narrator = "red door"},
                                       BlockMovement=true,
                                       ObjectTypeId = "RedDoor"
                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 2,
                                       ApproachingMessage = "You see a wall",
                                       CloseUpMessage = "The wall is solid stone, unpassable for sure.",
                                       Name = new Noun {Main = "Wall", Narrator = "wall"},
                                       BlockMovement=true,
                                       ObjectTypeId = "Decor"

                                   },
                               new NonCharacterObject
                                   {
                                       CellNumber = 7,
                                       ApproachingMessage = "You see a wall",
                                       CloseUpMessage = "The wall is solid stone, unpassable for sure.",
                                       Name = new Noun {Main = "Wall", Narrator = "wall"},
                                       BlockMovement=true,
                                       ObjectTypeId = "Decor"
                                   }
                );

            //| 1   |   2W  |   3SC |
            //-----------------------
            //| 4RK |   5RD |   6   |
            //-----------------------
            //| 7   |   8W  |   9FC |

            var level = new Level
                            {
                                CurrentMap = new Map
                                                   {
                                                       Cells =
                                                           new Cell[9]
                                                               {
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 0,
                                                                           NorthCell = 0,
                                                                           Number = 1,
                                                                           SouthCell = 4,
                                                                           TopCell = 0,
                                                                           EastCell = 2
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 1,
                                                                           NorthCell = 0,
                                                                           Number = 2,
                                                                           SouthCell = 5,
                                                                           TopCell = 0,
                                                                           EastCell = 3
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 2,
                                                                           NorthCell = 0,
                                                                           Number = 3,
                                                                           SouthCell = 6,
                                                                           TopCell = 0,
                                                                           EastCell = 0
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 0,
                                                                           NorthCell = 1,
                                                                           Number = 4,
                                                                           SouthCell = 7,
                                                                           TopCell = 0,
                                                                           EastCell = 5
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 4,
                                                                           NorthCell = 2,
                                                                           Number = 5,
                                                                           SouthCell = 8,
                                                                           TopCell = 0,
                                                                           EastCell = 6
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 5,
                                                                           NorthCell = 3,
                                                                           Number = 6,
                                                                           SouthCell = 9,
                                                                           TopCell = 0,
                                                                           EastCell = 0
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 0,
                                                                           NorthCell = 4,
                                                                           Number = 7,
                                                                           SouthCell = 0,
                                                                           TopCell = 0,
                                                                           EastCell = 8
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 7,
                                                                           NorthCell = 5,
                                                                           Number = 8,
                                                                           SouthCell = 0,
                                                                           TopCell = 0,
                                                                           EastCell = 9
                                                                       },
                                                                   new Cell
                                                                       {
                                                                           BottomCell = 0,
                                                                           WestCell = 8,
                                                                           NorthCell = 6,
                                                                           Number = 9,
                                                                           SouthCell = 0,
                                                                           TopCell = 0,
                                                                           EastCell = 0
                                                                       }
                                                               }
                                                   },
                                Name = new Noun { Main = "Test Level1", Narrator = "The light side" },
                                Number = 1,
                                StartingCell = 1,
                                Story = "Some story",
                                //Objects=levelInventory,
                                //Characters=levelCharacters
                            };
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
            //Assert.IsNotNull(actual.Objects);
            //Assert.AreEqual(level.Name.Main,actual.Objects.Owner.Name.Main);
            //Assert.AreEqual(level.Objects.Objects.Count,actual.Objects.Objects.Count);
            //Assert.False(actual.Objects.Objects.Any(o=>o.StorageContainer.Owner.Name.Main!=actual.Name.Main));
            //Assert.IsNotNull(actual.Characters);
            //Assert.AreEqual(level.Characters.Characters.Count, actual.Characters.Characters.Count);
        }

        [Test]
        public void LoadALevelAndCheckObjects()
        {
            Level level = Level.Create(1);
            Assert.IsNotNull(level);
        }
    }
}
