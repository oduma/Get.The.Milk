using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Quit : FightAction
    {
        public Quit()
        {
            Name = new Verb {Infinitive = "To Quit", Past = "quited", Present = "quit"};
        }
        public ActionResult Perform(ICharacter character, ICharacter targetCharacter)
        {

            return new ActionResult {ResultType = ActionResultType.RequestQuit};
        }

    }
}
