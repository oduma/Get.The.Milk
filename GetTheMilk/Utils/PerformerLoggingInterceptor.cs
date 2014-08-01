using Castle.DynamicProxy;
using GetTheMilk.Actions.ActionPerformers.Base;
using Sciendo.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.Utils
{
    public class PerformerLoggingInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            if(invocation.Method.ReturnType==typeof(PerformActionResult))
            {
                LoggingManager.Debug(invocation.ReturnValue.ToString());
            }
        }
    }
}
