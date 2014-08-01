using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Objects.BaseObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests.SingleTemplatesTests
{
    [TestFixture]
    public class ActionTemplatesToStringTests
    {
        #region NoObjectActionTemplates
        [Test]
        public void EmptyNoObjectActionTemplateToString()
        {
            var defaultActionTemplate = new NoObjectActionTemplate();

            Assert.AreEqual("NoObjectActionTemplate", defaultActionTemplate.ToString());
        }

        [Test]
        public void IncompleteEmptyNoObjectActionTemplateToString()
        {
            var defaultActionTemplate = new NoObjectActionTemplate { Name = new Verb { UniqueId = "Laugh" } };

            Assert.AreEqual("Laugh", defaultActionTemplate.ToString());
        }

        [Test]
        public void CompleteEmptyNoObjectActionTemplateToString()
        {
            var defaultActionTemplate = new NoObjectActionTemplate { Name = new Verb { UniqueId = "Laugh" ,Past="laughed", Present="laugh"} };

            Assert.AreEqual("laugh", defaultActionTemplate.ToString());
        }

        #endregion

        #region OneObjectActionTemplates
        [Test]
        public void EmptyOneObjectActionTemplateToString()
        {
            var defaultActionTemplate = new OneObjectActionTemplate();

            Assert.AreEqual("OneObjectActionTemplate No Target Object", defaultActionTemplate.ToString());
        }

        [Test]
        public void IncompleteEmptyOneObjectActionTemplateToString()
        {
            var defaultActionTemplate = new OneObjectActionTemplate { Name = new Verb { UniqueId = "Kick" } };

            Assert.AreEqual("Kick No Target Object", defaultActionTemplate.ToString());
        }

        [Test]
        public void NotEmptyOneActionTemplateWithTargetObjectToString()
        {
            var defaultActionTemplate = new OneObjectActionTemplate
            {
                ActiveCharacter = new Player(),
                TargetObject =
                    new Weapon
                    {
                        Name = new Noun { Main = "Super Weapon", Narrator = "super weapon" }
                    },
                PerformerType = typeof(OneObjectActionTemplatePerformer),
                Name = new Verb
                {
                    UniqueId = "SelectAttackWeapon",
                    Past = "selected attack weapon",
                    Present = "select attack weapon"
                }
            };

            Assert.AreEqual("select attack weapon super weapon", defaultActionTemplate.ToString());

        }

        #endregion

        #region ObjectUseOnObjectActionTemplates
        [Test]
        public void EmptyDefaultObjectUseOnObjectActionTemplateToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate();

            Assert.AreEqual("ObjectUseOnObjectActionTemplate No Target Object Assigned using No Active Object Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void IncompleteNotEmptyObjectUseOnObjectActionTemplateToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType = typeof(ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Open", Past = "opened", Present = "open" },
                DestroyActiveObject = true,
                DestroyTargetObject = true
            };

            Assert.AreEqual("open No Target Object Assigned using No Active Object Assigned", defaultActionTemplate.ToString());

        }

        [Test]
        public void CompleteNotEmptyObjectUseOnObjectActionTemplateToString()
        {
            var defaultActionTemplate = new ObjectUseOnObjectActionTemplate
            {
                PerformerType = typeof(ObjectUseOnObjectActionTemplatePerformer),
                Name = new Verb { UniqueId = "Open", Past = "opened", Present = "open" },
                DestroyActiveObject = true,
                DestroyTargetObject = true,
                ActiveObject = new Tool { Name = new Noun { Main="Key",Narrator="the key"} },
                TargetObject = new NonCharacterObject { Name = new Noun { Main="Door",Narrator="the door"} }
            };

            Assert.AreEqual("open the door using the key", defaultActionTemplate.ToString());

        }

        #endregion

        #region ObjectTransferActionTemplates
        [Test]
        public void EmptyDefaultObjectTransferActionTemplateToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate();

            Assert.AreEqual("ObjectTransferActionTemplate Target Object Not Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void IncompleteNotEmptyObjectTransferActionTemplateToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                PerformerType = typeof(BuyActionPerformer),
                Name = new Verb { UniqueId = "Buy", Past = "bought", Present = "buy" }
            };

            Assert.AreEqual("buy Target Object Not Assigned", defaultActionTemplate.ToString());

        }

        [Test]
        public void CompleteNotEmptyObjectTransferActionTemplateWithouPriceToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                PerformerType = typeof(TakeFromActionPerformer),
                Name = new Verb { UniqueId = "Takefrom", Past = "took", Present = "take" },
                TargetObject = new Tool { Name = new Noun { Main="Kazoo", Narrator="kazoo"} }
            };

            Assert.AreEqual("take kazoo", defaultActionTemplate.ToString());

        }

        [Test]
        public void CompleteNotEmptyObjectTransferActionTemplateWithBuyPriceToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                PerformerType = typeof(BuyActionPerformer),
                Name = new Verb { UniqueId = "Buy", Past = "bought", Present = "buy" },
                TargetObject = new Tool { Name = new Noun { Main="Freedom", Narrator="freedom"}, BuyPrice=100000 }
            };

            Assert.AreEqual("buy freedom (100000)", defaultActionTemplate.ToString());

        }

        [Test]
        public void CompleteNotEmptyObjectTransferActionTemplateWithSellPriceToString()
        {
            var defaultActionTemplate = new ObjectTransferActionTemplate
            {
                PerformerType = typeof(SellActionPerformer),
                Name = new Verb { UniqueId = "Sell", Past = "sold", Present = "sell" },
                TargetObject = new Tool { Name = new Noun { Main = "Soul", Narrator = "soul" }, SellPrice = 100000 }
            };

            Assert.AreEqual("sell soul (100000)", defaultActionTemplate.ToString());

        }
        #endregion

        #region ExposeInventoryActionTemplates

        [Test]
        public void EmptyDefaultExposeInventoryActionTemplateToString()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate();

            Assert.AreEqual("expose Inventory", defaultActionTemplate.ToString());
        }

        [Test]
        public void CompleteNotEmptyExposeInventoryActionTemplateNoAttackToString()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                ActiveCharacter = new Player(),
                SelfInventory = true
            };

            Assert.AreEqual("expose Inventory", defaultActionTemplate.ToString());

        }
        [Test]
        public void CompleteNotEmptyExposeInventoryActionTemplateAttackToString()
        {
            var defaultActionTemplate = new ExposeInventoryActionTemplate
            {
                ActiveCharacter = new Player(),
                SelfInventory = true,
                FinishActionUniqueId="Attack"
            };

            Assert.AreEqual("prepare for Battle", defaultActionTemplate.ToString());

        }
        #endregion

        #region TwoCharactersActionTemplates
        [Test]
        public void EmptyTwoCharactersActionTemplateToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate();

            Assert.AreEqual("TwoCharactersActionTemplate Target Character Not Assigned", defaultActionTemplate.ToString());
        }

        [Test]
        public void IncompleteNotEmptyTwoCharactersActionTemplateToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(AttackActionPerformer),
                Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" }
            };

            Assert.AreEqual("attack Target Character Not Assigned", defaultActionTemplate.ToString());


        }

        [Test]
        public void IncompleteNotEmptyTwoCharactersActionTemplateWithMessageToString()
        {

            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message = "hello"
            };

            Assert.AreEqual("say hello to Target Character Not Assigned", defaultActionTemplate.ToString());


        }

        [Test]
        public void IncompleteNotEmptyTwoCharactersActionTemplateToString2()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(AttackActionPerformer),
                Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" },
                TargetCharacter = new Characters.BaseCharacters.Character { Name = new Noun { Main="Joe",Narrator="Joe"} }
            };

            Assert.AreEqual("attack Joe", defaultActionTemplate.ToString());


        }

        [Test]
        public void CompleteNotEmptyTwoCharactersActionTemplateWithMessageToString()
        {

            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(CommunicateActionPerformer),
                Name = new Verb { UniqueId = "Communicate", Past = "said", Present = "say" },
                Message = "hello",
                TargetCharacter = new Characters.BaseCharacters.Character { Name = new Noun { Main = "Joe", Narrator = "Joe" } }
            };

            Assert.AreEqual("say hello to Joe", defaultActionTemplate.ToString());


        }

        [Test]
        public void CompleteNotEmptyTwoCharactersActionTemplateToString()
        {
            var defaultActionTemplate = new TwoCharactersActionTemplate
            {
                PerformerType = typeof(AttackActionPerformer),
                Name = new Verb { UniqueId = "Attack", Past = "attacked", Present = "attack" },
                TargetCharacter = new Characters.BaseCharacters.Character { Name = new Noun { Main = "Joe", Narrator = "Joe" } },
                ActiveCharacter = new Player { ActiveAttackWeapon = new Weapon { Name = new Noun { Main="FinnsSword",Narrator="Finn's Sword"} } }
            };

            Assert.AreEqual("attack Joe using Finn's Sword", defaultActionTemplate.ToString());


        }
        #endregion

        #region MovementActionTemplates

        [Test]
        public void EmptyMovementActionTemplateToString()
        {
            var defaultActionTemplate = new MovementActionTemplate();

            Assert.AreEqual("walk", defaultActionTemplate.ToString());
        }

        [Test]
        public void CompleteNotEmptyMovementActionTemplateToString()
        {
            var defaultActionTemplate = new MovementActionTemplate
            {
                PerformerType = typeof(RunActionPerformer),
                Name = new Verb { UniqueId = "Run", Past = "ran", Present = "run" },
                DefaultDistance = 3
            };

            Assert.AreEqual("run", defaultActionTemplate.ToString());

        }
        #endregion

    }
}
