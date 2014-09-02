namespace GetTheMilk.Actions.ActionTemplates
{
    public class OneObjectActionTemplate:BaseActionTemplate
    {
        #region Constructors
        public OneObjectActionTemplate()
        {
            StartingAction = true;
            Category = GetType().Name;

        }
        #endregion

        public override BaseActionTemplate Clone()
        {
            return new OneObjectActionTemplate
            {
                Name = Name,
                StartingAction = StartingAction,
                CurrentPerformer = CurrentPerformer,
                ActiveCharacter = ActiveCharacter,
                ActiveObject = ActiveObject,
                TargetObject=TargetObject
            };
        }
        internal override object[] Translate()
        {
            var result = base.Translate();
            result[2] = (TargetObject == null || TargetObject.Name == null) ? ((TargetCharacter==null)?"No Target Object":string.Empty) : TargetObject.Name.Narrator;
            return result;
        }
    }
}
