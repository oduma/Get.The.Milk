using System.Collections.Generic;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class BaseActionTemplatesEqualityComparer:EqualityComparer<BaseActionTemplate>
    {
        public override bool Equals(BaseActionTemplate x, BaseActionTemplate y)
        {
            if ( x.Name == null || string.IsNullOrEmpty(x.Name.UniqueId))
                return false;
            if (y.Name == null || string.IsNullOrEmpty(y.Name.UniqueId))
                return false;
            return (x.Name.UniqueId == y.Name.UniqueId);
        }

        public override int GetHashCode(BaseActionTemplate obj)
        {
            return (obj.Name.UniqueId).GetHashCode();
        }
    }
}
