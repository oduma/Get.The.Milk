using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Teleport:MovementAction
    {
        public override GameAction CreateNewInstance()
        {
            return new Teleport();
        }

        public Teleport()
        {
            Name = new Verb {Infinitive = "To Teleport", Past = "teleported", Present = "teleport"};
            DefaultDistance = 0;
            ActionType = ActionType.Teleport;
        }


        public int TargetCell { get; set; }

        public override ActionResult Perform()
        {
            if (Direction != Direction.None)
                return new ActionResult { ResultType = ActionResultType.UnknownError };
            var movementResult = MoveOneStep(TargetCell, CurrentMap.Cells[ActiveCharacter.CellNumber]);
            if (movementResult != null)
                return null;
            return MoveActiveCharacter(this,TargetCell,
                               ActionResultType.Ok, new NonCharacterObject[0], new Character[0]);
        }
    }
}
