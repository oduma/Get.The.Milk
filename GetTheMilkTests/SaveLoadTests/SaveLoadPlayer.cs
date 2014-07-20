using GetTheMilk.Characters;
using GetTheMilk.Utils;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GetTheMilkTests.SaveLoadTests
{
    [TestFixture]
    public class SaveLoadPlayer
    {
        [Test]
        public void SaveAPlayer()
        {
            Player player = new Player();
            var result = player.Save();

            var resultString = JsonConvert.SerializeObject(result);

            var actual = Player.Load<Player>(result);

            Assert.IsNotNull(actual);

            Assert.AreEqual(player.BlockMovement,actual.BlockMovement);
            Assert.AreEqual(player.CellNumber, actual.CellNumber);
            Assert.AreEqual(player.Experience, actual.Experience);
            Assert.AreEqual(player.Health, actual.Health);
            Assert.AreEqual(player.Interactions.Count, actual.Interactions.Count);
            Assert.AreEqual(player.Interactions[GenericInteractionRulesKeys.All].Length, actual.Interactions[GenericInteractionRulesKeys.All].Length);
            Assert.AreEqual(player.Name.Main, actual.Name.Main);
            Assert.AreEqual(player.Name.Narrator, actual.Name.Narrator);
            Assert.AreEqual(player.Range, actual.Range);
            Assert.AreEqual(player.Walet.MaxCapacity, actual.Walet.MaxCapacity);
            Assert.AreEqual(player.Walet.CurrentCapacity, actual.Walet.CurrentCapacity);
        }
    }
}
