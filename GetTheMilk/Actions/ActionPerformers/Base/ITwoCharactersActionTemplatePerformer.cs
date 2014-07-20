using System;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface ITwoCharactersActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(TwoCharactersActionTemplate actionTemplate);
        PerformActionResult Perform(TwoCharactersActionTemplate actionTemplate);

        event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;

    }
}
