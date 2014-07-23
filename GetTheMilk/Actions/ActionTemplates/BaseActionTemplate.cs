using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using Newtonsoft.Json;
using GetTheMilk.Factories;

namespace GetTheMilk.Actions.ActionTemplates
{
    public abstract class BaseActionTemplate
    {
        #region Naming and Categorisation

        public Verb Name { get; set; }

        public string Category { get; protected set; }

        #endregion

        #region Order of execution
        public bool StartingAction { get; set; }


        [LevelBuilderAccesibleProperty(typeof(bool))]
        public bool FinishTheInteractionOnExecution { get; set; }

        #endregion

        #region Elements taking part
        public NonCharacterObject TargetObject { get; set; }

        public NonCharacterObject ActiveObject { get; set; }

        public Character TargetCharacter { get; set; }

        [JsonIgnore]
        public Character ActiveCharacter { get; set; }
        #endregion

        #region Additional methods

        public abstract BaseActionTemplate Clone();

        public abstract bool CanPerform();

        public abstract PerformActionResult Perform();

        #endregion

        [JsonIgnore]
        public virtual IActionTemplatePerformer CurrentPerformer { get; set; }

        public virtual Type PerformerType { get; set; }

        protected virtual object[] Translate()
        {
            return new object[]
                       {
                           (Name!=null)?((Name.Present)??(GetType().Name + "-" + Name.UniqueId)):GetType().Name,
                           (TargetObject == null) ? string.Empty : TargetObject.Name.Narrator,
                           (TargetCharacter == null) ? string.Empty : TargetCharacter.Name.Narrator,
                           (ActiveObject == null) ? string.Empty : ActiveObject.Name.Narrator,
                           null,null,null,null,null,null
                       };
        }

        public override string ToString()
        {
            try
            {
                var gameSettings = GameSettings.GetInstance();

                var message = gameSettings.ActionTypeMessages.FirstOrDefault(m => m.Id == GetType().Name);
                return message == null ? gameSettings.TranslatorErrorMessage : string.Format(message.Value, Translate()).Trim();
            }
            catch
            {
                return "Translation for the an action of type: " + GetType().Name + " could not be found.";
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseActionTemplate))
                return false;
            var y = (BaseActionTemplate) obj;

            if (Name == null || string.IsNullOrEmpty(Name.UniqueId))
                return false;
            if (y.Name == null || string.IsNullOrEmpty(y.Name.UniqueId))
                return false;
            return (Name.UniqueId == y.Name.UniqueId);
        }

        public override int GetHashCode()
        {
            return (Name.UniqueId).GetHashCode();
        }
    }
}
