using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Navigation;
using NUnit.Framework;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class CharactersAbleToPerformMovementActionsTests
    {
        private Player _player = new Player();
        private Character _character=new Character{ObjectTypeId="NPCFriendly"};
        [SetUp]
        public void SetUp()
        {
            var factory = ObjectActionsFactory.GetFactory();

            var objAction = factory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;

            objAction = factory.CreateObjectAction("NPCFriendly");
            _character.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _character.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
        }
        [Test]
        public void PlayerNotAbleToEnterTheLevelNoActiveCharacter()
        {
 
            var enterlevelAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("EnterLevel");
            Assert.IsNotNull(enterlevelAction);
            Assert.AreEqual(typeof(MovementActionTemplate),enterlevelAction.GetType());
            Assert.False(_player.CanPerformAction(enterlevelAction));
        }
        [Test]
        public void PlayerNotAbleToEnterTheLevelNoMap()
        {
            var enterlevelAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("EnterLevel");
            enterlevelAction.ActiveCharacter = _player;
            Assert.IsNotNull(enterlevelAction);
            Assert.AreEqual(typeof(MovementActionTemplate), enterlevelAction.GetType());
            Assert.False(_player.CanPerformAction(enterlevelAction));
        }
        [Test]
        public void PlayerNotAbleToEnterTheLevelContradictoryInfo()
        {
            var enterlevelAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("EnterLevel");
            enterlevelAction.ActiveCharacter = _player;
            enterlevelAction.CurrentMap = new Map {Cells = new Cell[] {new Cell()}};
            enterlevelAction.Direction = Direction.Top;
            Assert.IsNotNull(enterlevelAction);
            Assert.AreEqual(typeof(MovementActionTemplate), enterlevelAction.GetType());
            Assert.False(_player.CanPerformAction(enterlevelAction));
        }
        [Test]
        public void PlayerAbleToEnterTheLevel()
        {
            var enterlevelAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("EnterLevel");
            enterlevelAction.ActiveCharacter = _player;
            enterlevelAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            Assert.IsNotNull(enterlevelAction);
            Assert.AreEqual(typeof(MovementActionTemplate), enterlevelAction.GetType());
            Assert.True(_player.CanPerformAction(enterlevelAction));
        }
        [Test]
        public void CharacterNotAbleToEnterTheLevel()
        {
            var enterlevelActionTemplate = _character.CreateNewInstanceOfAction<MovementActionTemplate>("EnterLevel");
            Assert.IsNull(enterlevelActionTemplate);
        }
        [Test]
        public void PlayerAbleToWalk()
        {
            var movementAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            movementAction.ActiveCharacter = _player;
            movementAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            movementAction.Direction = Direction.Top;
            Assert.IsNotNull(movementAction);
            Assert.AreEqual(typeof(MovementActionTemplate), movementAction.GetType());
            Assert.True(_player.CanPerformAction(movementAction));
        }
        [Test]
        public void PlayerAbleToRun()
        {
            var movementAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("Run");
            movementAction.ActiveCharacter = _player;
            movementAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            movementAction.Direction = Direction.Top;
            Assert.IsNotNull(movementAction);
            Assert.AreEqual(typeof(MovementActionTemplate), movementAction.GetType());
            Assert.True(_player.CanPerformAction(movementAction));
        }
        [Test]
        public void PlayerAbleToTeleport()
        {
            var enterlevelAction = _player.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            enterlevelAction.ActiveCharacter = _player;
            enterlevelAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            Assert.IsNotNull(enterlevelAction);
            Assert.AreEqual(typeof(MovementActionTemplate), enterlevelAction.GetType());
            Assert.True(_player.CanPerformAction(enterlevelAction));

        }
        [Test]
        public void AnyCharacterAbleToWalk()
        {
            var movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Walk");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            movementAction.Direction = Direction.Top;
            Assert.IsNotNull(movementAction);
            Assert.AreEqual(typeof(MovementActionTemplate), movementAction.GetType());
            Assert.True(_character.CanPerformAction(movementAction));

        }
        [Test]
        public void AnyCharacterAbleToRun()
        {
            var movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Run");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            movementAction.Direction = Direction.Top;
            Assert.IsNotNull(movementAction);
            Assert.AreEqual(typeof(MovementActionTemplate), movementAction.GetType());
            Assert.True(_character.CanPerformAction(movementAction));

        }
        [Test]
        public void AnyCharacterAbleToTeleport()
        {
            var movementAction = _character.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            movementAction.ActiveCharacter = _character;
            movementAction.CurrentMap = new Map { Cells = new Cell[] { new Cell() } };
            movementAction.Direction = Direction.None;
            Assert.IsNotNull(movementAction);
            Assert.AreEqual(typeof(MovementActionTemplate), movementAction.GetType());
            Assert.True(_character.CanPerformAction(movementAction));


        }
    }
}
