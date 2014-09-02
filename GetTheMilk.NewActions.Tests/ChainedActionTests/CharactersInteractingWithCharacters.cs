using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Factories;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GetTheMilk.NewActions.Tests.ChainedActionTests
{
    [TestFixture]
    public class CharactersInteractingWithCharacters : BaseTestClass
    {
        private Character _activeCharacter;

        private Character _interactionCharacter;
        [SetUp]
        public void SetUp()
        {
            _activeCharacter = new Character { ObjectTypeId = "NPCFriendly", Name = new Noun { Main = "MyActiveFriend", Narrator = "My Active Friend" } };

            _activeCharacter.AllowsTemplateAction = TestHelper.AllowsEverything;
            _activeCharacter.AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything;

            var activeObject = new Weapon
            {
                Name = new Noun { Main = "testweapon", Narrator = "test weapon" },
                AllowsTemplateAction = TestHelper.AllowsEverything,
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                DefensePower=2,
                AttackPower=5,
                WeaponTypes = new WeaponType[] { WeaponType.Attack,WeaponType.Deffense}
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
                Name = new Noun { Main = "reactor", Narrator = "reactor" },
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
                Action = new TwoCharactersActionTemplate { Name= new Verb{UniqueId="Interaction1-Action",Past="Interaction1-Action",Present="Interaction1-Action"},PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new ObjectResponseActionTemplate { Name=new Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new Verb{UniqueId="Interaction2-Action",Past="Interaction2-Action",Present="Interaction2-Action"},Message="sell me something",PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new ExposeInventoryActionTemplate { Name=new Verb{UniqueId="Interaction2-Reaction",Past="Interaction2-Reaction",Present="Interaction2-Reaction"},PerformerType=typeof(ExposeInventoryActionTemplatePerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new Verb{UniqueId="Interaction3-Action",Past="Interaction3-Action",Present="Interaction3-Action"},Message="Hello",PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new TwoCharactersActionTemplate { Name=new Verb{UniqueId="Interaction3-Reaction",Past="Interaction3-Reaction",Present="Interaction3-Reaction"},Message="Hi", PerformerType=typeof(CommunicateActionPerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new Verb{UniqueId="Interaction4-Action",Past="Interaction4-Action",Present="Interaction4-Action"},PerformerType=typeof(InitiateHostilitiesActionPerformer)}, 
                Reaction = new TwoCharactersActionTemplate { Name=new Verb{UniqueId="Interaction4-Reaction",Past="Interaction4-Reaction",Present="Interaction4-Reaction"},PerformerType=typeof(InitiateHostilitiesActionPerformer)} 
            }
            };
        }

        private static Interaction[] GetInteractionsResponses()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name=new Verb{UniqueId="Interaction3-Reaction",Past="Interaction3-Reaction",Present="Interaction3-Reaction"},Message="Hi", PerformerType=typeof(CommunicateActionPerformer)} , 
                Reaction = new TwoCharactersActionTemplate { Name=new Verb{UniqueId="Interaction3-ReReaction1",Past="Interaction3-ReReaction1",Present="Interaction3-ReReaction1"},Message="I'm good!", PerformerType=typeof(CommunicateActionPerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name=new Verb{UniqueId="Interaction3-Reaction",Past="Interaction3-Reaction",Present="Interaction3-Reaction"},Message="Hi", PerformerType=typeof(CommunicateActionPerformer)} , 
                Reaction = new TwoCharactersActionTemplate { Name=new Verb{UniqueId="Interaction3-ReReaction2",Past="Interaction3-ReReaction2",Present="Interaction3-ReReaction2"},Message="I'm bad!", PerformerType=typeof(CommunicateActionPerformer)} 
            }
            };
        }

        [Test]
        public void CharacterCommunicateCharacterCommunicateAndStop()
        {
            _interactionCharacter = new Character
            {
                Name = new Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "NPCFriendly",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionCharacter.Interactions = new SortedList<string, Interaction[]>();
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _activeCharacter.LoadInteractions(_interactionCharacter, _interactionCharacter.GetType());
            var actionThree = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Interaction3-Action");

            var result = _activeCharacter.PerformAction(actionThree);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNull(result.ExtraData);
            Assert.AreEqual(typeof(CommunicateActionPerformer), result.ForAction.PerformerType);
            Assert.AreEqual("Hi", ((TwoCharactersActionTemplate)result.ForAction).Message);


        }
        [Test]
        public void CharacterCommunicateCharacterCommunicateDouble()
        {
            _interactionCharacter = new Character
            {
                Name = new Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "NPCFriendly",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionCharacter.Interactions = new SortedList<string, Interaction[]>();
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());
            _activeCharacter.LoadInteractions(_interactionCharacter, _interactionCharacter.GetType());
            var actionThree = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Interaction3-Action");

            var result = _activeCharacter.PerformAction(actionThree);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNotNull(result.ExtraData);
            Assert.AreEqual(2, ((List<BaseActionTemplate>)result.ExtraData).Count);
            Assert.AreEqual(2, ((IEnumerable<BaseActionTemplate>)result.ExtraData).Count(a => a.PerformerType == typeof(CommunicateActionPerformer)));

            Assert.AreEqual(typeof(CommunicateActionPerformer), result.ForAction.PerformerType);
            Assert.AreEqual("Hi", ((TwoCharactersActionTemplate)result.ForAction).Message);

        }
        [Test]
        public void CharacterInitiateHostilitiesCharacterInitiateHostilities()
        {
            _interactionCharacter = new Character
            {
                Name = new Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "NPCFriendly",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionCharacter.Interactions = new SortedList<string, Interaction[]>();
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _activeCharacter.LoadInteractions(_interactionCharacter, _interactionCharacter.GetType());
            var actionFour = _activeCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>("Interaction4-Action");

            var result = _activeCharacter.PerformAction(actionFour);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNull(result.ExtraData);
            Assert.AreEqual(typeof(InitiateHostilitiesActionPerformer), result.ForAction.PerformerType);
        }
    }
}
