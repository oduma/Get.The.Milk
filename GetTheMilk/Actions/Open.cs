using GetTheMilk.Actions.GenericActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Open: DestroyBothObjects
    {
        public Open()
        {
            Name = new Verb {Infinitive = "To Open", Past = "opened", Present = "open"};
        }
    }
}
