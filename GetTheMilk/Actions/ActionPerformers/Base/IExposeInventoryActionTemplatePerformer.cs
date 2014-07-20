using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IExposeInventoryActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(ExposeInventoryActionTemplate actionTemplate);

        PerformActionResult Perform(ExposeInventoryActionTemplate actionTemplate);

    }
}
