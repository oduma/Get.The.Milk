using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using Newtonsoft.Json;
using GetTheMilk.Actions.ActionPerformers;

namespace GetTheMilk.NewActions.Tests.SaveLoadTests
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
                Owner = new Level()
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
                Owner = new Level()
            };

            NonCharacterObject decorum = new NonCharacterObject
            {
                ObjectTypeId = "Decor",
                CellNumber = 123,
                BlockMovement = true,
                Name = new Noun { Main = "Wall", Narrator = "wall" },
                AllowsTemplateAction = (a) => false
            };
            mockInventory.Add(decorum);

            decorum = new NonCharacterObject
            {
                ObjectTypeId = "Decor",

                CellNumber = 124,
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
                Owner = new Character()
            };

            Tool decorum = new Tool
            {
                ObjectTypeId = "Key",

                CellNumber = 123,
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
                Owner = new Character()
            };

            Tool decorum = new Tool
            {
                ObjectTypeId = "Key",

                CellNumber = 123,
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
            skInteractionRules.Add(GenericInteractionRulesKeys.AnyCharacter,
                                   new Interaction[]
                                       {
                                           new Interaction
                                               {
                                                   Action = new TwoCharactersActionTemplate
                                                   {
                                                       Name=new Verb{UniqueId="Meet",Past="met",Present="meet"},
                                                       Message="Hi",
                                                       PerformerType=typeof(CommunicateActionPerformer)
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
                                                           {FinishingAction = ExposeInventoryFinishingAction.CloseInventory}
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

            Inventory skInventory= new Inventory{InventoryType=InventoryType.CharacterInventory,MaximumCapacity=20};
            Weapon weaponum = new Weapon
            {
                ObjectTypeId = "Weapon",

                CellNumber = 124,
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
                                                                  Owner =new Level()
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
                                            Range = 1,
                                            Walet = skWalet
                                           
                                        };
            skCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, skInteractionRules[GenericInteractionRulesKeys.AnyCharacter]);
            skCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, skInteractionRules[GenericInteractionRulesKeys.AnyCharacterResponses]);

            mockCharacterCollection.Add(skCharacter);

            var characterCore = skCharacter.Save();
            var actual = Character.Load<Character>(characterCore);
            Assert.IsNotNull(actual);
            Assert.AreEqual(skCharacter.Interactions.Count, actual.Interactions.Count);
            Assert.AreEqual(skCharacter.ActiveAttackWeapon, actual.ActiveAttackWeapon);
            Assert.AreEqual(skCharacter.ActiveDefenseWeapon, actual.ActiveDefenseWeapon);
            Assert.True(actual.AllowsTemplateAction(null));
            Assert.False(actual.AllowsIndirectTemplateAction(new TwoCharactersActionTemplate{Name=new Verb{UniqueId="Attack"}},null ));
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

        [Test]
        public void SaveAPlayer()
        {
            Player player = new Player();
            var result = player.Save();

            var resultString = JsonConvert.SerializeObject(result);

            var actual = Player.Load<Player>(result);

            Assert.IsNotNull(actual);

            Assert.AreEqual(player.BlockMovement, actual.BlockMovement);
            Assert.AreEqual(player.CellNumber, actual.CellNumber);
            Assert.AreEqual(player.Experience, actual.Experience);
            Assert.AreEqual(player.Health, actual.Health);
            Assert.AreEqual(player.Interactions.Count, actual.Interactions.Count);
            Assert.AreEqual(player.Name.Main, actual.Name.Main);
            Assert.AreEqual(player.Name.Narrator, actual.Name.Narrator);
            Assert.AreEqual(player.Range, actual.Range);
            Assert.AreEqual(player.Walet.MaxCapacity, actual.Walet.MaxCapacity);
            Assert.AreEqual(player.Walet.CurrentCapacity, actual.Walet.CurrentCapacity);
        }

    }
}
