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
                "{\"Health\":0,\"Experience\":1,\"Walet\":{\"MaxCapacity\":200,\"CurrentCapacity\":20},\"Range\":1,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":{\"Main\":\"Player\",\"Narrator\":\"you\"},\"CellNumber\":0,\"BlockMovement\":true,\"ObjectTypeId\":\"Player\",\"CloseUpMessage\":null}",
                saveResult.Core);
            Assert.AreEqual(
                "[{\"DestroyActiveObject\":true,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ObjectUseOnObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Open\",\"Present\":\"open\",\"Past\":\"opened\"},\"Category\":\"ObjectUseOnObjectActionTemplate\",\"StartingAction\":true,\"FinishTheInteractionOnExecution\":true,\"TargetObject\":null,\"TargetCharacter\":null}]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"20\"}",
                saveResult.PackagedInventory);
            Assert.AreEqual(
    "{\"All\":[{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null}},{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.TwoCharactersActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null}}]}",
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
                        "{\"Health\":0,\"Experience\":1,\"Walet\":{\"MaxCapacity\":200,\"CurrentCapacity\":20},\"Range\":1,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":{\"Main\":\"Player\",\"Narrator\":\"you\"},\"CellNumber\":0,\"BlockMovement\":true,\"ObjectTypeId\":\"Player\",\"CloseUpMessage\":null}",
                    ActionTemplates = "[{\"DestroyActiveObject\":true,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ObjectUseOnObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Open\",\"Present\":\"open\",\"Past\":\"opened\"},\"Category\":\"ObjectUseOnObjectActionTemplate\",\"StartingAction\":true,\"FinishTheInteractionOnExecution\":true,\"TargetObject\":null,\"TargetCharacter\":null}]",
                    PackagedInventory = "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"20\"}",
                    Interactions = "{\"All\":[{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null}},{\"Action\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.AttackActionPerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Attack\",\"Present\":\"attack\",\"Past\":\"attacked\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null},\"Reaction\":{\"Message\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.TwoCharactersActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Name\":{\"UniqueId\":\"Quit\",\"Present\":\"quit\",\"Past\":\"quited\"},\"Category\":\"TwoCharactersActionTemplate\",\"StartingAction\":false,\"FinishTheInteractionOnExecution\":false,\"TargetObject\":null,\"TargetCharacter\":null}}]}"
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
