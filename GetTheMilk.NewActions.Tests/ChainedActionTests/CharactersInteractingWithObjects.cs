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
    public class CharactersInteractingWithObjects
    {
        private Character _character = new Character { ObjectTypeId = "NPCFriendly" };

        private NonCharacterObject _interactionObject;
        [SetUp]
        public void SetUp()
        {
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            var activeObject = new Tool
                                   {
                                       Name = new Noun {Main = "testtool", Narrator = "test tool"},
                                       AllowsTemplateAction = TestHelper.AllowsEverything,
                                       AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything
                                   };

            _character.Inventory = new Inventory { MaximumCapacity = 2, InventoryType = InventoryType.CharacterInventory };
            _character.Inventory.Add(activeObject);
            _character.Health = 100;
        }

        [Test]
        public void CharacterOneObjectObjectResponseAndStop()
        {
            _interactionObject= new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor",
                AllowsIndirectTemplateAction=TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction=TestHelper.AllowsEverything
            };
            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _character.LoadInteractions(_interactionObject, _interactionObject.GetType());
            var actionTwo = _character.CreateNewInstanceOfAction<OneObjectActionTemplate>("Interaction2-Action");

            var result = _character.PerformAction(actionTwo);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNull(result.ExtraData);
            Assert.AreEqual(typeof(ObjectResponseActionTemplatePerformer), result.ForAction.PerformerType);
        }

        [Test]
        public void CharacterOneObjectObjectResponseDoubleAndStop()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _character.LoadInteractions(_interactionObject, _interactionObject.GetType());
            var actionOne = _character.CreateNewInstanceOfAction<OneObjectActionTemplate>("Interaction1-Action");

            var result = _character.PerformAction(actionOne);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNull(result.ExtraData);
            Assert.AreEqual(typeof(ObjectResponseActionTemplatePerformer), result.ForAction.PerformerType);

        }

        [Test]
        public void CharacterObjectUseOnObjectObjectResponseAndStop()
        {

        }

        [Test]
        public void CharacterObjectUseOnObjectObjectResponseDoubleAndStop()
        {

        }

        private static Interaction[] GetInteractions()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new OneObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Action",Past="Interaction1-Action",Present="Interaction1-Action"},PerformerType=typeof(ExplodePerformer)}, 
                Reaction = new ObjectResponseActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)} 
            },
            new Interaction 
            { 
                Action = new OneObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction2-Action",Past="Interaction2-Action",Present="Interaction2-Action"},PerformerType=typeof(ExplodePerformer)}, 
                Reaction = new ObjectResponseActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction2-Reaction",Past="Interaction2-Reaction",Present="Interaction2-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)} 
            }
            };
        }

        private static Interaction[] GetInteractionsResponses()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new OneObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ExplodePerformer)}, 
                Reaction = new OneObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction1",Past="Interaction1-ReReaction1",Present="Interaction1-ReReaction1"},PerformerType=typeof(ExplodePerformer)} 
            },
            new Interaction 
            { 
                Action = new OneObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ExplodePerformer)}, 
                Reaction = new OneObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction2",Past="Interaction1-ReReaction2",Present="Interaction1-ReReaction2"},PerformerType=typeof(ExplodePerformer)} 
            }
            };
        }


    }
}
