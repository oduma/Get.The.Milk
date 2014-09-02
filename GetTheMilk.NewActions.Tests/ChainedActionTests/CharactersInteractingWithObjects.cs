using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
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
    public class CharactersInteractingWithObjects : BaseTestClass
    {
        private Character _character;

        private NonCharacterObject _interactionObject;
        [SetUp]
        public void SetUp()
        {
            _character = new Character { ObjectTypeId = "NPCFriendly", Name = new Noun { Main = "chr", Narrator = "chr" } };

            var objAction = ObjectActionsFactory.CreateObjectAction("NPCFriendly");
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
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());
            _character.LoadInteractions(_interactionObject, _interactionObject.GetType());
            var actionOne = _character.CreateNewInstanceOfAction<OneObjectActionTemplate>("Interaction1-Action");

            var result = _character.PerformAction(actionOne);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNotNull(result.ExtraData);
            Assert.AreEqual(2, ((List<BaseActionTemplate>)result.ExtraData).Count);
            Assert.AreEqual(2, ((IEnumerable<BaseActionTemplate>)result.ExtraData).Count(a => a.PerformerType == typeof(ExplodePerformer)));
            Assert.AreEqual(typeof(ObjectResponseActionTemplatePerformer), result.ForAction.PerformerType);

        }

        [Test]
        public void CharacterObjectUseOnObjectObjectResponseAndStop()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractionsWithUse());
            _character.LoadInteractions(_interactionObject, _interactionObject.GetType());
            var actionTwo = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Interaction2-Action");
            actionTwo.ActiveObject = _character.Inventory[0];
            var result = _character.PerformAction(actionTwo);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNull(result.ExtraData);
            Assert.AreEqual(typeof(ObjectResponseActionTemplatePerformer), result.ForAction.PerformerType);

        }

        [Test]
        public void CharacterObjectUseOnObjectObjectResponseDoubleAndStop()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor",
                AllowsIndirectTemplateAction = TestHelper.AllowsIndirectEverything,
                AllowsTemplateAction = TestHelper.AllowsEverything
            };
            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractionsWithUse());
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponsesWithUse());
            _character.LoadInteractions(_interactionObject, _interactionObject.GetType());
            var actionOne = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Interaction1-Action");
            actionOne.ActiveObject = _character.Inventory[0];
            var result = _character.PerformAction(actionOne);

            Assert.IsNotNull(result);
            Assert.AreEqual(ActionResultType.Ok, result.ResultType);
            Assert.IsNotNull(result.ExtraData);
            Assert.AreEqual(2, ((List<BaseActionTemplate>)result.ExtraData).Count);
            Assert.AreEqual(2, ((IEnumerable<BaseActionTemplate>)result.ExtraData).Count(a => a.PerformerType == typeof(ObjectUseOnObjectActionTemplatePerformer)));
            Assert.AreEqual(typeof(ObjectResponseActionTemplatePerformer), result.ForAction.PerformerType);


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

        private static Interaction[] GetInteractionsWithUse()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new ObjectUseOnObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Action",Past="Interaction1-Action",Present="Interaction1-Action"},PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer)}, 
                Reaction = new ObjectResponseActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)} 
            },
            new Interaction 
            { 
                Action = new ObjectUseOnObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction2-Action",Past="Interaction2-Action",Present="Interaction2-Action"},PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer)}, 
                Reaction = new ObjectResponseActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction2-Reaction",Past="Interaction2-Reaction",Present="Interaction2-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)} 
            }
            };
        }
        private static Interaction[] GetInteractionsResponses()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new ObjectResponseActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)}, 
                Reaction = new OneObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction1",Past="Interaction1-ReReaction1",Present="Interaction1-ReReaction1"},PerformerType=typeof(ExplodePerformer)} 
            },
            new Interaction 
            { 
                Action = new ObjectResponseActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)}, 
                Reaction = new OneObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction2",Past="Interaction1-ReReaction2",Present="Interaction1-ReReaction2"},PerformerType=typeof(ExplodePerformer)} 
            }
            };
        }
        private static Interaction[] GetInteractionsResponsesWithUse()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new ObjectResponseActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)}, 
                Reaction = new ObjectUseOnObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction1",Past="Interaction1-ReReaction1",Present="Interaction1-ReReaction1"},PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer)} 
            },
            new Interaction 
            { 
                Action = new ObjectResponseActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ObjectResponseActionTemplatePerformer)}, 
                Reaction = new ObjectUseOnObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction2",Past="Interaction1-ReReaction2",Present="Interaction1-ReReaction2"},PerformerType=typeof(ObjectUseOnObjectActionTemplatePerformer)} 
            }
            };
        }


    }
}
