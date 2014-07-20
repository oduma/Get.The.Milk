using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface INoObjectActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(NoObjectActionTemplate actionTemplate);
        PerformActionResult Perform(NoObjectActionTemplate actionTemplate);

    }
}
