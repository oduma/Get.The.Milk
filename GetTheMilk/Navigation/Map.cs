using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Navigation
{
    public class Map
    {
        public int Number { get; set; }

        public Cell[] Cells { get; set; }

        public ActionResult CalculateMovement(ICharacter active, 
            int defaultDistance, 
            Direction direction,
            IEnumerable<IPositionableObject> allLevelObjects,
            IEnumerable<IPositionableObject> allLevelCharacters)
        {
            var currentCell = Cells.FirstOrDefault(c => c.Number == active.CellNumber);
            int nextCellId=0;
            if(currentCell!=null)
            {
                for(int i=0;i<defaultDistance;i++)
                {
                    nextCellId = currentCell.GetNeighbourCellNumber(direction);
                    if (nextCellId == 0)
                        return CalculateMovementResult(active.MapNumber,currentCell.Number, allLevelObjects, allLevelCharacters, 
                                                       ActionResultType.OutOfTheMap,new IPositionableObject[0]);
                    if (allLevelObjects != null)
                    {
                        var objectsBlocking =
                            allLevelObjects.Where(o => (o.MapNumber == Number && o.CellNumber == nextCellId)&&o.BlockMovement);
                        if (objectsBlocking.Any())
                            return CalculateMovementResult(active.MapNumber,currentCell.Number, allLevelObjects, allLevelCharacters, 
                                                           ActionResultType.BlockedByObject, objectsBlocking.ToArray());
                    }
                    var blockingCharacters = (allLevelCharacters == null)
                                                 ? null
                                                 : allLevelCharacters.Where(c => c.BlockMovement);
                    if (blockingCharacters != null)
                    {
                        var charactersBlocking =
                            blockingCharacters.Where(o => o.MapNumber == Number && o.CellNumber == nextCellId);
                        if (charactersBlocking.Any())
                            return CalculateMovementResult(active.MapNumber,currentCell.Number, allLevelObjects, allLevelCharacters, 
                                                           ActionResultType.BlockedByCharacter,
                                                           charactersBlocking.ToArray());
                    }
                    currentCell = Cells.FirstOrDefault(c => c.Number == nextCellId);
                    if(currentCell.IsObjective)
                    {
                        return CalculateMovementResult(active.MapNumber,nextCellId, allLevelObjects, allLevelCharacters, 
                                                       ActionResultType.LevelCompleted, new IPositionableObject[0]);
                    }
                }
            }
            else
                return CalculateMovementResult(active.MapNumber,active.CellNumber, allLevelObjects, allLevelCharacters,
                                               ActionResultType.OriginNotOnTheMap, new IPositionableObject[0]);

            return CalculateMovementResult(active.MapNumber,nextCellId, allLevelObjects, allLevelCharacters,
                                           ActionResultType.Ok, new IPositionableObject[0]);
        }

        public ActionResult CalculateDirectMovement(int targetMapNumber, 
            int targetCellId,
            Direction direction,
            IEnumerable<IPositionableObject> allLevelObjects,
            IEnumerable<IPositionableObject> allLevelCharacters)
        {
            if (direction != Direction.None)
                return new ActionResult {ResultType = ActionResultType.UnknownError};
            var blockingObjects = allLevelObjects.Where(
                o => o.MapNumber == targetMapNumber && o.CellNumber == targetCellId);
            var blockingCharacters=
                    allLevelCharacters.Where(
                        c =>
                        c.MapNumber == targetMapNumber && c.CellNumber == targetCellId);
            if (blockingObjects.Any())
                return CalculateMovementResult(targetMapNumber, targetCellId, allLevelObjects, allLevelCharacters,
                                               ActionResultType.BlockedByObject, blockingObjects.ToArray());
            if(blockingCharacters.Any())
                return CalculateMovementResult(targetMapNumber, targetCellId, allLevelObjects, allLevelCharacters,
                                               ActionResultType.BlockedByCharacter, blockingCharacters.ToArray());
            
            return CalculateMovementResult(targetMapNumber, targetCellId, allLevelObjects, allLevelCharacters,
                               ActionResultType.Ok, new IPositionableObject[0]);
            }
        private ActionResult CalculateMovementResult(int targetMapNumber,int targetCellId, IEnumerable<IPositionableObject> allObjects,
                                                       IEnumerable<IPositionableObject> allLevelCharacters,
                                                       ActionResultType resultType,IPositionableObject[] objectsBlocking)
        {

            var movementResult = new ActionResult
                                     {
                                         ExtraData = new MovementActionExtraData
                                                         {
                                                             MoveToCell =targetCellId,
                                                             ObjectsBlocking = objectsBlocking
                                                         },
                                         ResultType = resultType
                                     };
            ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange = (allLevelCharacters == null)
                                                   ? null
                                                   : allLevelCharacters.Where(
                                                       c =>
                                                       AreInRange(c.MapNumber, c.CellNumber,
                                                                  targetMapNumber,
                                                                  ((MovementActionExtraData)movementResult.ExtraData).MoveToCell)).ToArray();
            ((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange = (allObjects == null)
                                                ? null
                                                : allObjects.Where(
                                                    c =>
                                                    AreInRange(c.MapNumber, c.CellNumber, targetMapNumber,
                                                               ((MovementActionExtraData)movementResult.ExtraData).MoveToCell) &&
                                                    c is INonCharacterObject)
                                                      .ToArray();
            ((MovementActionExtraData)movementResult.ExtraData).ObjectsInCell = ((allLevelCharacters == null)
                                                   ? new IPositionableObject[0]
                                                   : allLevelCharacters.Where(
                                                       c =>c.MapNumber==targetMapNumber && c.CellNumber==
                                                                  ((MovementActionExtraData)movementResult.ExtraData).MoveToCell))
                                                .Union(((allObjects == null)
                                                ? new IPositionableObject[0]
                                                : allObjects.Where(
                                                    c =>c.MapNumber==targetMapNumber && c.CellNumber==((MovementActionExtraData)movementResult.ExtraData).MoveToCell &&
                                                    c is INonCharacterObject)))
                                               .ToArray();
            return movementResult;
        }

        public bool AreInRange(int activeMapNumber, int activeCellNumber, int passiveMapNumber, int passiveCellNumber)
        {
            if (activeMapNumber != passiveMapNumber)
                return false;
            var activeCell = Cells.FirstOrDefault(c => c.Number == activeCellNumber);
            if (activeCell == null)
                return false;
            return activeCell.IsANeighbourOfOrSelf(passiveCellNumber);

        }
    }
}
