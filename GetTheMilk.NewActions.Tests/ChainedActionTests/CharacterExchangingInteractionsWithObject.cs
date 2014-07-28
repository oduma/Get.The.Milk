﻿using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
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
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            Assert.AreEqual(5, _character.AllActions.Count);
        }
        [Test]
        public void CharacterReceivingInteractionsFromObjectUnderAnyCharacter()
        {

            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor"
            };
            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _character.LoadInteractions(_interactionObject);
            Assert.AreEqual(2, _character.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            
            TestHelper.ValidateAnyCharacterLoadedInteractions(_character, 1,_interactionObject.Name.Main);

            TestHelper.ValidateAnyCharacterLoadedInteractions(_interactionObject, 0, GenericInteractionRulesKeys.AnyCharacter);

            TestHelper.CheckAllActionsAfterInteractionsLoad(_character,_interactionObject.Name.Main,_interactionObject,GenericInteractionRulesKeys.AnyCharacter);

            Assert.AreEqual(7, _character.AllActions.Count);

        }

        [Test]
        public void CharacterReceivingInteractionsFromObjectUnderAnyCharacterResponse()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor"
            };

            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());
            _character.LoadInteractions(_interactionObject);
            Assert.AreEqual(2, _character.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            TestHelper.ValidateAnyCharacterResponsesInteractions(_character,1,"reactor_Responses");
            TestHelper.ValidateAnyCharacterResponsesInteractions(_interactionObject, 0, GenericInteractionRulesKeys.AnyCharacterResponses);

            TestHelper.CheckAllActionsAfterInteractionsLoad(_interactionObject, GenericInteractionRulesKeys.AnyCharacterResponses, _character, "reactor_Responses");

            Assert.AreEqual(8, _character.AllActions.Count);

        }


        [Test]
        public void CharacterReceivingInteractionsFromObjectAllTogether()
        {
            _interactionObject = new NonCharacterObject
            {
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "reactor" },
                ObjectTypeId = "Decor"
            };

            _interactionObject.Interactions = new SortedList<string, Interaction[]>();
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _interactionObject.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());

            _character.LoadInteractions(_interactionObject);
            Assert.AreEqual(3, _character.Interactions.Count);
            Assert.AreEqual(2, _interactionObject.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            TestHelper.ValidateAnyCharacterLoadedInteractions(_character,1,_interactionObject.Name.Main);
            TestHelper.ValidateAnyCharacterLoadedInteractions(_interactionObject, 0, GenericInteractionRulesKeys.AnyCharacter);
            TestHelper.CheckAllActionsAfterInteractionsLoad(_character, _interactionObject.Name.Main, _interactionObject, GenericInteractionRulesKeys.AnyCharacter);

            TestHelper.ValidateAnyCharacterResponsesInteractions(_character, 2, _interactionObject.Name.Main +"_Responses");
            TestHelper.CheckAllActionsAfterInteractionsLoad(_interactionObject, GenericInteractionRulesKeys.AnyCharacterResponses, _character, "reactor_Responses");
            Assert.AreEqual(10, _character.AllActions.Count);

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