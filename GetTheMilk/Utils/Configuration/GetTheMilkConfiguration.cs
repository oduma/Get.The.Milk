using System;
using System.Configuration;

namespace GetTheMilk.Utils.Configuration
{
    public class GetTheMilkConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("GameDataDirectory", DefaultValue = "GameData", IsRequired = true)]
        public string GameDataDirectory
        {
            get { return (string)this["GameDataDirectory"]; }
            set { this["GameDataDirectory"] = value; }
        }

        [ConfigurationProperty("SaveDirectory", IsRequired = true, DefaultValue = "Saved")]
        public string SaveDirectory
        {
            get
            {
                try
                {
                    return (string)this["SaveDirectory"];

                }
                catch
                {
                    return "Saved";
                }
            }
            set
            {
                this["SaveDirectory"] = value;
            }
        }
    }
}