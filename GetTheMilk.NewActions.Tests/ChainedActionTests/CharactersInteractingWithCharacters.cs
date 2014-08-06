using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using System.Collections.Generic;

namespace GetTheMilk.NewActions.Tests.ChainedActionTests
{
    [TestFixture]
    public class CharactersInteractingWithCharacters
    {
        private Character _activeCharacter;

        private Character _interactionCharacter;
        [SetUp]
        public void SetUp()
        {
            _activeCharacter = new Character { ObjectTypeId = "NPCFriendly" };
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("NPCFriendly");
            _activeCharacter.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _activeCharacter.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            var activeObject = new Weapon
            {
                Name = new Noun { Main = "testweapon", Narrator = "test weapon" },
                AllowsTemplateAction = TestHelper.AllowsEverything,
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                DefensePower=2,
                AttackPower=5
            };

            _activeCharacter.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _activeCharacter.Inventory.Add(activeObject);
            _activeCharacter.ActiveAttackWeapon = activeObject;
            _activeCharacter.ActiveDefenseWeapon = activeObject;
            _activeCharacter.Health = 100;
        }


        [Test]
        public void CharacterCommunicateCharacterExposeInventory()
        {
            _interactionCharacter = new Character
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "NPCFriendly",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionCharacter.Interactions = new SortedList<string, Interaction[]>();
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _activeCharacter.LoadInteractions(_interactionCharacter, _interactionCharacter.GetType());
            var actionTwo = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Interaction2-Action");

            var result = _activeCharacter.PerformAction(actionTwo);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNotNull(result.ExtraData);
            Assert.AreEqual(typeof(InventoryExtraData), result.ExtraData.GetType());
            Assert.AreEqual(typeof(ExposeInventoryActionTemplatePerformer), result.ForAction.PerformerType);

        }

        private static Interaction[] GetInteractions()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Action",Past="Interaction1-Action",Present="Interaction1-Action"},PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new ObjectResponseActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction2-Action",Past="Interaction2-Action",Present="Interaction2-Action"},Message="sell me something",PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new ExposeInventoryActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction2-Reaction",Past="Interaction2-Reaction",Present="Interaction2-Reaction"},PerformerType=typeof(ExposeInventoryActionTemplatePerformer)} 
            }
            };
        }
        [Test]
        public void CharacterCommunicateCharacterCommunicateAndStop()
        {

        }
        [Test]
        public void CharacterCommunicateCharacterCommunicateDouble()
        {

        }
        [Test]
        public void CharacterInitiateHostilitiesCharacterInitiateHostilities()
        {

        }
        [Test]
        public void CharacterAttackCharacterAttack()
        {

        }
    }
}
