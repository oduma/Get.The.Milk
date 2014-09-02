using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.BulkTemplatesTests
{
    [TestFixture]
    public class LoadingActionTemplatesInNestedObjects : BaseTestClass
    {
        [Test]
        public void SaveAModifiedObjectInACharacterInventory()
        {
            var character = new Character { ObjectTypeId = "NPCFriendly" };

            character.Inventory= new Inventory{MaximumCapacity=2};
            var tool = new Tool
                           {
                               Name = new Noun {Main = "TestTool", Narrator = "test tool"},
                               ObjectTypeId = "Tool"
                           };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                                    Past = "exploded",
                                    Present = "explode"
                                }
                    }
                );
            character.Inventory.Add(tool);

            var saveResult = character.Save();
            Assert.AreEqual(
    "{\"Contents\":\"[{\\\"Core\\\":\\\"{\\\\\\\"BuyPrice\\\\\\\":0,\\\\\\\"SellPrice\\\\\\\":0,\\\\\\\"ObjectTypeId\\\\\\\":\\\\\\\"Tool\\\\\\\",\\\\\\\"Name\\\\\\\":{\\\\\\\"Main\\\\\\\":\\\\\\\"TestTool\\\\\\\",\\\\\\\"Narrator\\\\\\\":\\\\\\\"test tool\\\\\\\",\\\\\\\"Description\\\\\\\":null},\\\\\\\"CellNumber\\\\\\\":0,\\\\\\\"BlockMovement\\\\\\\":false,\\\\\\\"ObjectCategory\\\\\\\":1}\\\",\\\"ActionTemplates\\\":\\\"{\\\\\\\"Explode\\\\\\\":{\\\\\\\"Name\\\\\\\":{\\\\\\\"UniqueId\\\\\\\":\\\\\\\"Explode\\\\\\\",\\\\\\\"Present\\\\\\\":\\\\\\\"explode\\\\\\\",\\\\\\\"Past\\\\\\\":\\\\\\\"exploded\\\\\\\"},\\\\\\\"Category\\\\\\\":\\\\\\\"OneObjectActionTemplate\\\\\\\",\\\\\\\"StartingAction\\\\\\\":true,\\\\\\\"TargetObject\\\\\\\":null,\\\\\\\"TargetCharacter\\\\\\\":null,\\\\\\\"PerformerType\\\\\\\":\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\"}}\\\",\\\"Interactions\\\":\\\"null\\\"}]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"2\"}",
    saveResult.PackagedInventory);
            Assert.AreEqual(
                "{\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\"}",
                saveResult.Core);
            Assert.AreEqual(
                "[]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{}",
                saveResult.Interactions);


        }

        [Test]
        public void LoadAModifiedObjectInACharacterInventory()
        {
            var expected = new Character();
            expected.Inventory = new Inventory { MaximumCapacity = 2 };
            var tool = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId = "Tool"
            };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                            Past = "exploded",
                            Present = "explode"
                        }
                }
                );
            expected.Inventory.Add(tool);
            var actual =
                Character.Load<Character>(new ContainerWithActionsPackage
                {
                    Core =
                        "{\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\"}",
                    ActionTemplates = "[]",
                    PackagedInventory = "{\"Contents\":\"[{\\\"Core\\\":\\\"{\\\\\\\"BuyPrice\\\\\\\":0,\\\\\\\"SellPrice\\\\\\\":0,\\\\\\\"ObjectTypeId\\\\\\\":\\\\\\\"Tool\\\\\\\",\\\\\\\"Name\\\\\\\":{\\\\\\\"Main\\\\\\\":\\\\\\\"TestTool\\\\\\\",\\\\\\\"Narrator\\\\\\\":\\\\\\\"test tool\\\\\\\",\\\\\\\"Description\\\\\\\":null},\\\\\\\"CellNumber\\\\\\\":0,\\\\\\\"BlockMovement\\\\\\\":false,\\\\\\\"ObjectCategory\\\\\\\":1}\\\",\\\"ActionTemplates\\\":\\\"{\\\\\\\"Explode\\\\\\\":{\\\\\\\"Name\\\\\\\":{\\\\\\\"UniqueId\\\\\\\":\\\\\\\"Explode\\\\\\\",\\\\\\\"Present\\\\\\\":\\\\\\\"explode\\\\\\\",\\\\\\\"Past\\\\\\\":\\\\\\\"exploded\\\\\\\"},\\\\\\\"Category\\\\\\\":\\\\\\\"OneObjectActionTemplate\\\\\\\",\\\\\\\"StartingAction\\\\\\\":true,\\\\\\\"TargetObject\\\\\\\":null,\\\\\\\"TargetCharacter\\\\\\\":null,\\\\\\\"PerformerType\\\\\\\":\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\"}}\\\",\\\"Interactions\\\":\\\"null\\\"}]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"2\"}",
                    Interactions = "{}"
                });
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Inventory.Count);
            Assert.AreEqual(expected.Inventory[0].ObjectCategory, actual.Inventory[0].ObjectCategory);
            Assert.AreEqual(expected.Inventory[0].AllActions.Count, actual.Inventory[0].AllActions.Count);
            Assert.AreEqual(typeof(OneObjectActionTemplate), actual.Inventory[0].AllActions["Explode"].GetType());
       }

        [Test]
        public void SaveAModifiedObjectInALevelInventory()
        {
            var level = new Level();

            level.Inventory = new Inventory { MaximumCapacity = 2 };
            var tool = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId = "Tool"
            };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                            Past = "exploded",
                            Present = "explode"
                        }
                }
                );
            level.Inventory.Add(tool);
            var saveResult = level.PackageForSave();
            Assert.IsNotNull(saveResult);
            Assert.AreEqual(
    "{\"Contents\":\"[{\\\"Core\\\":\\\"{\\\\\\\"BuyPrice\\\\\\\":0,\\\\\\\"SellPrice\\\\\\\":0,\\\\\\\"ObjectTypeId\\\\\\\":\\\\\\\"Tool\\\\\\\",\\\\\\\"Name\\\\\\\":{\\\\\\\"Main\\\\\\\":\\\\\\\"TestTool\\\\\\\",\\\\\\\"Narrator\\\\\\\":\\\\\\\"test tool\\\\\\\",\\\\\\\"Description\\\\\\\":null},\\\\\\\"CellNumber\\\\\\\":0,\\\\\\\"BlockMovement\\\\\\\":false,\\\\\\\"ObjectCategory\\\\\\\":1}\\\",\\\"ActionTemplates\\\":\\\"{\\\\\\\"Explode\\\\\\\":{\\\\\\\"Name\\\\\\\":{\\\\\\\"UniqueId\\\\\\\":\\\\\\\"Explode\\\\\\\",\\\\\\\"Present\\\\\\\":\\\\\\\"explode\\\\\\\",\\\\\\\"Past\\\\\\\":\\\\\\\"exploded\\\\\\\"},\\\\\\\"Category\\\\\\\":\\\\\\\"OneObjectActionTemplate\\\\\\\",\\\\\\\"StartingAction\\\\\\\":true,\\\\\\\"TargetObject\\\\\\\":null,\\\\\\\"TargetCharacter\\\\\\\":null,\\\\\\\"PerformerType\\\\\\\":\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\"}}\\\",\\\"Interactions\\\":\\\"null\\\"}]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"2\"}",
    saveResult.InventoryCore);
            Assert.AreEqual(
                "{\"CurrentMap\":null,\"Number\":0,\"Story\":null,\"StartingCell\":0,\"Name\":null,\"FinishMessage\":null,\"SizeOfLevel\":3,\"ObjectiveCell\":0}",
                saveResult.Core);
        }

        [Test]
        public void LoadAModifiedObjectInALevelInventory()
        {
            var expected = new Level();

            expected.Inventory = new Inventory { MaximumCapacity = 2 };
            var tool = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId = "Tool"
            };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                            Past = "exploded",
                            Present = "explode"
                        }
                }
                );
            expected.Inventory.Add(tool);
            var actual =
                Level.Create(new ContainerNoActionsPackage
                                 {
                                     InventoryCore =
                                         "{\"Contents\":\"[{\\\"Core\\\":\\\"{\\\\\\\"BuyPrice\\\\\\\":0,\\\\\\\"SellPrice\\\\\\\":0,\\\\\\\"ObjectTypeId\\\\\\\":\\\\\\\"Tool\\\\\\\",\\\\\\\"Name\\\\\\\":{\\\\\\\"Main\\\\\\\":\\\\\\\"TestTool\\\\\\\",\\\\\\\"Narrator\\\\\\\":\\\\\\\"test tool\\\\\\\",\\\\\\\"Description\\\\\\\":null},\\\\\\\"CellNumber\\\\\\\":0,\\\\\\\"BlockMovement\\\\\\\":false,\\\\\\\"ObjectCategory\\\\\\\":1}\\\",\\\"ActionTemplates\\\":\\\"{\\\\\\\"Explode\\\\\\\":{\\\\\\\"Name\\\\\\\":{\\\\\\\"UniqueId\\\\\\\":\\\\\\\"Explode\\\\\\\",\\\\\\\"Present\\\\\\\":\\\\\\\"explode\\\\\\\",\\\\\\\"Past\\\\\\\":\\\\\\\"exploded\\\\\\\"},\\\\\\\"Category\\\\\\\":\\\\\\\"OneObjectActionTemplate\\\\\\\",\\\\\\\"StartingAction\\\\\\\":true,\\\\\\\"TargetObject\\\\\\\":null,\\\\\\\"TargetCharacter\\\\\\\":null,\\\\\\\"PerformerType\\\\\\\":\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\"}}\\\",\\\"Interactions\\\":\\\"null\\\"}]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"2\"}",
                                     Core =
                                         "{\"CurrentMap\":null,\"Number\":0,\"Story\":null,\"StartingCell\":0,\"Name\":null,\"FinishMessage\":null,\"SizeOfLevel\":3,\"ObjectiveCell\":0}"
                                 });
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Inventory.Count);
            Assert.AreEqual(expected.Inventory[0].ObjectCategory, actual.Inventory[0].ObjectCategory);
            Assert.AreEqual(expected.Inventory[0].AllActions.Count, actual.Inventory[0].AllActions.Count);
            Assert.AreEqual(typeof(OneObjectActionTemplate), actual.Inventory[0].AllActions["Explode"].GetType());

        }

        [Test]
        public void SaveAModifiedCharacterInALevelInventory()
        {
                        var character = new Character {ObjectTypeId = "NPCFriendly"};
            character.AddAvailableAction(new ExposeInventoryActionTemplate
                                         {
                                             PerformerType=typeof(ExposeInventoryActionTemplatePerformer),
                                             FinishingAction = ExposeInventoryFinishingAction.CloseInventory,
                                             Name = new Verb { UniqueId="ExposeSelfInventory", Present="expose inventory", Past="exposed inventory"}
                                         });
            character.Inventory = new Inventory {InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 2};
            var tool = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId = "Tool"
            };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                            Past = "exploded",
                            Present = "explode"
                        }
                }
                );
            character.Inventory.Add(tool);

            var level = new Level();
            level.Number = 0;
            level.Characters = new CharacterCollection {character};
            var saveResult = level.PackageForSave();
            Assert.IsNotNull(saveResult);
            Assert.AreEqual(
    "{\"Contents\":\"[]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"0\"}",
    saveResult.InventoryCore);
            Assert.AreEqual(
                "{\"CurrentMap\":null,\"Number\":0,\"Story\":null,\"StartingCell\":0,\"Name\":null,\"FinishMessage\":null,\"SizeOfLevel\":3,\"ObjectiveCell\":0}",
                saveResult.Core);
            Assert.AreEqual("{\"Contents\":\"[{\\\"PackagedInventory\\\":\\\"{\\\\\\\"Contents\\\\\\\":\\\\\\\"[{\\\\\\\\\\\\\\\"Core\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"BuyPrice\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":0,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"SellPrice\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":0,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"ObjectTypeId\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Tool\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Name\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Main\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"TestTool\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Narrator\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"test tool\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Description\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":null},\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"CellNumber\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":0,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"BlockMovement\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":false,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"ObjectCategory\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":1}\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\"ActionTemplates\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Explode\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Name\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"UniqueId\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Explode\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Present\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"explode\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Past\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"exploded\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"},\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Category\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"OneObjectActionTemplate\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"StartingAction\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":true,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"TargetObject\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":null,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"TargetCharacter\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":null,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"PerformerType\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"}}\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\"Interactions\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"null\\\\\\\\\\\\\\\"}]\\\\\\\",\\\\\\\"InventoryType\\\\\\\":\\\\\\\"1\\\\\\\",\\\\\\\"MaximumCapacity\\\\\\\":\\\\\\\"2\\\\\\\"}\\\",\\\"Core\\\":\\\"{\\\\\\\"Health\\\\\\\":0,\\\\\\\"Experience\\\\\\\":0,\\\\\\\"Walet\\\\\\\":{\\\\\\\"MaxCapacity\\\\\\\":0,\\\\\\\"CurrentCapacity\\\\\\\":0},\\\\\\\"Range\\\\\\\":0,\\\\\\\"ActiveDefenseWeapon\\\\\\\":null,\\\\\\\"ActiveAttackWeapon\\\\\\\":null,\\\\\\\"Name\\\\\\\":null,\\\\\\\"CellNumber\\\\\\\":0,\\\\\\\"BlockMovement\\\\\\\":false,\\\\\\\"ObjectTypeId\\\\\\\":\\\\\\\"NPCFriendly\\\\\\\"}\\\",\\\"ActionTemplates\\\":\\\"[{\\\\\\\"FinishingAction\\\\\\\":0,\\\\\\\"SelfInventory\\\\\\\":false,\\\\\\\"Name\\\\\\\":{\\\\\\\"UniqueId\\\\\\\":\\\\\\\"ExposeSelfInventory\\\\\\\",\\\\\\\"Present\\\\\\\":\\\\\\\"expose inventory\\\\\\\",\\\\\\\"Past\\\\\\\":\\\\\\\"exposed inventory\\\\\\\"},\\\\\\\"Category\\\\\\\":\\\\\\\"ExposeInventoryActionTemplate\\\\\\\",\\\\\\\"StartingAction\\\\\\\":false,\\\\\\\"TargetObject\\\\\\\":null,\\\\\\\"TargetCharacter\\\\\\\":null,\\\\\\\"PerformerType\\\\\\\":\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\"}]\\\",\\\"Interactions\\\":\\\"{}\\\"}]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"0\"}", saveResult.LevelCharacters);

        }

        [Test]
        public void LoadAModifiedCharacterInALevelInventory()
        {
            var character = new Character { ObjectTypeId = "NPCFriendly" };
            character.AddAvailableAction(new ExposeInventoryActionTemplate
            {
                PerformerType=typeof(ExposeInventoryActionTemplatePerformer),
                FinishingAction = ExposeInventoryFinishingAction.CloseInventory,
                Name = new Verb { UniqueId = "ExposeSelfInventory", Present = "expose inventory", Past = "exposed inventory" }
            });
            character.Inventory = new Inventory { InventoryType = InventoryType.CharacterInventory, MaximumCapacity = 2 };
            var tool = new Tool
            {
                Name = new Noun { Main = "TestTool", Narrator = "test tool" },
                ObjectTypeId = "Tool"
            };
            tool.AddAvailableAction(
                new OneObjectActionTemplate
                {
                    PerformerType=typeof(OneObjectActionTemplatePerformer),
                    Name = new Verb
                    {
                        UniqueId = "Explode",
                            Past = "exploded",
                            Present = "explode"
                        }
                }
                );
            character.Inventory.Add(tool);
            var expected = new Level();
            expected.Number = 0;
            expected.Characters = new CharacterCollection { character };

            var actual =
                Level.Create(new ContainerNoActionsPackage
                                 {
                                     Core =
                                         "{\"CurrentMap\":null,\"Number\":0,\"Story\":null,\"StartingCell\":0,\"Name\":null,\"FinishMessage\":null,\"SizeOfLevel\":3,\"ObjectiveCell\":0}",
                                     InventoryCore =
                                         "{\"Contents\":\"[]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"0\"}",
                                     LevelCharacters =
                                         "{\"Contents\":\"[{\\\"PackagedInventory\\\":\\\"{\\\\\\\"Contents\\\\\\\":\\\\\\\"[{\\\\\\\\\\\\\\\"Core\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"BuyPrice\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":0,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"SellPrice\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":0,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"ObjectTypeId\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Tool\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Name\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Main\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"TestTool\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Narrator\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"test tool\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Description\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":null},\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"CellNumber\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":0,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"BlockMovement\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":false,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"ObjectCategory\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":1}\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\"ActionTemplates\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Explode\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Name\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":{\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"UniqueId\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Explode\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Present\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"explode\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Past\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"exploded\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"},\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"Category\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"OneObjectActionTemplate\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"StartingAction\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":true,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"TargetObject\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":null,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"TargetCharacter\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":null,\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"PerformerType\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.OneObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"}}\\\\\\\\\\\\\\\",\\\\\\\\\\\\\\\"Interactions\\\\\\\\\\\\\\\":\\\\\\\\\\\\\\\"null\\\\\\\\\\\\\\\"}]\\\\\\\",\\\\\\\"InventoryType\\\\\\\":\\\\\\\"1\\\\\\\",\\\\\\\"MaximumCapacity\\\\\\\":\\\\\\\"2\\\\\\\"}\\\",\\\"Core\\\":\\\"{\\\\\\\"Health\\\\\\\":0,\\\\\\\"Experience\\\\\\\":0,\\\\\\\"Walet\\\\\\\":{\\\\\\\"MaxCapacity\\\\\\\":0,\\\\\\\"CurrentCapacity\\\\\\\":0},\\\\\\\"Range\\\\\\\":0,\\\\\\\"ActiveDefenseWeapon\\\\\\\":null,\\\\\\\"ActiveAttackWeapon\\\\\\\":null,\\\\\\\"Name\\\\\\\":null,\\\\\\\"CellNumber\\\\\\\":0,\\\\\\\"BlockMovement\\\\\\\":false,\\\\\\\"ObjectTypeId\\\\\\\":\\\\\\\"NPCFriendly\\\\\\\"}\\\",\\\"ActionTemplates\\\":\\\"[{\\\\\\\"FinishingAction\\\\\\\":0,\\\\\\\"SelfInventory\\\\\\\":false,\\\\\\\"Name\\\\\\\":{\\\\\\\"UniqueId\\\\\\\":\\\\\\\"ExposeSelfInventory\\\\\\\",\\\\\\\"Present\\\\\\\":\\\\\\\"expose inventory\\\\\\\",\\\\\\\"Past\\\\\\\":\\\\\\\"exposed inventory\\\\\\\"},\\\\\\\"Category\\\\\\\":\\\\\\\"ExposeInventoryActionTemplate\\\\\\\",\\\\\\\"StartingAction\\\\\\\":false,\\\\\\\"TargetObject\\\\\\\":null,\\\\\\\"TargetCharacter\\\\\\\":null,\\\\\\\"PerformerType\\\\\\\":\\\\\\\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\\\\\\\"}]\\\",\\\"Interactions\\\":\\\"{}\\\"}]\",\"InventoryType\":\"0\",\"MaximumCapacity\":\"0\"}"
                                 });
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Characters.Count);
            Assert.AreEqual(expected.Characters[0].ObjectTypeId, actual.Characters[0].ObjectTypeId);
            Assert.AreEqual(expected.Characters[0].AllActions.Count, actual.Characters[0].AllActions.Count);
            Assert.AreEqual(1, actual.Characters[0].Inventory.Count);
            Assert.AreEqual(expected.Characters[0].Inventory[0].ObjectCategory, actual.Characters[0].Inventory[0].ObjectCategory);
            Assert.AreEqual(expected.Characters[0].Inventory[0].AllActions.Count, actual.Characters[0].Inventory[0].AllActions.Count);
            Assert.AreEqual(typeof(OneObjectActionTemplate), actual.Characters[0].Inventory[0].AllActions["Explode"].GetType());


        }

    }
}
