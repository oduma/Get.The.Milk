using System;
using System.Xml.Serialization;

namespace GetTheMilk.UI.Translators.MovementResultTemplates
{
    [Serializable]
    public class Message
    {
        [XmlAttribute(AttributeName="Id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName="Value")]
        public string Value { get; set; }

    }
}
