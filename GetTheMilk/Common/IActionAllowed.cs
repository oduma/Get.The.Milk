using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Common
{
    public interface IActionAllowed
    {
        string ObjectTypeId { get; set; }

        ObjectCategory ObjectCategory { get; set; }

        bool AllowsTemplateAction(BaseActionTemplate actionTemplate);

        bool AllowsIndirectTemplateAction(BaseActionTemplate actionTemplate, IPositionable obj);

    }
}
