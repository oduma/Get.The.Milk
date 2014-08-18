using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Factories;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class TwoCharactersActionTemplate: BaseActionTemplate
    {
        public TwoCharactersActionTemplate()
        {
            Category = GetType().Name;
        }

        public string Message { get; set; }

        private Type _performerType;
        public override Type PerformerType
        {
            get
            {
                return _performerType;
            }
            set
            {
                _performerType = value;
                if(_performerType!=null)
                    CurrentPerformer = TemplatedActionPerformersFactory.GetFactory().CreateActionPerformer<ITwoCharactersActionTemplatePerformer>(value.Name);
            }
        }

        ITwoCharactersActionTemplatePerformer _currentPerformer;
        public override IActionTemplatePerformer CurrentPerformer
        {
            get
            {
                return _currentPerformer;
            }
            protected set
            {
                _currentPerformer = (ITwoCharactersActionTemplatePerformer)value;
                BuildPerformer(ref _currentPerformer);

            }
        }
        public override BaseActionTemplate Clone()
        {
            return new TwoCharactersActionTemplate { Name = Name, StartingAction = StartingAction,
                                                        Message = Message,
                                                     CurrentPerformer = CurrentPerformer,
                                                     ActiveCharacter = ActiveCharacter,
                                                     TargetCharacter=TargetCharacter
            };
        }

        public override bool CanPerform()
        {
            return ((ITwoCharactersActionTemplatePerformer)CurrentPerformer).CanPerform(this);
        }

        public override PerformActionResult Perform()
        {
            return ((ITwoCharactersActionTemplatePerformer)CurrentPerformer).Perform(this);
        }

        internal override object[] Translate()
        {
            
            var result= base.Translate();
            result[3] = (TargetCharacter == null || TargetCharacter.Name == null) ? "Target Character Not Assigned" : TargetCharacter.Name.Narrator;
            result[5] = (!string.IsNullOrEmpty(Message)) ? (Message + " to ") : string.Empty;
            if (PerformerType == typeof(AttackActionPerformer))
                result[8] = (ActiveCharacter == null || ActiveCharacter.ActiveAttackWeapon == null || ActiveCharacter.ActiveAttackWeapon.Name == null) ? string.Empty : " using " + ActiveCharacter.ActiveAttackWeapon.Name.Narrator;

            return result;
        }

        public override bool Equals(object obj)
        {
            var result= base.Equals(obj);
            if (!result)
                return false;
            var y = (TwoCharactersActionTemplate)obj;
            if (y.CurrentPerformer.GetType() == typeof(CommunicateActionPerformer) && CurrentPerformer.GetType() == typeof(CommunicateActionPerformer))
                return y.Message == Message;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
