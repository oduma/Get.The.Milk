using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.BulkTemplatesTests
{
    [TestFixture]
    public class LoadingActionTemplatesInPlayerTests
    {
        [Test]
        public void LoadPlayersDefaultActionTemplates()
        {
            var player = new Player();
            Assert.IsNotNull(player.AllActions);
            Assert.AreEqual(8, player.AllActions.Count);
            Assert.AreEqual(2,
                            player.AllActions.Count(
                                a =>
                                (a.Key == "Attack" || a.Key == "Quit")));
        }

        [Test]
        public void SaveAModifiedPlayer()
        {
            var player = new Player();
            player.AddAvailableAction(new ObjectUseOnObjectActionTemplate
            {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId = "Open",
                    Past = "opened",
                    Present = "open"
                },
                ChanceOfSuccess = ChanceOfSuccess.Full,
                DestroyActiveObject = true,
                DestroyTargetObject = true,
                StartingAction = true,
                PercentOfHealthFailurePenalty = 0,
                FinishTheInteractionOnExecution = true
            });
            var saveResult = player.Save();
            Assert.AreEqual(
                "{\"TwoCharactersActionsPerformers\":[{\"Name\":{\"Identifier\":\"AcceptQuit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"AllowPass\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Attack\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Meet\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Talk\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"InitiateHostilities\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Quit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"}],\"Health\":0,\"Experience\":1,\"Walet\":{\"MaxCapacity\":200,\"CurrentCapacity\":20},\"Range\":1,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":{\"Main\":\"Player\",\"Narrator\":\"you\"},\"CellNumber\":0,\"BlockMovement\":true,\"ObjectTypeId\":\"Player\",\"CloseUpMessage\":null}",
                saveResult.Core);
            Assert.AreEqual(
                "[{\"DestroyActiveObject\":true,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"Category\":\"ObjectUseOnObjectActionTemplate\",\"Name\":{\"Identifier\":\"Open\",\"Present\":\"open\",\"Past\":\"opened\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":true,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"20\"}",
                saveResult.PackagedInventory);
            Assert.AreEqual(
    "{\"All\":[{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}},{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}}]}",
    saveResult.Interactions);


        }

        [Test]
        public void LoadAModifiedPlayer()
        {
            var expected = new Player();
            expected.AddAvailableAction(new ObjectUseOnObjectActionTemplate
            {
                CurrentPerformer = new ObjectUseOnObjectActionTemplatePerformer(),
                Name = new Verb
                {
                    UniqueId = "Open",
                    Past = "opened",
                    Present = "open"
                },
                ChanceOfSuccess = ChanceOfSuccess.Full,
                DestroyActiveObject = true,
                DestroyTargetObject = true,
                StartingAction = true,
                PercentOfHealthFailurePenalty = 0,
                FinishTheInteractionOnExecution = true
            });
            var actual =
                Character.Load<Player>(new ContainerWithActionsPackage
                {
                    Core =
                        "{\"TwoCharactersActionsPerformers\":[{\"Name\":{\"Identifier\":\"AcceptQuit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"AllowPass\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Attack\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Meet\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Talk\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"InitiateHostilities\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"},{\"Name\":{\"Identifier\":\"Quit\",\"Present\":null,\"Past\":null},\"Category\":\"TwoCharactersActionTemplate\"}],\"Health\":0,\"Experience\":1,\"Walet\":{\"MaxCapacity\":200,\"CurrentCapacity\":20},\"Range\":1,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":{\"Main\":\"Player\",\"Narrator\":\"you\"},\"CellNumber\":0,\"BlockMovement\":true,\"ObjectTypeId\":\"Player\",\"CloseUpMessage\":null}",
                    ActionTemplates = "[{\"DestroyActiveObject\":true,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"Category\":\"ObjectUseOnObjectActionTemplate\",\"Name\":{\"Identifier\":\"Open\",\"Present\":\"open\",\"Past\":\"opened\"},\"StartingAction\":true,\"FinishTheInteractionOnExecution\":true,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}]",
                    PackagedInventory = "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"20\"}",
                    Interactions = "{\"All\":[{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}},{\"Action\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null},\"Reaction\":{\"Message\":null,\"Category\":\"TwoCharactersActionTemplate\",\"Name\":{\"Identifier\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"ActiveObject\":null,\"TargetCharacter\":null,\"ActiveCharacter\":null}}]}"
                });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions.Count, actual.AllActions.Count);
            var newAction = actual.AllActions.FirstOrDefault(a => a.Value.GetType() == typeof(ObjectUseOnObjectActionTemplate));
            Assert.IsNotNull(newAction);
            Assert.AreEqual(1, actual.Interactions.Count);
            Assert.AreEqual(GenericInteractionRulesKeys.All, actual.Interactions.Keys[0]);
            Assert.AreEqual(2, actual.Interactions[GenericInteractionRulesKeys.All].Length);

        }

    }
}
