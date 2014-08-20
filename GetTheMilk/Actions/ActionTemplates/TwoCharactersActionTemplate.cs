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

        public override BaseActionTemplate Clone()
        {
            return new TwoCharactersActionTemplate { Name = Name, StartingAction = StartingAction,
                                                        Message = Message,
                                                     CurrentPerformer = CurrentPerformer,
                                                     ActiveCharacter = ActiveCharacter,
                                                     TargetCharacter=TargetCharacter
            };
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
