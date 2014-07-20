using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IObjectTransferActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(ObjectTransferActionTemplate actionTemplate);
        PerformActionResult Perform(ObjectTransferActionTemplate actionTemplate);


    }
}
