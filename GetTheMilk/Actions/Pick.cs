using System.Linq;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Pick : OneObjectAction
    {
        public override string Name
        {
            get { return "Pick"; }
        }

        public override void Perform(ICharacter c, IPositionableObject o)
        {
            if (!c.RightHandObject.Objects.Any())
            {
                c.RightHandObject.Add(o);
            }
            else if (!c.LeftHandObject.Objects.Any())
            {
                c.LeftHandObject.Add(o);
            }
        }
    }
}

