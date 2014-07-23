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
            Assert.AreEqual(5,character.AllActions.Count);
            Assert.AreEqual(2,
                            character.AllActions.Count(
                                a =>
                                (a.Key == "Attack" || a.Key == "Quit")));
        }

        [Test]
        public void SaveAModifiedCharacter()
        {
            var character = new Character {ObjectTypeId = "NPCFriendly"};
            character.AddAvailableAction(new ExposeInventoryActionTemplate
                                         {
                                             CurrentPerformer=new ExposeInventoryActionTemplatePerformer(),
                                             Name= new Verb{UniqueId="ExposeSelfInventory", Past="exposed inventory", Present="expose inventory"},
                                             FinishActionUniqueId = "CloseInventory"
                                         });
            var saveResult = character.Save();
            Assert.AreEqual(
                "{\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\",\"CloseUpMessage\":null}",
                saveResult.Core);
            Assert.AreEqual(
                "[{\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"FinishActionUniqueId\":\"CloseInventory\",\"SelfInventory\":false,\"Name\":{\"UniqueId\":\"ExposeSelfInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"Category\":\"ExposeInventoryActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null}]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"0\"}",
                saveResult.PackagedInventory);
            Assert.AreEqual(
                "{\"All\":[{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null}},{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.TwoCharactersActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null}}]}",
                saveResult.Interactions);

        }

        [Test]
        public void LoadAModifiedCharacter()
        {
            var expected = new Character();
            expected.AddAvailableAction(new ExposeInventoryActionTemplate
            {
                CurrentPerformer = new ExposeInventoryActionTemplatePerformer(),
                Name = new Verb { UniqueId = "ExposeSelfInventory", Past = "exposed inventory", Present = "expose inventory" },
                FinishActionUniqueId = "CloseInventory"
            });
            var actual =
                Character.Load<Character>(new ContainerWithActionsPackage
                                              {
                                                  Core =
                                                      "{\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\",\"CloseUpMessage\":null}",
                                                  ActionTemplates = "[{\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ExposeInventoryActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"FinishActionUniqueId\":\"CloseInventory\",\"SelfInventory\":false,\"Name\":{\"UniqueId\":\"ExposeSelfInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"Category\":\"ExposeInventoryActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null}]",
                                                  PackagedInventory = "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"0\"}",
                                                  Interactions = "{\"All\":[{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null}},{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.TwoCharactersActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null}}]}"
                                              });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions.Count,actual.AllActions.Count);
            var newAction = actual.AllActions.FirstOrDefault(a => a.Key == "ExposeSelfInventory");
            Assert.IsNotNull(newAction);
            Assert.AreEqual("CloseInventory", ((ExposeInventoryActionTemplate)newAction.Value).FinishActionUniqueId);
            Assert.AreEqual(1,actual.Interactions.Count);
            Assert.AreEqual(GenericInteractionRulesKeys.All,actual.Interactions.Keys[0]);
            Assert.AreEqual(2, actual.Interactions[GenericInteractionRulesKeys.All].Length);
        }
        
    }
}
