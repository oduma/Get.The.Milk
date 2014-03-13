using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using Newtonsoft.Json;

namespace GetTheMilk.Actions.BaseActions
{
    public class MovementAction : GameAction
    {
        public Direction Direction { get; set; }

        [JsonIgnore]
        public int DefaultDistance { get; protected set; }

        public bool Perform(ICharacter active)
        {
            if (TargetCell == 0)
                return false;
            active.CellNumber = TargetCell;
            if(active.Inventory!=null)
                active.Inventory.FollowTheLeader();
            return true;
        }

        public int PossibleDistance { get; set; }

        public int TargetCell { get; set; }
    }
}
