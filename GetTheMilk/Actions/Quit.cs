using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Quit : FightAction
    {
        public override string Name
        {
            get { return "Quit"; }
        }

        public ActionResult Perform(ICharacter character, ICharacter targetCharacter)
        {

            return new ActionResult {ResultType = ActionResultType.RequestQuit};
        }

    }
}
