using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;

namespace GetTheMilk.Actions.ActionTemplates
{
    public abstract class BaseActionTemplate
    {
        #region Naming and Categorisation

        public string Category { get; set; }

        public Type PerformerType { get; set; }

        public Verb Name { get; set; }

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

        public Character ActiveCharacter { get; set; }
        #endregion

        #region Additional methods

        protected virtual List<BaseActionTemplate> GetAvailableActions()
        {
            return new List<BaseActionTemplate>();
        }

        public abstract BaseActionTemplate Clone();

        #endregion

        protected virtual object[] Translate()
        {
            return new object[]
                       {
                           (Name.Present)??(GetType().Name + "-" + Name.UniqueId),
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

                var message = gameSettings.ActionTypeMessages.FirstOrDefault(m => m.Id == Category);
                return message == null ? gameSettings.TranslatorErrorMessage : string.Format(message.Value, Translate()).Trim();
            }
            catch
            {
                return "Translation for the action: " + Name.UniqueId + " could not be found.";
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseActionTemplate))
                return false;
            var y = (BaseActionTemplate) obj;

            if (string.IsNullOrEmpty(Category) || Name == null || string.IsNullOrEmpty(Name.UniqueId))
                return false;
            if (string.IsNullOrEmpty(y.Category) || y.Name == null || string.IsNullOrEmpty(y.Name.UniqueId))
                return false;
            return (Name.UniqueId == y.Name.UniqueId);
        }

        public override int GetHashCode()
        {
            return (Name.UniqueId).GetHashCode();
        }
    }
}
