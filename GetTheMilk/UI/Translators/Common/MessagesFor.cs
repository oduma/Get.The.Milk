using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GetTheMilk.Actions;

namespace GetTheMilk.UI.Translators.Common
{
    [Serializable]
    public class MessagesFor
    {
        [XmlAttribute(AttributeName="ResultType")]
        public ActionResultType ResultType { get; set; }

        public List<Message> Messages { get; set; }

    }
}
