using System.Collections.Generic;
using System.IO;
using GetTheMilk.UI.Translators.MovementResultTemplates;
using Sciendo.Common.Serialization;

namespace GetTheMilk.UI.Translators
{
    public class ActionResultsTemplatesLoader
    {
        public List<MessagesFor> LoadActionResultsTemplates(string directory)
        {
            var result= new List<MessagesFor>();
            if (!Directory.Exists(directory))
                return result;
            foreach(var translatorDataFile in Directory.GetFiles(directory))
            {
                result.AddRange(Serializer.DeserializeFromFile<MessagesFor>(translatorDataFile));
            }
            return result;
        }
    }
}
