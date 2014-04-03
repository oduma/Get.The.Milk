using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Navigation;
using GetTheMilk.Objects;
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

            if(ActiveCharacter.CellNumber<0 || ActiveCharacter.CellNumber>=CurrentMap.Cells.Length)
                return MoveActiveCharacter(this,ActiveCharacter.CellNumber,
                               ActionResultType.OriginNotOnTheMap, new NonCharacterObject[0], new Character[0]);

            var currentCell = CurrentMap.Cells[ActiveCharacter.CellNumber];

            for (int i = 0; i < DefaultDistance; i++)
            {
                int nextCellId = currentCell.GetNeighbourCellNumber(Direction);
                var moveActiveCharacter = MoveOneStep(nextCellId, currentCell);
                if (moveActiveCharacter != null) 
                    return moveActiveCharacter;

                currentCell = CurrentMap.Cells[nextCellId];

            }

            return MoveActiveCharacter(this,currentCell.Number,
                                           ActionResultType.Ok, new NonCharacterObject[0], new Character[0]);
        }

        protected ActionResult MoveOneStep(int nextCellId, Cell currentCell)
        {
            if (nextCellId == -1)
                return MoveActiveCharacter(this,currentCell.Number,
                    ActionResultType.OutOfTheMap, new NonCharacterObject[0], new Character[0]);
            var objectsBlocking = (CurrentMap.Cells[nextCellId].AllObjects == null)
                ? new NonCharacterObject[0]
                : CurrentMap.Cells[nextCellId].AllObjects.Where(o => o.BlockMovement);

            var charactersBlocking = (CurrentMap.Cells[nextCellId].AllCharacters == null)
                ? new Character[0]
                : CurrentMap.Cells[nextCellId].AllCharacters.Where(o => o.BlockMovement);

            if (objectsBlocking.Any() || charactersBlocking.Any())
                return MoveActiveCharacter(this,currentCell.Number,
                    ActionResultType.Blocked, objectsBlocking, charactersBlocking);
            if (currentCell.IsObjective)
                return MoveActiveCharacter(this,currentCell.Number,
                    ActionResultType.LevelCompleted, new NonCharacterObject[0], new Character[0]);
            return null;
        }

        protected ActionResult MoveActiveCharacter(MovementAction actionPerformed,int targetCellId,
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
                ActiveCharacter.CellNumber = targetCellId;
                ActiveCharacter.Inventory.FollowTheLeader();
            }

            ((MovementActionExtraData) movementResult.ExtraData).AvailableActions = new List<GameAction>();
            ((MovementActionExtraData) movementResult.ExtraData).AvailableActions.AddRange(
                GetActionsOnObjects(((MovementActionExtraData) movementResult.ExtraData).ObjectsInCell.Union(
                    ((MovementActionExtraData) movementResult.ExtraData).ObjectsInRange)));
            ((MovementActionExtraData)movementResult.ExtraData).AvailableActions.AddRange(
                GetActionsOnCharacters(((MovementActionExtraData)movementResult.ExtraData).CharactersInRange));


            return movementResult;
        }

        private IEnumerable<GameAction> GetActionsOnCharacters(IEnumerable<Character> charactersInRange)
        {
            foreach (var characterInRange in charactersInRange)
            {
                foreach (var templateAction in ActionsFactory.GetFactory().GetActions()
                    .Where(a => a is TwoCharactersAction && a.StartingAction))
                {
                        var action = ActionsFactory.GetFactory().CreateNewActionInstance(templateAction.ActionType);

                        action.ActiveCharacter = ActiveCharacter;
                        action.TargetCharacter = characterInRange;
                        if (action.CanPerform())
                            yield return action;
                }
            }
        }

        private IEnumerable<GameAction> GetActionsOnObjects(IEnumerable<NonCharacterObject> objectsInRange)
        {
            foreach (var objectInRange in objectsInRange)
            {
                foreach (var templateAction in ActionsFactory.GetFactory().GetActions()
                    .Where(a => !(a is TwoCharactersAction 
                        || a is ObjectTransferFromAction 
                        || a is ObjectTransferToAction 
                        || a is MovementAction) && a.StartingAction))
                {
                    if (templateAction is ObjectUseOnObjectAction)
                        foreach (var activeObject in ActiveCharacter.Inventory)
                        {
                            var action = ActionsFactory.GetFactory().CreateNewActionInstance(templateAction.ActionType);

                            action.ActiveCharacter = ActiveCharacter;
                            action.TargetObject = objectInRange;
                            action.ActiveObject = activeObject;
                            if (action.CanPerform())
                                yield return action;
                        }
                    else if (templateAction is OneObjectAction)
                    {
                        var action = ActionsFactory.GetFactory().CreateNewActionInstance(templateAction.ActionType);

                        action.ActiveCharacter = ActiveCharacter;
                        action.TargetObject = objectInRange;
                        if (action.CanPerform())
                            yield return action;
                    }
                }
            }
        }


        public override GameAction CreateNewInstance()
        {
            return new MovementAction();
        }


    }
}
