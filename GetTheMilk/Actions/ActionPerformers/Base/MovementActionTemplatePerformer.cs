using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class MovementActionTemplatePerformer:IMovementActionTemplatePerformer
    {
        public string PerformerType
        {
            get { return GetType().Name; }
        }

        public virtual bool CanPerform(MovementActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null || actionTemplate.CurrentMap == null)
                return false;
            return actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate);
        }

        public virtual PerformActionResult Perform(MovementActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter.CellNumber < 0 || actionTemplate.ActiveCharacter.CellNumber >= actionTemplate.CurrentMap.Cells.Length)
                return MoveActiveCharacter(actionTemplate, actionTemplate.ActiveCharacter.CellNumber,
                               ActionResultType.OriginNotOnTheMap, new NonCharacterObject[0], new Character[0]);

            var currentCell = actionTemplate.CurrentMap.Cells[actionTemplate.ActiveCharacter.CellNumber];

            for (int i = 0; i < actionTemplate.DefaultDistance; i++)
            {
                int nextCellId = currentCell.GetNeighbourCellNumber(actionTemplate.Direction);
                var moveActiveCharacter = MoveOneStep(nextCellId, currentCell,actionTemplate);
                if (moveActiveCharacter != null)
                    return moveActiveCharacter;

                currentCell = actionTemplate.CurrentMap.Cells[nextCellId];

            }

            return MoveActiveCharacter(actionTemplate, currentCell.Number,
                                           ActionResultType.Ok, new NonCharacterObject[0], new Character[0]);
        }

        protected PerformActionResult MoveActiveCharacter(MovementActionTemplate actionTemplate, 
            int targetCellId,
            ActionResultType resultType, 
            IEnumerable<NonCharacterObject> objectsBlocking,
            IEnumerable<Character> charactersBlocking)
        {
            if (resultType == ActionResultType.OriginNotOnTheMap)
                return new PerformActionResult
                           {
                               ForAction = actionTemplate,
                               ResultType = resultType,
                               ExtraData = new MovementActionTemplateExtraData()
                           };
            if (resultType == ActionResultType.Ok
                && actionTemplate.ActiveCharacter is IPlayer
                && actionTemplate.CurrentMap.Parent.ObjectiveCell == targetCellId)
                return new PerformActionResult
                           {ForAction = actionTemplate, ResultType = ActionResultType.LevelCompleted};
            if (resultType == ActionResultType.LevelCompleted
                && actionTemplate.ActiveCharacter is IPlayer)
                return new PerformActionResult
                           {ForAction = actionTemplate, ResultType = ActionResultType.LevelCompleted};
            var movementResult = new PerformActionResult
                                     {
                                         ForAction = actionTemplate,
                                         ExtraData = new MovementActionTemplateExtraData
                                                         {
                                                             ObjectsBlocking = objectsBlocking,
                                                             CharactersBlocking = charactersBlocking,
                                                             CharactersInRange = actionTemplate.CurrentMap.Cells.Where(c => actionTemplate.CurrentMap.AreInRange(targetCellId, c.Number)).SelectMany(o => o.AllCharacters),
                                                             ObjectsInRange = actionTemplate.CurrentMap.Cells.Where(c => actionTemplate.CurrentMap.AreInRange(targetCellId, c.Number)).SelectMany(o => o.AllObjects),
                                                             ObjectsInCell = actionTemplate.CurrentMap.Cells[targetCellId].AllObjects
                                                         },
                                         ResultType = resultType
                                     };
            if (targetCellId != -1)
            {
                actionTemplate.ActiveCharacter.CellNumber = targetCellId;
                actionTemplate.ActiveCharacter.Inventory.FollowTheLeader();
            }
            var objects = ((MovementActionTemplateExtraData) movementResult.ExtraData).ObjectsInCell.Union(
                ((MovementActionTemplateExtraData) movementResult.ExtraData).ObjectsInRange).ToArray();

            ((MovementActionTemplateExtraData)movementResult.ExtraData).AvailableActionTemplates= new List<BaseActionTemplate>();
            ((MovementActionTemplateExtraData) movementResult.ExtraData).AvailableActionTemplates.AddRange(
                (GetActionsOnObjects(actionTemplate,objects))??new BaseActionTemplate[0]);

            ((MovementActionTemplateExtraData) movementResult.ExtraData).AvailableActionTemplates.AddRange(
                (GetActionsOnCharacters(actionTemplate,((MovementActionTemplateExtraData) movementResult.ExtraData).CharactersInRange))??new TwoCharactersActionTemplate[0]);


            return movementResult;
        }

        private IEnumerable<BaseActionTemplate> GetActionsOnObjects(MovementActionTemplate actionTemplate, IEnumerable<NonCharacterObject> objectsInRange)
        {
            foreach (var objectInRange in objectsInRange)
            {

                actionTemplate.ActiveCharacter.LoadInteractions(objectInRange, objectInRange.GetType());
                foreach (var templateAction in actionTemplate.ActiveCharacter.AllActions
                    .Where(a => !(a.Value.GetType() == typeof(TwoCharactersActionTemplate)
                        || a.Value.GetType() == typeof(MovementActionTemplate)) && a.Value.StartingAction))
                {
                    if (templateAction.Value.GetType() == typeof(ObjectUseOnObjectActionTemplate))
                        foreach (var activeObject in actionTemplate.ActiveCharacter.Inventory)
                        {
                            var action =
                                actionTemplate.ActiveCharacter.CreateNewInstanceOfAction
                                    <ObjectUseOnObjectActionTemplate>(templateAction.Key);

                            action.ActiveCharacter = actionTemplate.ActiveCharacter;
                            action.TargetObject = objectInRange;
                            action.ActiveObject = activeObject;
                            if (action.ActiveCharacter.CanPerformAction(action))
                                yield return action;
                        }
                    else if (templateAction.Value.GetType() == typeof(OneObjectActionTemplate))
                    {
                        var action =
                            actionTemplate.ActiveCharacter.CreateNewInstanceOfAction<OneObjectActionTemplate>(
                                templateAction.Key);

                        action.ActiveCharacter = actionTemplate.ActiveCharacter;
                        action.TargetObject = objectInRange;
                        if (action.ActiveCharacter.CanPerformAction(action))
                            yield return action;
                    }
                    else if (templateAction.Value.GetType() == typeof(ObjectTransferActionTemplate))
                    {
                        var action =
                            actionTemplate.ActiveCharacter.CreateNewInstanceOfAction<ObjectTransferActionTemplate>(
                                templateAction.Key);

                        action.ActiveCharacter = actionTemplate.ActiveCharacter;
                        action.TargetObject = objectInRange;
                        if (action.ActiveCharacter.CanPerformAction(action))
                            yield return action;
                    }
                }
            }
        }

        private IEnumerable<TwoCharactersActionTemplate> GetActionsOnCharacters(MovementActionTemplate actionTemplate,IEnumerable<Character> charactersInRange)
        {
            foreach (var characterInRange in charactersInRange)
            {
                actionTemplate.ActiveCharacter.LoadInteractions(characterInRange, characterInRange.GetType());
                foreach (var action in actionTemplate.ActiveCharacter.AllActions
                    .Where(a => a.Value.GetType() == typeof(TwoCharactersActionTemplate) && a.Value.StartingAction))
                {
                    var executableAction = actionTemplate.ActiveCharacter.CreateNewInstanceOfAction<TwoCharactersActionTemplate>(action.Key);

                    executableAction.ActiveCharacter = actionTemplate.ActiveCharacter;
                    executableAction.TargetCharacter = characterInRange;
                    if(executableAction.ActiveCharacter.CanPerformAction(executableAction))
                        yield return executableAction;
                }
            }
        }

        protected PerformActionResult MoveOneStep(int nextCellId, Cell currentCell, MovementActionTemplate actionTemplate)
        {
            if (nextCellId == -1)
                return MoveActiveCharacter(actionTemplate, currentCell.Number,
                    ActionResultType.OutOfTheMap, new NonCharacterObject[0], new Character[0]);
            var objectsBlocking = (actionTemplate.CurrentMap.Cells[nextCellId].AllObjects == null)
                ? new NonCharacterObject[0]
                : actionTemplate.CurrentMap.Cells[nextCellId].AllObjects.Where(o => o.BlockMovement);

            var charactersBlocking = (actionTemplate.CurrentMap.Cells[nextCellId].AllCharacters == null)
                ? new Character[0]
                : actionTemplate.CurrentMap.Cells[nextCellId].AllCharacters.Where(o => o.BlockMovement);

            if (objectsBlocking.Any() || charactersBlocking.Any())
                return MoveActiveCharacter(actionTemplate, currentCell.Number,
                    ActionResultType.Blocked, objectsBlocking, charactersBlocking);
            if (currentCell.Parent.Parent.ObjectiveCell == currentCell.Number)
                return MoveActiveCharacter(actionTemplate, currentCell.Number,
                    ActionResultType.LevelCompleted, new NonCharacterObject[0], new Character[0]);
            return null;
        }

        public string Category { get { return CategorysCatalog.MovementCategory; } }
    }
}
