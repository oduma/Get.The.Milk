using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class ObjectResponseActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public ObjectResponseActionTemplate()
        {
            StartingAction = false;
            Category = GetType().Name;
        }
        #endregion

        public override BaseActionTemplate Clone()
        {
            return new ObjectResponseActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                CurrentPerformer = CurrentPerformer,
                ActiveObject=ActiveObject
            };
        }
        internal override object[] Translate()
        {
            var result = base.Translate();
            result[4] = (ActiveObject == null || ActiveObject.Name == null) ? "No Active Object Assigned" : ActiveObject.Name.Narrator;
            return result;
        }


    }
}
