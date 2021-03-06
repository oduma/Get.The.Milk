using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using GetTheMilkTests.Stubs;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilkTests.SaveLoadTests
{
    [TestFixture]
    public class SerializeDeserializeTests
    {
        [Test]
        public void SerializeAnEmptyDecorInventory()
        {
            var mockInventory = new Inventory
            {
                InventoryType = InventoryType.LevelInventory,
                MaximumCapacity = 10,
                Owner = new MockInventoryOwner()
            };

            var result = JsonConvert.SerializeObject(mockInventory.Save());
            Inventory actual = Inventory.Load(JsonConvert.DeserializeObject<CollectionPackage>(result));
            actual.LinkObjectsToInventory();
            Assert.AreEqual(mockInventory.InventoryType,actual.InventoryType);
            Assert.AreEqual(mockInventory.MaximumCapacity, actual.MaximumCapacity);
        }
        [Test]
        public void SerializeADecorInventory()
        {
            var mockInventory = new Inventory
            {
                InventoryType = InventoryType.LevelInventory,
                MaximumCapacity = 10,
                Owner = new MockInventoryOwner()
            };

            NonCharacterObject decorum = new NonCharacterObject
            {
                ObjectTypeId = "Decor",
                CellNumber = 123,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false
            };
            mockInventory.Add(decorum);

            decorum = new NonCharacterObject
            {
                ObjectTypeId = "Decor",

                CellNumber = 124,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false
            };
            mockInventory.Add(decorum);
            var result = JsonConvert.SerializeObject(mockInventory.Save());
            Inventory actual = Inventory.Load(JsonConvert.DeserializeObject<CollectionPackage>(result,new NonChracterObjectConverter()));
            actual.LinkObjectsToInventory();
            Assert.AreEqual(mockInventory.InventoryType, actual.InventoryType);
            Assert.AreEqual(mockInventory.MaximumCapacity, actual.MaximumCapacity);
            Assert.AreEqual(mockInventory.Count, actual.Count);
            Assert.AreEqual(mockInventory[0].CellNumber, actual[0].CellNumber);
            Assert.AreEqual(mockInventory[1].CellNumber, actual[1].CellNumber);
            Assert.True(mockInventory.Any(o => o.StorageContainer != null));
        }

        [Test]
        public void SerializeAToolInventory()
        {
            var mockInventory = new Inventory
            {
                InventoryType = InventoryType.LevelInventory,
                MaximumCapacity = 10,
                Owner = new MockInventoryOwner()
            };

            Tool decorum = new Tool
            {
                ObjectTypeId = "Key",

                CellNumber = 123,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false,
                BuyPrice=100,
                SellPrice=50
            };
            mockInventory.Add(decorum);

            decorum = new Tool
            {
                ObjectTypeId = "Key",

                CellNumber = 124,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false,
                BuyPrice=200,
                SellPrice=100
            };
            mockInventory.Add(decorum);
            var result = JsonConvert.SerializeObject(mockInventory.Save());
            Inventory actual = Inventory.Load(JsonConvert.DeserializeObject<CollectionPackage>(result, new NonChracterObjectConverter()));
            actual.LinkObjectsToInventory();
            Assert.AreEqual(mockInventory.InventoryType, actual.InventoryType);
            Assert.AreEqual(mockInventory.MaximumCapacity, actual.MaximumCapacity);
            Assert.AreEqual(mockInventory.Count, actual.Count);
            Assert.AreEqual(mockInventory[0].CellNumber, actual[0].CellNumber);
            Assert.AreEqual(mockInventory[1].CellNumber, actual[1].CellNumber);
            Assert.AreEqual(((Tool)mockInventory[0]).BuyPrice, ((Tool)actual[0]).BuyPrice);
            Assert.AreEqual(((Tool)mockInventory[0]).SellPrice, ((Tool)actual[0]).SellPrice);
            Assert.AreEqual(((Tool)mockInventory[1]).BuyPrice, ((Tool)actual[1]).BuyPrice);
            Assert.AreEqual(((Tool)mockInventory[1]).SellPrice, ((Tool)actual[1]).SellPrice);
            Assert.True(mockInventory.Any(o=>o.StorageContainer!=null));
        }

        [Test]
        public void SerializeAMixedInventory()
        {
            var mockInventory = new Inventory
            {
                InventoryType = InventoryType.LevelInventory,
                MaximumCapacity = 10,
                Owner = new MockInventoryOwner()
            };

            Tool decorum = new Tool
            {
                ObjectTypeId = "Key",

                CellNumber = 123,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false,
                BuyPrice = 100,
                SellPrice = 50
            };
            mockInventory.Add(decorum);

            Weapon weaponum = new Weapon
            {
                ObjectTypeId = "Weapon",

                CellNumber = 124,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false,
                BuyPrice = 200,
                SellPrice = 100,
                Durability=100,
                DefensePower=20,
                AttackPower=10
            };
            mockInventory.Add(weaponum);
            NonCharacterObject tdecorum = new NonCharacterObject
            {
                ObjectTypeId = "Decor",

                CellNumber = 123,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false
            };
            mockInventory.Add(tdecorum);


            var result = JsonConvert.SerializeObject(mockInventory.Save());
            Inventory actual = Inventory.Load(JsonConvert.DeserializeObject<CollectionPackage>(result, new NonChracterObjectConverter()));
            actual.LinkObjectsToInventory();
            Assert.AreEqual(mockInventory.InventoryType, actual.InventoryType);
            Assert.AreEqual(mockInventory.MaximumCapacity, actual.MaximumCapacity);
            Assert.AreEqual(mockInventory.Count, actual.Count);
            Assert.AreEqual(mockInventory[0].CellNumber, actual[0].CellNumber);
            Assert.AreEqual(mockInventory[1].CellNumber, actual[1].CellNumber);
            Assert.AreEqual(((Tool)mockInventory[0]).BuyPrice, ((Tool)actual[0]).BuyPrice);
            Assert.AreEqual(((Tool)mockInventory[0]).SellPrice, ((Tool)actual[0]).SellPrice);
            Assert.AreEqual(((Tool)mockInventory[1]).BuyPrice, ((Tool)actual[1]).BuyPrice);
            Assert.AreEqual(((Tool)mockInventory[1]).SellPrice, ((Tool)actual[1]).SellPrice);
            Assert.AreEqual(ObjectCategory.Decor, actual[2].ObjectCategory);
            Assert.AreEqual(ObjectCategory.Weapon, actual[1].ObjectCategory);
            Assert.AreEqual(ObjectCategory.Tool, actual[0].ObjectCategory);
            Assert.True(mockInventory.Any(o => o.StorageContainer != null));
        }

        [Test]
        public void SerializeACharacter()
        {
            SortedList<string, Interaction[]> skInteractionRules=new SortedList<string, Interaction[]>();
            skInteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific,
                                   new Interaction[]
                                       {
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate{Name=new Verb{PerformerId="Meet"}},
                                                   Reaction =
                                                       new TwoCharactersActionTemplate
                                                           {

                                                               Name= new Verb{PerformerId="Talk"}, Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           }
                                               },
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate {Name= new Verb{PerformerId="Talk"}, Message = "Yes"},
                                                   Reaction =
                                                       new ExposeInventoryActionTemplate
                                                           {FinishActionType = "CloseInventory"}
                                               },
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate {Name= new Verb{PerformerId="Talk"}, Message = "No"},
                                                   Reaction =
                                                       new TwoCharactersActionTemplate
                                                           {Name= new Verb{PerformerId="Talk"}, Message = "Why oh Why!?"}
                                               }
                                       });
            skInteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                                   new Interaction[]
                                       {
                                           new Interaction
                                               {
                                                   Action =
                                                       new TwoCharactersActionTemplate
                                                           {
                                                               Name= new Verb{PerformerId="Talk"}, Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           },
                                                   Reaction =
                                                       new TwoCharactersActionTemplate {Name= new Verb{PerformerId="Talk"}, Message = "Yes"}
                                               },
                                           new Interaction
                                               {
                                                   Action =
                                                       new TwoCharactersActionTemplate
                                                           {
                                                               Name= new Verb{PerformerId="Talk"}, Message =
                                                                   "How are you? Beautifull day out there better buy something!"
                                                           },
                                                   Reaction =
                                                       new TwoCharactersActionTemplate {Name= new Verb{PerformerId="Talk"}, Message = "No"}
                                               }

                                       });

            Inventory skInventory= new Inventory{InventoryType=InventoryType.CharacterInventory,MaximumCapacity=20};
            Weapon weaponum = new Weapon
            {
                ObjectTypeId = "Weapon",

                CellNumber = 124,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false,
                BuyPrice = 200,
                SellPrice = 100,
                Durability = 100,
                DefensePower = 20,
                AttackPower = 10
            };
            skInventory.Add(weaponum);
            Tool decorum = new Tool
            {
                ObjectTypeId = "Key",

                CellNumber = 123,
                CloseUpMessage = "You can't pass me",
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false,
                BuyPrice = 100,
                SellPrice = 50
            };
            skInventory.Add(decorum);

            Walet skWalet= new Walet{CurrentCapacity=100,MaxCapacity = 1000};

            CharacterCollection mockCharacterCollection = new CharacterCollection
                                                              {
                                                                  Owner =
                                                                      new MockInventoryOwner
                                                                          {
                                                                              Name =
                                                                                  new Noun
                                                                                      {
                                                                                          Main = "Level",
                                                                                          Narrator = "level"
                                                                                      }
                                                                          }
                                                              };
            Character skCharacter = new Character
                                        {
                                            ActiveAttackWeapon = null,
                                            ActiveDefenseWeapon = null,
                                            BlockMovement = true,
                                            CellNumber = 234,
                                            Experience = 200,
                                            Health = 100,
                                            Inventory = skInventory,
                                            Name = new Noun {Main = "John the Shop Keeper", Narrator = "John the Shop Keeper"},
                                            ObjectTypeId = "NPCFriendly",
                                            Interactions=skInteractionRules,
                                            Range = 1,
                                            Walet = skWalet
                                           
                                        };
            mockCharacterCollection.Add(skCharacter);
            var characterCore = skCharacter.Save();
            var actual = Character.Load<Character>(characterCore);
            Assert.IsNotNull(actual);
            Assert.AreEqual(skCharacter.Interactions.Count, actual.Interactions.Count);
            Assert.AreEqual(skCharacter.Interactions[GenericInteractionRulesKeys.All].Count(),
                            actual.Interactions[GenericInteractionRulesKeys.All].Count());
            Assert.AreEqual(skCharacter.ActiveAttackWeapon, actual.ActiveAttackWeapon);
            Assert.AreEqual(skCharacter.ActiveDefenseWeapon, actual.ActiveDefenseWeapon);
            Assert.True(actual.AllowsTemplateAction(null));
            Assert.True(actual.AllowsIndirectTemplateAction(new BaseActionTemplate{Name=new Verb{PerformerId="Meet"}}, null));
            Assert.False(actual.AllowsIndirectTemplateAction(new TwoCharactersActionTemplate{Name=new Verb{PerformerId="Attack"}},null ));
            Assert.AreEqual(skCharacter.BlockMovement, actual.BlockMovement);
            Assert.AreEqual(skCharacter.CellNumber, actual.CellNumber);
            Assert.AreEqual(skCharacter.Experience, actual.Experience);
            Assert.AreEqual(skCharacter.Health, actual.Health);
            Assert.AreEqual(skCharacter.Inventory.Owner.Name.Main, actual.Inventory.Owner.Name.Main);
            Assert.AreEqual(skCharacter.Inventory.Count, actual.Inventory.Count);
            Assert.AreEqual(skCharacter.Name.Main, actual.Name.Main);
            Assert.AreEqual(skCharacter.Name.Narrator, actual.Name.Narrator);
            Assert.AreEqual(skCharacter.Range, actual.Range);
            Assert.AreEqual(skCharacter.Walet.MaxCapacity, actual.Walet.MaxCapacity);
        }

    }
}
