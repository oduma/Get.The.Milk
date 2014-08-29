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
    public class LoadingActionTemplatesInPlayerTests : BaseTestClass
    {
        [Test]
        public void LoadPlayersDefaultActionTemplates()
        {
            var player = new Player();
            Assert.IsNotNull(player.AllActions);
            Assert.AreEqual(7, player.AllActions.Count);
        }

        [Test]
        public void SaveAModifiedPlayer()
        {
            var player = new Player();
            player.AddAvailableAction(new ObjectUseOnObjectActionTemplate
            {
                PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer),
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
                PercentOfHealthFailurePenalty = 0
            });
            var saveResult = player.Save();
            Assert.AreEqual(
                "{\"Health\":0,\"Experience\":1,\"Walet\":{\"MaxCapacity\":200,\"CurrentCapacity\":20},\"Range\":1,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":{\"Main\":\"Player\",\"Narrator\":\"you\",\"Description\":null},\"CellNumber\":0,\"BlockMovement\":true,\"ObjectTypeId\":\"Player\"}",
                saveResult.Core);
            Assert.AreEqual(
                "[{\"DestroyActiveObject\":true,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"Name\":{\"UniqueId\":\"Open\",\"Present\":\"open\",\"Past\":\"opened\"},\"Category\":\"ObjectUseOnObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ObjectUseOnObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}]",
                saveResult.ActionTemplates);
            Assert.AreEqual(
                "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"20\"}",
                saveResult.PackagedInventory);
            Assert.AreEqual(
    "{}",
    saveResult.Interactions);


        }

        [Test]
        public void LoadAModifiedPlayer()
        {
            var expected = new Player();
            expected.AddAvailableAction(new ObjectUseOnObjectActionTemplate
            {
                PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer),
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
                PercentOfHealthFailurePenalty = 0
            });
            var actual =
                Character.Load<Player>(new ContainerWithActionsPackage
                {
                    Core =
                        "{\"Health\":0,\"Experience\":1,\"Walet\":{\"MaxCapacity\":200,\"CurrentCapacity\":20},\"Range\":1,\"ActiveDefenseWeapon\":null,\"ActiveAttackWeapon\":null,\"Name\":{\"Main\":\"Player\",\"Narrator\":\"you\",\"Description\":null},\"CellNumber\":0,\"BlockMovement\":true,\"ObjectTypeId\":\"Player\"}",
                    ActionTemplates = "[{\"DestroyActiveObject\":true,\"DestroyTargetObject\":true,\"ChanceOfSuccess\":100,\"PercentOfHealthFailurePenalty\":0,\"Name\":{\"UniqueId\":\"Open\",\"Present\":\"open\",\"Past\":\"opened\"},\"Category\":\"ObjectUseOnObjectActionTemplate\",\"StartingAction\":true,\"TargetObject\":null,\"TargetCharacter\":null,\"PerformerType\":\"GetTheMilk.Actions.ActionPerformers.Base.ObjectUseOnObjectActionTemplatePerformer, GetTheMilk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}]",
                    PackagedInventory = "{\"Contents\":\"[]\",\"InventoryType\":\"1\",\"MaximumCapacity\":\"20\"}",
                    Interactions = "{}"
                });
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AllActions.Count, actual.AllActions.Count);
            var newAction = actual.AllActions.FirstOrDefault(a => a.Value.GetType() == typeof(ObjectUseOnObjectActionTemplate));
            Assert.IsNotNull(newAction);

        }

    }
}
