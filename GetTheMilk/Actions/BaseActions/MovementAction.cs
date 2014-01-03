using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;

namespace GetTheMilk.Actions.BaseActions
{
    public abstract class MovementAction : GameAction
    {
        public Direction Direction { get; set; }

        public abstract int DefaultDistance { get; }

        public bool Perform(Character active)
        {
            if (TargetCell == 0)
                return false;
            active.CellNumber = TargetCell;
            active.RightHandObject.FollowTheLeader();
            active.LeftHandObject.FollowTheLeader();
            active.ToolInventory.FollowTheLeader();
            active.WeaponInventory.FollowTheLeader();
            return true;
        }

        public int PossibleDistance { get; set; }

        public int TargetCell { get; set; }
    }
}
