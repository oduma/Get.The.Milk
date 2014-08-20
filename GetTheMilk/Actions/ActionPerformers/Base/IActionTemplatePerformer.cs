using GetTheMilk.Actions.ActionTemplates;
using System;
namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IActionTemplatePerformer
    {
        string PerformerType { get; }

        string Category { get; }

        bool CanPerform(BaseActionTemplate actionTemplate);

        PerformActionResult Perform(BaseActionTemplate actionTemplate);

        event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;

    }
}