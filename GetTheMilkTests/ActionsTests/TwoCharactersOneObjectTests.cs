using System.Linq;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class TwoCharactersOneObjectTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases2C1O")]
        public Inventory TwoCharactersOneObjectAction(ObjectTransferToAction action)
        {
            var result = action.Perform();
            if (result.ResultType==ActionResultType.NotOk)
                return action.TargetObject.StorageContainer;

            if(action.TargetCharacter.Name.Main=="Player")
            {
                Assert.Contains(action.TargetObject,action.TargetCharacter.Inventory);
                Assert.True(action.ActiveCharacter.Inventory.All(o => o.Name.Main != action.TargetObject.Name.Main));
                return action.TargetObject.StorageContainer;
            }
            return new Inventory();
        }

        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases2CC")]
        public int TwoCharactersCurrencyAction1(Character active, Character passive,int amount,TransactionType transactionType)
        {

            if(!active.Walet.CanPerformTransaction(new TransactionDetails{TransactionType=transactionType,Price=amount,Towards=passive}))
                return active.Walet.CurrentCapacity;
            active.Walet.PerformTransaction(new TransactionDetails
                                                {TransactionType = transactionType, Price = amount, Towards = passive});
            Assert.AreEqual(50,passive.Walet.CurrentCapacity);

            return active.Walet.CurrentCapacity;
        }

        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCases2C1OC")]
        public int TwoCharactersCurrencyAction(ObjectTransferFromAction action)
        {
            var result = action.Perform();
            if (result.ResultType==ActionResultType.NotOk)
            {
                Assert.AreEqual(action.TargetCharacter.Name.Main,action.TargetObject.StorageContainer.Owner.Name.Main);
                return action.ActiveCharacter.Walet.CurrentCapacity;
            }
            if(action.ActiveCharacter.Inventory.MaximumCapacity==0)
                Assert.AreEqual(action.TargetCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
            else if (action.TargetCharacter.Name.Main == "John the Shop Keeper")
            {
                Assert.AreEqual(action.ActiveCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
            }
            else
            {
                Assert.AreEqual(action.TargetCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
            }
            return action.ActiveCharacter.Walet.CurrentCapacity;
        }

        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCases2C1OC1")]
        public int TwoCharactersCurrencyActionSell(ObjectTransferToAction action)
        {
            var result = action.Perform();
            if (result.ResultType == ActionResultType.NotOk)
            {
                Assert.AreEqual(action.ActiveCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
                return action.ActiveCharacter.Walet.CurrentCapacity;
            }
            if (action.ActiveCharacter.Inventory.MaximumCapacity == 0)
                Assert.AreEqual(action.TargetCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
            else if (action.TargetCharacter.Name.Main == "John the Shop Keeper")
            {
                Assert.AreEqual(action.ActiveCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
            }
            else
            {
                Assert.AreEqual(action.TargetCharacter.Name.Main, action.TargetObject.StorageContainer.Owner.Name.Main);
            }
            return action.ActiveCharacter.Walet.CurrentCapacity;
        }

    }
}
