using GetTheMilk.Actions.ActionTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public interface IObjectResponseActionTemplatePerformer:IActionTemplatePerformer
    {
        bool CanPerform(ObjectResponseActionTemplate actionTemplate);
        PerformActionResult Perform(ObjectResponseActionTemplate actionTemplate);

    }
}
