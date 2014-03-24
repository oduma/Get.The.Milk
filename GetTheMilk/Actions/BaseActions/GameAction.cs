using System;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class GameAction
    {
        public Verb Name { get; protected set; }

        public ActionType ActionType { get; protected set; }

        public GameAction()
        {
            ActionType = ActionType.Default;
        }

        public NonCharacterObject TargetObject { get; set; }

        public NonCharacterObject ActiveObject { get; set; }

        public ICharacter TargetCharacter { get; set; }

        public ICharacter ActiveCharacter { get; set; }

        public virtual ActionResult Perform()
        {
            throw new NotImplementedException();
        }

        public virtual bool CanPerform()
        {
            return true;
        }

        public virtual GameAction CreateNewInstance()
        {
            return new GameAction();
        }
    }
}
