using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Keep:OneObjectAction
    {

        public Keep()
        {
            Name = new Verb {Infinitive = "To Keep", Past = "kept", Present = "keep"};
            ActionType = ActionType.Keep;
        }
        public void Perform(ICharacter c, NonCharacterObject o)
        {
            c.Inventory.Add(o);
        }
    }
}
