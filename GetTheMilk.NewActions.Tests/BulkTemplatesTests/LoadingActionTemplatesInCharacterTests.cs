using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;
using NUnit.Framework;

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
                                             FinishActionCategory = typeof (NoObjectActionTemplate),
                                             FinishActionType = "CloseInventory"
                                         });
            var saveResult = character.Save();
            Assert.AreEqual(
                "{\"TwoCharactersActionsPerformers\":[{\"Name\":{\"Identifier\":\"AcceptQuit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"AllowPass\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Attack\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Meet\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Talk\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"InitiateHostilities\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Quit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"}],\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\",\"CloseUpMessage\":null}",
                saveResult.Core);
            Assert.AreEqual(
                "[{\"FinishActionType\":\"CloseInventory\",\"FinishActionCategory\":\"GetTheMilk.Actions.ActionTemplates.NoObjectActionTemplate, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"SelfInventory\":false,\"Category\":\"ExposeInventoryActionTemplate\",\"Name\":{\"Identifier\":\"ExposeInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"0\"}",
                saveResult.PackagedInventory);
            Assert.AreEqual(
                "{\"All\":[{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}},{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}}]}",
                saveResult.Interactions);

        }

        [Test]
        public void LoadAModifiedCharacter()
        {
            var expected = new Character();
            expected.AddAvailableAction(new ExposeInventoryActionTemplate
            {
                FinishActionCategory = typeof(NoObjectActionTemplate),
                FinishActionType = "CloseInventory"
            });
            var actual =
                Character.Load<Character>(new ContainerWithActionsPackage
                                              {
                                                  Core =
                                                      "{\"TwoCharactersActionsPerformers\":[{\"Name\":{\"Identifier\":\"AcceptQuit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"AllowPass\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Attack\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Meet\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Talk\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"InitiateHostilities\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Quit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"}],\"Health\":0,\"Experience\":0,\"Walet\":{\"MaxCapacity\":0,\"CurrentCapacity\":0},\"Range\":0,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":null,\"CellNumber\":0,\"BlockMovement\":false,\"ObjectTypeId\":\"NPCFriendly\",\"CloseUpMessage\":null}",
                                                  ActionTemplates = "[{\"FinishActionType\":\"CloseInventory\",\"FinishActionCategory\":\"GetTheMilk.Actions.ActionTemplates.NoObjectActionTemplate, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"SelfInventory\":false,\"Category\":\"ExposeInventoryActionTemplate\",\"Name\":{\"Identifier\":\"ExposeInventory\",\"Present\":\"expose inventory\",\"Past\":\"exposed inventory\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}]",
                                                  PackagedInventory = "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"0\"}",
                                                  Interactions = "{\"All\":[{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}},{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}}]}"
                                              });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions.Count,actual.AllActions.Count);
            var newAction = actual.AllActions.FirstOrDefault(a => a.Key == "ExposeInventory");
            Assert.IsNotNull(newAction);
            Assert.AreEqual(typeof(NoObjectActionTemplate),((ExposeInventoryActionTemplate)newAction.Value).FinishActionCategory);
            Assert.AreEqual("CloseInventory", ((ExposeInventoryActionTemplate)newAction.Value).FinishActionType);
            Assert.AreEqual(1,actual.Interactions.Count);
            Assert.AreEqual(GenericInteractionRulesKeys.All,actual.Interactions.Keys[0]);
            Assert.AreEqual(2, actual.Interactions[GenericInteractionRulesKeys.All].Length);
        }
        
    }
}
