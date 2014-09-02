using System;
using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
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

        #endregion

        #region Elements taking part
        public virtual NonCharacterObject TargetObject { get; set; }

        [JsonIgnore]
        public virtual NonCharacterObject ActiveObject { get; set; }
        
        public virtual Character TargetCharacter { get; set; }

        [JsonIgnore]
        public virtual Character ActiveCharacter { get; set; }
        #endregion

        #region Additional methods

        public abstract BaseActionTemplate Clone();

        public virtual bool CanPerform()
        {
            return (CurrentPerformer).CanPerform(this);
        }

        public virtual PerformActionResult Perform()
        {
            return (CurrentPerformer).Perform(this);
        }

        #endregion

        internal virtual object[] Translate()
        {
            return new object[]
                       {
                           (Name!=null)?((Name.Past)??(Name.UniqueId)):Category,
                           (Name!=null)?((Name.Present)??(Name.UniqueId)):Category,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           (ActiveCharacter==null || ActiveCharacter.Name==null)?"No Active Character Assigned":ActiveCharacter.Name.Narrator,
                           null,
                           null,
                           null
                       };
        }

        public override string ToString()
        {
            try
            {
                var gameSettings = GameSettings.GetInstance();

                var message = gameSettings.ActionTemplateMessages.FirstOrDefault(m => m.Id == GetType().Name);
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
            if (Name == null || string.IsNullOrEmpty(Name.UniqueId))
                return -1;
            return (Name.UniqueId).GetHashCode();
        }

        protected void BuildPerformer<T>(ref T currentPerformer) where T:IActionTemplatePerformer
        {
            if (currentPerformer != null)
                if (PerformerType == null || PerformerType.Name != currentPerformer.GetType().Name)
                    PerformerType = currentPerformer.GetType();
        }

        public bool TryConvertTo<T>(out T result) where T:BaseActionTemplate
        {
            if(Category!=typeof(T).Name)
            {
                result = null;
                return false;
            }
            result = this as T;
            return true;
        }

        private Type _performerType;
        public virtual Type PerformerType
        {
            get
            {
                return _performerType;
            }
            set
            {
                _performerType = value;
                if (_performerType != null)
                    CurrentPerformer = TemplatedActionPerformersFactory.CreateActionPerformer(value.Name);
            }
        }

        IActionTemplatePerformer _currentPerformer;

        [JsonIgnore]
        public virtual IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            set
            {
                _currentPerformer = value;
                BuildPerformer(ref _currentPerformer);

            }
        }

    }
}
