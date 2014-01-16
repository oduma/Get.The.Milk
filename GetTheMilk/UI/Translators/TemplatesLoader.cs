using System.Collections.Generic;
using System.IO;
using System.Linq;
using GetTheMilk.UI.Translators.Common;
using GetTheMilk.UI.Translators.MovementResultTemplates;
using Sciendo.Common.Serialization;

namespace GetTheMilk.UI.Translators
{
    public class TemplatesLoader
    {
        public const string ActionResultFilesFilter = "*ResultTemplates.xml";
        public const string ExtraDataFilesFilter = "*ExtraDataTemplate.xml";
        public List<MessagesFor> LoadActionResultsTemplates(string directory)
        {
            var result= new List<MessagesFor>();
            if (!Directory.Exists(directory))
                return result;
            foreach(var translatorDataFile in Directory.GetFiles(directory,ActionResultFilesFilter))
            {
                result.AddRange(Serializer.DeserializeFromFile<MessagesFor>(translatorDataFile));
            }
            return result;
        }
        public MovementExtraDataTemplate LoadActionExtraDataTemplate(string directory)
        {
            var result = new MovementExtraDataTemplate();
            if (!Directory.Exists(directory))
                return result;
            var translatorDataFile = Directory.GetFiles(directory, ExtraDataFilesFilter);
            if(!translatorDataFile.Any())
                return result;
            return Serializer.DeserializeOneFromFile<MovementExtraDataTemplate>(translatorDataFile[0]);
        }

    }
}
