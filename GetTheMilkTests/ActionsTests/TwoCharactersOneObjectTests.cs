using System.Linq;
using GetTheMilk;
using GetTheMilk.Accounts;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
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

            if(passive.Name.Main=="Player")
            {
                Assert.Contains(action.UseableObject,passive.Inventory.Objects);
                Assert.True(active.Inventory.Objects.All(o => o.Name.Main != action.UseableObject.Name.Main));
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
            Assert.AreEqual(210,passive.Walet.CurrentCapacity);

            return active.Walet.CurrentCapacity;
        }

        [Test, TestCaseSource(typeof(DataGeneratorForActions), "TestCases2C1OC")]
        public int TwoCharactersCurrencyAction(Character active, Character passive, ObjectTransferAction action)
        {
            var result = active.TryPerformAction(action, passive);
            if (result.ResultType==ActionResultType.NotOk)
            {
                if(action.Name.Infinitive=="To Buy")
                    Assert.AreEqual(passive.Name.Main,action.UseableObject.StorageContainer.Owner.Name.Main);
                if(action.Name.Infinitive=="To Sell")
                    Assert.AreEqual(active.Name.Main, action.UseableObject.StorageContainer.Owner.Name.Main);

                return active.Walet.CurrentCapacity;
            }
            if(active.Inventory.MaximumCapacity==0)
                Assert.AreEqual(passive.Name.Main, action.UseableObject.StorageContainer.Owner.Name.Main);
            else if (passive.Name.Main == "John the Shop Keeper")
            {
                Assert.AreEqual(active.Name.Main, action.UseableObject.StorageContainer.Owner.Name.Main);
            }
            else
            {
                Assert.AreEqual(passive.Name.Main, action.UseableObject.StorageContainer.Owner.Name.Main);
            }
            return active.Walet.CurrentCapacity;
        }

    }
}
