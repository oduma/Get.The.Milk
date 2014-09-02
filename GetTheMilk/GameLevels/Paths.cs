using System;
using System.Configuration;
using System.IO;
using GetTheMilk.Utils;

namespace GetTheMilk.GameLevels
{
    public class Paths
    {
        private GetTheMilkConfiguration _configSection;
        protected GetTheMilkConfiguration ConfigSection
        {
            get { return _configSection = (_configSection) ?? ConfigurationManager.GetSection("GameSettings") as GetTheMilkConfiguration; }
        }

        public string SaveDefaultPath
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigSection.SaveDirectory);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public string GameData
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigSection.GameDataDirectory); }
        }

        public string GameDescriptionFileName
        {
            get { return "GDES.gdu"; }
        }

        public string TemplatesFileName
        {
            get { return "GDT.gdu"; }
        }

        public string LevelsFileNameTemplate
        {
            get { return "GL{0}.gdu"; }
        }
    }
}
