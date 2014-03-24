using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using Newtonsoft.Json;

namespace GetTheMilk.Actions.BaseActions
{
    public class MovementAction : GameAction
    {
        public Direction Direction { get; set; }

        [JsonIgnore]
        public int DefaultDistance { get; protected set; }

        public Map CurrentMap { get; set; }
        public override ActionResult Perform()
        {

            if(TargetCharacter.CellNumber<0 || TargetCharacter.CellNumber>=CurrentMap.Cells.Length)
                return MoveTargetCharacter(this,TargetCharacter.CellNumber,
                               ActionResultType.OriginNotOnTheMap, new NonCharacterObject[0], new Character[0]);

            var currentCell = CurrentMap.Cells[TargetCharacter.CellNumber];

            for (int i = 0; i < DefaultDistance; i++)
            {
                int nextCellId = currentCell.GetNeighbourCellNumber(Direction);
                var moveTargetCharacter = MoveOneStep(nextCellId, currentCell);
                if (moveTargetCharacter != null) 
                    return moveTargetCharacter;

                currentCell = CurrentMap.Cells[nextCellId];

            }

            return MoveTargetCharacter(this,currentCell.Number,
                                           ActionResultType.Ok, new NonCharacterObject[0], new Character[0]);
        }

        protected ActionResult MoveOneStep(int nextCellId, Cell currentCell)
        {
            if (nextCellId == -1)
                return MoveTargetCharacter(this,currentCell.Number,
                    ActionResultType.OutOfTheMap, new NonCharacterObject[0], new Character[0]);
            var objectsBlocking = (CurrentMap.Cells[nextCellId].AllObjects == null)
                ? new NonCharacterObject[0]
                : CurrentMap.Cells[nextCellId].AllObjects.Where(o => o.BlockMovement);

            var charactersBlocking = (CurrentMap.Cells[nextCellId].AllCharacters == null)
                ? new Character[0]
                : CurrentMap.Cells[nextCellId].AllCharacters.Where(o => o.BlockMovement);

            if (objectsBlocking.Any() || charactersBlocking.Any())
                return MoveTargetCharacter(this,currentCell.Number,
                    ActionResultType.Blocked, objectsBlocking, charactersBlocking);
            if (currentCell.IsObjective)
                return MoveTargetCharacter(this,currentCell.Number,
                    ActionResultType.LevelCompleted, new NonCharacterObject[0], new Character[0]);
            return null;
        }

        protected ActionResult MoveTargetCharacter(MovementAction actionPerformed,int targetCellId,
                                               ActionResultType resultType, 
            IEnumerable<NonCharacterObject> objectsBlocking, 
            IEnumerable<Character> charactersBlocking)
        {

            var movementResult = new ActionResult
            {
                ForAction=actionPerformed,
                ExtraData = new MovementActionExtraData
                {
                    ObjectsBlocking = objectsBlocking,
                    CharactersBlocking = charactersBlocking
                },
                ResultType = resultType
            };
            ((MovementActionExtraData) movementResult.ExtraData).CharactersInRange =
                CurrentMap.Cells.Where(c => CurrentMap.AreInRange(targetCellId, c.Number))
                    .SelectMany(o => o.AllCharacters);

            ((MovementActionExtraData) movementResult.ExtraData).ObjectsInRange =
                CurrentMap.Cells.Where(c => CurrentMap.AreInRange(targetCellId, c.Number))
                    .SelectMany(o => o.AllObjects);

            ((MovementActionExtraData) movementResult.ExtraData).ObjectsInCell =
                CurrentMap.Cells[targetCellId].AllObjects;
            if (targetCellId != -1)
            {
                TargetCharacter.CellNumber = targetCellId;
                TargetCharacter.Inventory.FollowTheLeader();
            }
            return movementResult;
        }

        public override GameAction CreateNewInstance()
        {
            return new MovementAction();
        }


    }
}
