using System;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Common
{
    public interface IPositionable:IInventoryOwner
    {
        int CellNumber { get; set; }

        bool BlockMovement { get; set; }
        
        string ObjectTypeId { get; set; }

        Func<BaseActionTemplate, bool> AllowsTemplateAction { get; set; }

        Func<BaseActionTemplate, IPositionable, bool> AllowsIndirectTemplateAction { get; set; }


        string SpriteName { get; set; }


    }
}
