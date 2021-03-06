﻿using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class TeleportActionPerformer:MovementActionTemplatePerformer
    {
        public override PerformActionResult Perform(MovementActionTemplate actionTemplate)
        {
            if (actionTemplate.Direction != Direction.None)
                return new PerformActionResult { ResultType = ActionResultType.UnknownError, ForAction = actionTemplate };
            var movementResult = MoveOneStep(actionTemplate.TargetCell, actionTemplate.CurrentMap.Cells[actionTemplate.ActiveCharacter.CellNumber],actionTemplate);
            if (movementResult == null)
                return MoveActiveCharacter(actionTemplate, actionTemplate.TargetCell,
                               ActionResultType.Ok, new NonCharacterObject[0], new Character[0]);
            if (movementResult.ResultType == ActionResultType.LevelCompleted)
                return movementResult;
            return null;
        }

        public override bool CanPerform(MovementActionTemplate actionTemplate)
        {
            return (base.CanPerform(actionTemplate) && actionTemplate.TargetCell >= 0 &&
                    actionTemplate.Direction == Direction.None);
        }
    }
}
