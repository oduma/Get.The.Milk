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

        public string SaveDefaultPath
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Saved");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}
