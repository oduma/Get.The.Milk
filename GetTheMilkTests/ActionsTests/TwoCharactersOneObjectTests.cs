using System.Linq;
using GetTheMilk;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using NUnit.Framework;

namespace GetTheMilkTests.ActionsTests
{
    [TestFixture]
    public class TwoCharactersOneObjectTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases2C1O")]
        public Inventory TwoCharactersOneObjectAction(Character active, Character passive, ObjectTransferAction action)
        {
            var result = active.TryPerformAction(action, passive);
            if (result.ResultType==ActionResultType.NotOk)
                return action.UseableObject.StorageContainer;

            if(passive.Name=="Keyless Child")
            {
                Assert.Contains(action.UseableObject,passive.RightHandObject.Objects);
                Assert.True(!active.RightHandObject.Objects.Any());
                return action.UseableObject.StorageContainer;
            }
            return null;
        }

        [Test,TestCaseSource(typeof(DataGeneratorForActions),"TestCases2CC")]
        public int TwoCharactersCurrencyAction1(Character active, Character passive,int amount,TransactionType transactionType)
        {

            if(!active.Walet.CanPerformTransaction(new TransactionDetails{TransactionType=transactionType,Price=amount,Towards=passive}))
                return active.Walet.CurrentCapacity;
            active.Walet.PerformTransaction(new TransactionDetails
                                                {TransactionType = transactionType, Price = amount, Towards = passive});
            Assert.AreEqual(60,passive.Walet.CurrentCapacity);

            return active.Walet.CurrentCapacity;
        }

        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCases2C1OC")]
        public int TwoCharactersCurrencyAction(Character active, Character passive, ObjectTransferAction action)
        {
            var result = active.TryPerformAction(action, passive);
            if (result.ResultType==ActionResultType.NotOk)
            {
                if(action.Name=="Buy")
                    Assert.AreEqual(passive.Name,action.UseableObject.StorageContainer.Owner.Name);
                if(action.Name=="Sell")
                    Assert.AreEqual(active.Name, action.UseableObject.StorageContainer.Owner.Name);

                return active.Walet.CurrentCapacity;
            }
            if(active.ToolInventory.MaximumCapacity==0)
                Assert.AreEqual(passive.Name, action.UseableObject.StorageContainer.Owner.Name);
            else if(passive.Name=="Shop Keeper")
            {
                Assert.AreEqual(active.Name, action.UseableObject.StorageContainer.Owner.Name);
            }
            else
            {
                Assert.AreEqual(passive.Name, action.UseableObject.StorageContainer.Owner.Name);
            }
            return active.Walet.CurrentCapacity;
        }

    }
}
