using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Kill:TwoCharactersAction
    {
        public override string Name
        {
            get { return "Kill"; }
        }

        public double ExperienceTaken { get; set; }

        public override bool Perform(ICharacter active, ICharacter passive)
        {
            active.Experience += (int)Math.Ceiling(passive.Experience*ExperienceTaken);
            if(passive.StorageContainer!=null && passive.StorageContainer.Owner!=null)
                passive.StorageContainer.Remove(passive);
            passive = null;
            return true;
        }
    }
}
