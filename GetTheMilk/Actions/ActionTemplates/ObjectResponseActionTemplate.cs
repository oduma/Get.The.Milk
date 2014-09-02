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
