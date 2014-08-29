using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.IOC
{
    public static class ExtentionMethods
    {
        public static IEnumerable<RegisteredType> With(this IEnumerable<RegisteredType> inTypes, LifeStyle lifeStyle)
        {
            foreach(var inType in inTypes)
            {
                yield return inType.With(lifeStyle);
            }
        }
    }
}
