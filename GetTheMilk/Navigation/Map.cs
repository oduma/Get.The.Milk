using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Navigation
{
    public class Map
    {
        public int Number { get; set; }

        public Cell[] Cells { get; set; }

        public ActionResult AllowsMovement(ICharacter active, int defaultDistance, Direction direction,
            IEnumerable<IPositionableObject> allLevelObjects,
            IEnumerable<IPositionableObject> allLevelCharacters)
        {
            var currentCell = Cells.FirstOrDefault(c => c.Number == active.CellNumber);
            if(currentCell!=null)
            {
                for(int i=0;i<defaultDistance;i++)
                {
                    var nextCellId = currentCell.GetNeighbourCellNumber(direction);
                    if (nextCellId == 0)
                        return CalculateMovementResult(active, direction, allLevelObjects, allLevelCharacters, i,
                                                       ActionResultType.OutOfTheMap,new IPositionableObject[0]);
                    if (allLevelObjects != null)
                    {
                        var objectsBlocking =
                            allLevelObjects.Where(o => (o.MapNumber == Number && o.CellNumber == nextCellId)&&o.BlockMovement);
                        if (objectsBlocking.Any())
                            return CalculateMovementResult(active, direction, allLevelObjects, allLevelCharacters, i,
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
                            return CalculateMovementResult(active, direction, allLevelObjects, allLevelCharacters, i,
                                                           ActionResultType.BlockedByCharacter,
                                                           charactersBlocking.ToArray());
                    }
                    currentCell = Cells.FirstOrDefault(c => c.Number == nextCellId);
                }
            }
            else
                return CalculateMovementResult(active, direction, allLevelObjects, allLevelCharacters, 0,
                                               ActionResultType.OriginNotOnTheMap, new IPositionableObject[0]);

            return CalculateMovementResult(active, direction, allLevelObjects, allLevelCharacters, defaultDistance,
                                           ActionResultType.Ok, new IPositionableObject[0]);
        }

        private ActionResult CalculateMovementResult(ICharacter active, Direction direction, IEnumerable<IPositionableObject> allObjects,
                                                       IEnumerable<IPositionableObject> allLevelCharacters, int i,
                                                       ActionResultType resultType,IPositionableObject[] objectsBlocking)
        {

            var movementResult = new ActionResult
                                     {
                                         ExtraData = new MovementActionExtraData
                                                         {
                                                             MoveToCell =
                                                                 (resultType == ActionResultType.OriginNotOnTheMap)
                                                                     ? active.CellNumber
                                                                     : CalculateMovement(active, direction, i),
                                                             ObjectsBlocking = objectsBlocking
                                                         },
                                         ResultType = resultType
                                     };
            ((MovementActionExtraData)movementResult.ExtraData).CharactersInRange = (allLevelCharacters == null)
                                                   ? null
                                                   : allLevelCharacters.Where(
                                                       c =>
                                                       AreInRange(c.MapNumber, c.CellNumber,
                                                                  active.MapNumber,
                                                                  ((MovementActionExtraData)movementResult.ExtraData).MoveToCell)).ToArray();
            ((MovementActionExtraData)movementResult.ExtraData).ObjectsInRange = (allObjects == null)
                                                ? null
                                                : allObjects.Where(
                                                    c =>
                                                    AreInRange(c.MapNumber, c.CellNumber, active.MapNumber,
                                                               ((MovementActionExtraData)movementResult.ExtraData).MoveToCell) &&
                                                    c is ITransactionalObject)
                                                      .ToArray();
            return movementResult;
        }

        public int CalculateMovement(ICharacter active, Direction direction, int possibleDistance)
        {
            var currentCell = Cells.FirstOrDefault(c => c.Number == active.CellNumber);
            if (currentCell == null)
                return 0;
            for (int i = 0; i < possibleDistance; i++)
            {
                var nextCellId = currentCell.GetNeighbourCellNumber(direction);
                currentCell = Cells.FirstOrDefault(c => c.Number == nextCellId);
            }
            return currentCell.Number;
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
