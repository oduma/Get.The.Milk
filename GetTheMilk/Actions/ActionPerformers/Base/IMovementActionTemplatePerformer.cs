using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IMovementActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(MovementActionTemplate actionTemplate);
        PerformActionResult Perform(MovementActionTemplate actionTemplate);


    }
}
