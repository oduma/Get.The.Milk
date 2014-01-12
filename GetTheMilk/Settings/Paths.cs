using System;
using System.IO;

namespace GetTheMilk.Settings
{
    public class Paths
    {
        public string ActionResultsTemplates
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"UI\Data\");
            }
        }
    }
}
