using System;
using System.Collections.Generic;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Actions.BaseActions
{
    public class GameAction
    {
        public Verb Name { get; protected set; }

        public ActionType ActionType { get; protected set; }

        public bool StartingAction { get; protected set; }

        public bool FinishTheInteractionOnExecution { get; set; }

        public GameAction()
        {
            ActionType = ActionType.Default;
        }

        public NonCharacterObject TargetObject { get; set; }

        public NonCharacterObject ActiveObject { get; set; }

        [JsonIgnore]
        public ICharacter TargetCharacter { get; set; }

        [JsonIgnore]
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

        protected virtual List<GameAction> GetAvailableActions()
        {
            return new List<GameAction>();
        }

    }
}
