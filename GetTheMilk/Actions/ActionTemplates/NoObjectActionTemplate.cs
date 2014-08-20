using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using System;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class NoObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public NoObjectActionTemplate()
        {
            StartingAction = false;
            Category = GetType().Name;

        }
        #endregion

        public override BaseActionTemplate Clone()
        {
            return new NoObjectActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                CurrentPerformer = CurrentPerformer,
                ActiveCharacter = ActiveCharacter
            };
        }
    }
}
