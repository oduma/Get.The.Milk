﻿using GetTheMilk.Actions.ActionPerformers;
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
    public class CharacterExchangingInteractionsWithCharacter
    {
        private Character _character;


        private Character _interactionCharacter;

        [SetUp]
        public void SetUp()
        {
            _character = new Character
            {
                ObjectTypeId = "NPCFriendly",
                Name = new BaseCommon.Noun { Main = "Joe", Narrator = "Joe the plumber" }
            };
            _interactionCharacter = new Character
            {
                ObjectTypeId = "NPCFriendly",
                Name = new BaseCommon.Noun { Main = "reactor", Narrator = "Joe the plumber" }
            };
        }

        [Test]
        public void BothCharactersHaveDefaultInteractions()
        {
            Assert.AreEqual(1, _character.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            Assert.AreEqual(5, _character.AllActions.Count);

            Assert.AreEqual(1, _interactionCharacter.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_interactionCharacter);
            Assert.AreEqual(5, _interactionCharacter.AllActions.Count);

        }


        [Test]
        public void CharacterReceivingInteractionsFromCharacterUnderAnyCharacter()
        {

            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _character.LoadInteractions(_interactionCharacter);
            Assert.AreEqual(2, _character.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            TestHelper.ValidateDefaultCharacterInteractions(_interactionCharacter);
            int keyIndex = 1;
            TestHelper.ValidateAnyCharacterLoadedInteractions(_character, keyIndex,_interactionCharacter.Name.Main);

            TestHelper.ValidateAnyCharacterLoadedInteractions(_interactionCharacter, keyIndex, GenericInteractionRulesKeys.AnyCharacter);

            TestHelper.CheckAllActionsAfterInteractionsLoad(_character, _interactionCharacter.Name.Main, _interactionCharacter, GenericInteractionRulesKeys.AnyCharacter);
            Assert.AreEqual(7, _character.AllActions.Count);

            Assert.AreEqual(7, _interactionCharacter.AllActions.Count);

        }

        [Test]
        public void CharacterReceivingInteractionsFromCharacterUnderAnyCharacterResponse()
        {
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());
            _character.LoadInteractions(_interactionCharacter);
            Assert.AreEqual(2, _character.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            TestHelper.ValidateDefaultCharacterInteractions(_interactionCharacter);

            TestHelper.ValidateAnyCharacterResponsesInteractions(_character, 1, "reactor_Responses");
            TestHelper.ValidateAnyCharacterResponsesInteractions(_interactionCharacter, 1, GenericInteractionRulesKeys.AnyCharacterResponses);

            TestHelper.CheckAllActionsAfterInteractionsLoad(_interactionCharacter, GenericInteractionRulesKeys.AnyCharacterResponses, _character, "reactor_Responses");

            
            Assert.AreEqual(8, _character.AllActions.Count);

            Assert.AreEqual(6, _interactionCharacter.AllActions.Count);

        }


        [Test]
        public void CharacterReceivingInteractionsFromObjectAllTogether()
        {
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, GetInteractions());
            _interactionCharacter.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, GetInteractionsResponses());

            _character.LoadInteractions(_interactionCharacter);
            Assert.AreEqual(3, _character.Interactions.Count);
            TestHelper.ValidateDefaultCharacterInteractions(_character);
            TestHelper.ValidateAnyCharacterLoadedInteractions(_character, 1, _interactionCharacter.Name.Main);

            TestHelper.ValidateAnyCharacterLoadedInteractions(_interactionCharacter, 1, GenericInteractionRulesKeys.AnyCharacter);

            TestHelper.CheckAllActionsAfterInteractionsLoad(_character, _interactionCharacter.Name.Main, _interactionCharacter, GenericInteractionRulesKeys.AnyCharacter);
            TestHelper.ValidateAnyCharacterResponsesInteractions(_character, 2, "reactor_Responses");
            TestHelper.ValidateAnyCharacterResponsesInteractions(_interactionCharacter, 2, GenericInteractionRulesKeys.AnyCharacterResponses);

            TestHelper.CheckAllActionsAfterInteractionsLoad(_interactionCharacter, GenericInteractionRulesKeys.AnyCharacterResponses, _character, "reactor_Responses");


            Assert.AreEqual(10, _character.AllActions.Count);
            Assert.AreEqual(7, _interactionCharacter.AllActions.Count);

        }

        private static Interaction[] GetInteractions()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Action",Past="Interaction1-Action",Present="Interaction1-Action"},PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new TwoCharactersActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(CommunicateActionPerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction2-Action",Past="Interaction2-Action",Present="Interaction2-Action"},PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new TwoCharactersActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction2-Reaction",Past="Interaction2-Reaction",Present="Interaction2-Reaction"},PerformerType=typeof(CommunicateActionPerformer)} 
            }
            };
        }

        private static Interaction[] GetInteractionsResponses()
        {
            return new Interaction[] { new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new TwoCharactersActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction1",Past="Interaction1-ReReaction1",Present="Interaction1-ReReaction1"},PerformerType=typeof(CommunicateActionPerformer)} 
            },
            new Interaction 
            { 
                Action = new TwoCharactersActionTemplate { Name= new BaseCommon.Verb{UniqueId="Interaction1-Reaction",Past="Interaction1-Reaction",Present="Interaction1-Reaction"},PerformerType=typeof(CommunicateActionPerformer)}, 
                Reaction = new TwoCharactersActionTemplate { Name=new BaseCommon.Verb{UniqueId="Interaction1-ReReaction2",Past="Interaction1-ReReaction2",Present="Interaction1-ReReaction2"},PerformerType=typeof(CommunicateActionPerformer)} 
            }
            };
        }

    }
}