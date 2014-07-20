using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IObjectUseOnObjectActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(ObjectUseOnObjectActionTemplate actionTemplate);
        PerformActionResult Perform(ObjectUseOnObjectActionTemplate actionTemplate);


    }
}
