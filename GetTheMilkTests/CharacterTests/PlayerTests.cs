using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using NUnit.Framework;

namespace GetTheMilkTests.CharacterTests
{
    [TestFixture]
    public class PlayerTests
    {
        [Test]
        public void PlayerCreation()
        {
            var player1 = Player.GetNewInstance();
            player1.SetPlayerName("Me");
            player1.Experience = 200;
            player1.Walet.CurrentCapacity = 250;

            var player2 = Player.GetNewInstance();

            Assert.AreEqual(player1.Name, player2.Name);

            Assert.AreEqual(player1.Experience, player2.Experience);

            Assert.AreEqual(player1.Walet.CurrentCapacity, player2.Walet.CurrentCapacity);
        }

    }
}
