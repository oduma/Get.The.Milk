using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
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
            IEnumerable<NonCharacterObject> allLevelObjects,
            IEnumerable<Character> allLevelCharacters)
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
                                                       ActionResultType.OutOfTheMap,new NonCharacterObject[0], new Character[0]);
                    NonCharacterObject[] objectsBlocking= new NonCharacterObject[0];
                    if (allLevelObjects != null)
                    {
                        objectsBlocking =
                            allLevelObjects.Where(o => (o.MapNumber == Number && o.CellNumber == nextCellId)&&o.BlockMovement).ToArray();
                    }
                    var blockingCharacters = (allLevelCharacters == null)
                                                 ? null
                                                 : allLevelCharacters.Where(c => c.BlockMovement);
                    Character[] charactersBlocking=new Character[0];
                    if (blockingCharacters != null)
                    {
                        charactersBlocking =
                            blockingCharacters.Where(o => o.MapNumber == Number && o.CellNumber == nextCellId).ToArray();
                    }
                    if(objectsBlocking.Any() || charactersBlocking.Any())
                    return CalculateMovementResult(active.MapNumber, currentCell.Number, allLevelObjects, allLevelCharacters,
                               ActionResultType.Blocked, objectsBlocking,charactersBlocking);

                    currentCell = Cells.FirstOrDefault(c => c.Number == nextCellId);
                    if(currentCell.IsObjective)
                    {
                        return CalculateMovementResult(active.MapNumber,nextCellId, allLevelObjects, allLevelCharacters, 
                                                       ActionResultType.LevelCompleted, new NonCharacterObject[0], new Character[0]);
                    }
                }
            }
            else
                return CalculateMovementResult(active.MapNumber,active.CellNumber, allLevelObjects, allLevelCharacters,
                                               ActionResultType.OriginNotOnTheMap, new NonCharacterObject[0], new Character[0]);

            return CalculateMovementResult(active.MapNumber,nextCellId, allLevelObjects, allLevelCharacters,
                                           ActionResultType.Ok, new NonCharacterObject[0], new Character[0]);
        }

        public ActionResult CalculateDirectMovement(int targetMapNumber, 
            int targetCellId,
            Direction direction,
            IEnumerable<NonCharacterObject> allLevelObjects,
            IEnumerable<Character> allLevelCharacters)
        {
            if (direction != Direction.None)
                return new ActionResult {ResultType = ActionResultType.UnknownError};
            var blockingObjects = allLevelObjects.Where(
                o => o.MapNumber == targetMapNumber && o.CellNumber == targetCellId);
            var blockingCharacters=
                    allLevelCharacters.Where(
                        c =>
                        c.MapNumber == targetMapNumber && c.CellNumber == targetCellId);
            if(blockingObjects.Any() || blockingCharacters.Any())
                return CalculateMovementResult(targetMapNumber, targetCellId, allLevelObjects, allLevelCharacters,
                                               ActionResultType.Blocked, blockingObjects.ToArray(),blockingCharacters.ToArray());
            
            return CalculateMovementResult(targetMapNumber, targetCellId, allLevelObjects, allLevelCharacters,
                               ActionResultType.Ok, new NonCharacterObject[0],new Character[0]);
            }
        private ActionResult CalculateMovementResult(int targetMapNumber,int targetCellId, IEnumerable<NonCharacterObject> allObjects,
                                                       IEnumerable<Character> allLevelCharacters,
                                                       ActionResultType resultType,NonCharacterObject[] objectsBlocking,Character[] charactersBlocking)
        {

            var movementResult = new ActionResult
                                     {
                                         ExtraData = new MovementActionExtraData
                                                         {
                                                             MoveToCell =targetCellId,
                                                             ObjectsBlocking = objectsBlocking,
                                                             CharactersBlocking=charactersBlocking
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
                                                    c is NonCharacterObject)
                                                      .ToArray();
            ((MovementActionExtraData)movementResult.ExtraData).ObjectsInCell = ((allObjects == null)
                                                ? new NonCharacterObject[0]
                                                : allObjects.Where(
                                                    c =>c.MapNumber==targetMapNumber && c.CellNumber==((MovementActionExtraData)movementResult.ExtraData).MoveToCell &&
                                                    c is NonCharacterObject).Select(o=>o as NonCharacterObject))
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
