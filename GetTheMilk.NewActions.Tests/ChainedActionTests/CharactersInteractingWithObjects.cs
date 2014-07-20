using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.ChainedActionTests
{
    [TestFixture]
    public class CharactersInteractingWithObjects
    {
        private Character _character = new Character { ObjectTypeId = "NPCFriendly" };

        private NonCharacterObject _interactionObject = TestHelper.InteractionObject;
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
        public void ObjectCannotPerformReaction()
        {
            _character.LoadInteractions(_interactionObject,_interactionObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Default");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectUseOnObjectActionTemplate), useAction.GetType());
            useAction.ActiveCharacter = _character;
            useAction.ActiveObject = _character.Inventory[0];
            useAction.TargetObject = _interactionObject;
            Assert.AreEqual(ActionResultType.NotOk, _character.PerformAction(useAction).ResultType);
            Assert.AreEqual(1, _character.Inventory.Count);
        }
        [Test]
        public void CharacterInitiateObjectUseOnObjectObjectReactsWithOneObject()
        {
            _character.LoadInteractions(_interactionObject, _interactionObject.Name.Main);
            var useAction = _character.CreateNewInstanceOfAction<ObjectUseOnObjectActionTemplate>("Ping");
            Assert.IsNotNull(useAction);
            useAction.ActiveCharacter = _character;
            useAction.ActiveObject = _character.Inventory[0];
            useAction.TargetObject = _interactionObject;
            Assert.AreEqual(ActionResultType.NotOk, _character.PerformAction(useAction).ResultType);
            Assert.AreEqual(1, _character.Inventory.Count);

        }

        [Test]
        public void CharacterInitiateOneObjectObjectReactsWithOneObject()
        {

        }

        [Test]
        public void CharacterInitiateObjectUseOnObjectObjectReactsWithOneObjectDoubleChain()
        {

        }

        [Test]
        public void CharacterInitiateOnOebjectObjectReactsWithOneObjectDoubleChain()
        {

        }

    }
}
