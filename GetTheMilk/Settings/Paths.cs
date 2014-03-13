using System;
using System.IO;

namespace GetTheMilk.Settings
{
    public class Paths
    {
        public string ActionResultsTemplates
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"UI\Data\"); }
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

        public string GameData
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"GameData"); }
        }

        public string GameDescriptionFileName
        {
            get { return "GDES.gdu"; }
        }

        public string TemplatesFileName
        {
            get { return "GDT.gdu"; }
        }

        public string NewGameDataFile
        {
            get { return "GD.gdu"; }
        }

        public string LevelsFileNameTemplate
        {
            get { return "GL{0}.gdu"; }
        }
    }
}
