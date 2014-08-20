using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;
using NUnit.Framework;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests.BulkTemplatesTests
{
    [TestFixture]
    public class LoadingActionTemplatesInCharacterTests
    {
        [Test]
        public void LoadCharactersDefaultActionTemplates()
        {
            var character = new Character {ObjectTypeId = "NPCFriendly"};
            Assert.IsNotNull(character.AllActions);
            Assert.AreEqual(4,character.AllActions.Count);
        }

        [Test]
        public void SaveAModifiedCharacter()
        {
            var character = new Character {ObjectTypeId = "NPCFriendly"};
            character.AddAvailableAction(new ExposeInventoryActionTemplate
                                         {
                                             PerformerType=typeof(ExposeInventoryActionTemplatePerformer),
                                             Name= new Verb{UniqueId="ExposeSelfInventory", Past="exposed inventory", Present="expose inventory"},
                                             FinishingAction =ExposeInventoryFinishingAction.CloseInventory
                                         });
            var saveResult = character.Save();
            Assert.AreEqual(
                "{\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\"}",
                saveResult.Core);
            Assert.AreEqual(
                "[{\"FinishingAction\":0,\"SelfInventory\":false,\"Name\":{\"UniqueId\":\"ExposeSelfInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"Category\":\"ExposeInventoryActionTemplate\",\"StartingAction\":false,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"0\"}",
                saveResult.PackagedInventory);
            Assert.AreEqual(
                "{}",
                saveResult.Interactions);

        }

        [Test]
        public void LoadAModifiedCharacter()
        {
            var expected = new Character();
            expected.AddAvailableAction(new ExposeInventoryActionTemplate
            {
                PerformerType=typeof(ExposeInventoryActionTemplatePerformer),
                Name = new Verb { UniqueId = "ExposeSelfInventory", Past = "exposed inventory", Present = "expose inventory" },
                FinishingAction = ExposeInventoryFinishingAction.CloseInventory
            });
            var actual =
                Character.Load<Character>(new ContainerWithActionsPackage
                                              {
                                                  Core =
                                                      "{\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\"}",
                                                  ActionTemplates = "[{\"FinishingAction\":0,\"SelfInventory\":false,\"Name\":{\"UniqueId\":\"ExposeSelfInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"Category\":\"ExposeInventoryActionTemplate\",\"StartingAction\":false,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}]",
                                                  PackagedInventory = "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"0\"}",
                                                  Interactions = "{}"
                                              });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions.Count,actual.AllActions.Count);
            var newAction = actual.AllActions.FirstOrDefault(a => a.Key == "ExposeSelfInventory");
            Assert.IsNotNull(newAction);
            Assert.AreEqual(ExposeInventoryFinishingAction.CloseInventory, ((ExposeInventoryActionTemplate)newAction.Value).FinishingAction);
            Assert.AreEqual(0,actual.Interactions.Count);
        }
        
    }
}
