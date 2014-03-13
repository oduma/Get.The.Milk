using System.Collections.Generic;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class ExposeInventory:GameAction
    {
        public ExposeInventory()
        {
            Name = new Verb {Infinitive = "To Expose", Past = "exposed", Present = "expose"};
            ActionType = ActionType.ExposeInventory;
        }

        public GameAction[] AllowedNextActions { get; set; }

        public bool IncludeWallet { get; set; }

        public int Amount { get; set; }

        public ActionResult Perform(ICharacter targetCharacter)
        {
            var result = new ActionResult {ResultType = ActionResultType.Ok};
            var list = new List<NonCharacterObject>();
            list.AddRange(targetCharacter.Inventory.Objects);
            result.ExtraData = new ExposeInventoryExtraData
                                   {
                                       Contents = list.ToArray(),
                                       PossibleUses = AllowedNextActions,
                                       Money = (IncludeWallet) ? Amount : 0
                                   };

            return result;
        }
    }
}
