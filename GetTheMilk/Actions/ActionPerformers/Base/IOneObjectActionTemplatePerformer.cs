using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IOneObjectActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(OneObjectActionTemplate actionTemplate);
        PerformActionResult Perform(OneObjectActionTemplate actionTemplate);


    }
}
