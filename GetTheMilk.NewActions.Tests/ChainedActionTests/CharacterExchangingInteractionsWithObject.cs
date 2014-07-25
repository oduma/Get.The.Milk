using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests.ChainedActionTests
{
    [TestFixture]
    public class CharacterExchangingInteractionsWithObject
    {
        private Character _character;


        private NonCharacterObject _interactionObject;

        [SetUp]
        public void SetUp()
        {
            _character = new Character
            {
                ObjectTypeId = "NPCFriendly",
                Name = new BaseCommon.Noun { Main = "Joe", Narrator = "Joe the plumber" }
            };
        }

        [Test]
        public void CharacterHasDefaultInteractions()
        {
            Assert.AreEqual(1, _character.Interactions.Count);
            ValidateDefaultCharacterInteractions();
            Assert.AreEqual(5, _character.AllActions.Count);
        }

        private void ValidateDefaultCharacterInteractions()
        {
            Assert.AreEqual(GenericInteractionRulesKeys.All, _character.Interactions.Keys[0]);
            Assert.AreEqual(2, _character.Interactions[GenericInteractionRulesKeys.All].Length);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][0].Action);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][0].Reaction);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][1].Action);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][1].Reaction);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][0].Action.Name.UniqueId == "Attack");
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][0].Reaction.Name.UniqueId == "Attack");
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][1].Action.Name.UniqueId == "Attack");
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.All][1].Reaction.Name.UniqueId == "Quit");
            Assert.True(_character.AllActions.Any(a => a.Key == _character.Interactions[GenericInteractionRulesKeys.All][0].Action.Name.UniqueId));
            Assert.True(_character.AllActions.Any(a => a.Key == _character.Interactions[GenericInteractionRulesKeys.All][1].Reaction.Name.UniqueId));
        }

        [Test]
        public void CharacterReceivingInteractionsFromObjectUnderAnyCharacter()
        {

            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor"
            };
            _interactionObject.Interactions = new SortedList<string, Actions.Interactions.Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _character.LoadInteractions(_interactionObject);
            Assert.AreEqual(2, _character.Interactions.Count);
            ValidateDefaultCharacterInteractions();
            int keyIndex = 1;
            ValidateAnyCharacterLoadedInteractions(keyIndex);

            Assert.AreEqual(7, _character.AllActions.Count);

        }

        private void ValidateAnyCharacterLoadedInteractions(int keyIndex)
        {
            Assert.AreEqual("reactor", _character.Interactions.Keys[keyIndex]);
            Assert.AreEqual(2, _character.Interactions["reactor"].Length);
            Assert.IsNotNull(_character.Interactions["reactor"][0].Action);
            Assert.IsNotNull(_character.Interactions["reactor"][0].Reaction);
            Assert.IsNotNull(_character.Interactions["reactor"][1].Action);
            Assert.IsNotNull(_character.Interactions["reactor"][1].Reaction);
            Assert.AreEqual(_character.Interactions["reactor"][0].Action.Name.UniqueId, "Interaction1-Action");
            Assert.AreEqual(_character.Interactions["reactor"][0].Reaction.Name.UniqueId, "Interaction1-Reaction");
            Assert.AreEqual(_character.Interactions["reactor"][1].Action.Name.UniqueId, "Interaction2-Action");
            Assert.AreEqual(_character.Interactions["reactor"][1].Reaction.Name.UniqueId, "Interaction2-Reaction");
            Assert.True(_character.AllActions.Any(a => a.Key == _character.Interactions["reactor"][0].Action.Name.UniqueId));
            Assert.True(_character.AllActions.Any(a => a.Key == _character.Interactions["reactor"][1].Action.Name.UniqueId));
        }

        [Test]
        public void CharacterReceivingInteractionsFromObjectUnderAnyCharacterResponse()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor"
            };

            _interactionObject.Interactions = new SortedList<string, Actions.Interactions.Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());
            _character.LoadInteractions(_interactionObject);
            Assert.AreEqual(2, _character.Interactions.Count);
            ValidateDefaultCharacterInteractions();
            int keyIndex = 1;
            ValidateAnyCharacterResponsesInteractions(keyIndex);

            Assert.AreEqual(7, _character.AllActions.Count);

        }

        private void ValidateAnyCharacterResponsesInteractions(int keyIndex)
        {
            Assert.AreEqual(GenericInteractionRulesKeys.AnyCharacterResponses, _character.Interactions.Keys[keyIndex]);
            Assert.AreEqual(2, _character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].Length);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][0].Action);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][0].Reaction);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][1].Action);
            Assert.IsNotNull(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][1].Reaction);
            Assert.AreEqual(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][0].Action.Name.UniqueId, "Interaction1-Reaction");
            Assert.AreEqual(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][0].Reaction.Name.UniqueId, "Interaction1-ReReaction1");
            Assert.AreEqual(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][1].Action.Name.UniqueId, "Interaction1-Reaction");
            Assert.AreEqual(_character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][1].Reaction.Name.UniqueId, "Interaction1-ReReaction2");
            Assert.True(_character.AllActions.Any(a => a.Key == _character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][0].Reaction.Name.UniqueId));
            Assert.True(_character.AllActions.Any(a => a.Key == _character.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses][1].Reaction.Name.UniqueId));
        }

        [Test]
        public void CharacterReceivingInteractionsFromObjectAllTogether()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor"
            };

            _interactionObject.Interactions = new SortedList<string, Actions.Interactions.Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());

            _character.LoadInteractions(_interactionObject);
            Assert.AreEqual(3, _character.Interactions.Count);
            ValidateDefaultCharacterInteractions();
            ValidateAnyCharacterLoadedInteractions(2);
            ValidateAnyCharacterResponsesInteractions(1);
            Assert.AreEqual(9, _character.AllActions.Count);

        }

        private static Interaction[] GetInteractions()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new OneObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Action",Past="Interaction1-Action",Present="Interaction1-Action"},PerformerType=typeof(ExplodePerformer)}, 
                Reaction = new OneObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(ExplodePerformer)} 
            },
            new Interaction 
            { 
                Action = new OneObjectActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction2-Action",Past="Interaction2-Action",Present="Interaction2-Action"},PerformerType=typeof(ExplodePerformer)}, 
                Reaction = new OneObjectActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction2-Reaction",Past="Interaction2-Reaction",Present="Interaction2-Reaction"},PerformerType=typeof(ExplodePerformer)} 
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
