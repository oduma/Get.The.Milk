using System.Linq;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Pick : OneObjectAction
    {
        public Pick()
        {
            Name = new Verb {Infinitive = "To Pick", Past = "picked", Present = "pick"};
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

