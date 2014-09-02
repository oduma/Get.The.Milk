using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects.Base;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class MovementActionTemplatePerformer:IActionTemplatePerformer
    {
        public string PerformerType
        {
            get { return GetType().Name; }
        }

        public virtual bool CanPerform(BaseActionTemplate act)
        {
            MovementActionTemplate actionTemplate;
            if (!act.TryConvertTo(out actionTemplate))
                return false;

            if (actionTemplate.ActiveCharacter == null || actionTemplate.CurrentMap == null)
                return false;
            return actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate);
        }

        public virtual PerformActionResult Perform(BaseActionTemplate act)
        {
            MovementActionTemplate actionTemplate;
            if (!act.TryConvertTo(out actionTemplate))
                return new PerformActionResult { ResultType = ActionResultType.NotOk, ForAction = act };

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
                (GetActionsOnObjects(actionTemplate,objects)));

            ((MovementActionTemplateExtraData) movementResult.ExtraData).AvailableActionTemplates.AddRange(
                (GetActionsOnCharacters(actionTemplate,((MovementActionTemplateExtraData) movementResult.ExtraData).CharactersInRange)));


            return movementResult;
        }

        private IEnumerable<BaseActionTemplate> GetActionsOnObjects(MovementActionTemplate actionTemplate, IEnumerable<NonCharacterObject> objectsInRange)
        {
            foreach (var objectInRange in objectsInRange)
            {

                actionTemplate.ActiveCharacter.LoadInteractions(objectInRange, objectInRange.GetType());
                foreach (var templateAction in actionTemplate.ActiveCharacter.AllActions
                    .Where(a => !(a.Value.GetType() == typeof(TwoCharactersActionTemplate)
                        || a.Value is MovementActionTemplate) && a.Value.StartingAction))
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

            var nonCharacterObjects = objectsBlocking as NonCharacterObject[] ?? objectsBlocking.ToArray();
            var characters = charactersBlocking as Character[] ?? charactersBlocking.ToArray();
            if (nonCharacterObjects.Any() || characters.Any())
                return MoveActiveCharacter(actionTemplate, currentCell.Number,
                    ActionResultType.Blocked, nonCharacterObjects, characters);
            if (currentCell.Parent.Parent.ObjectiveCell == currentCell.Number)
                return MoveActiveCharacter(actionTemplate, currentCell.Number,
                    ActionResultType.LevelCompleted, new NonCharacterObject[0], new Character[0]);
            return null;
        }

        public string Category { get { return CategorysCatalog.MovementCategory; } }


        public event System.EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        public event System.EventHandler<FeedbackEventArgs> FeedbackFromSubAction;
    }
}
