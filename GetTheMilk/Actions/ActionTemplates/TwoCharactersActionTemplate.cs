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

        [LevelBuilderAccesibleProperty(typeof(string))]
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
                                                        FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,
                                                        Message = Message,
                                                     CurrentPerformer = CurrentPerformer,
                                                     ActiveCharacter = ActiveCharacter
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

        protected override object[] Translate()
        {
            
            var result= base.Translate();
            result[0] = (!string.IsNullOrEmpty(Message)) ? (result[0] + " " + Message + " to") : result[0];
            result[2] = (TargetCharacter == null) ? "Target Character Not Assigned" : TargetCharacter.Name.Narrator;

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
