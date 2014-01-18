using System;
using System.Xml.Serialization;

namespace GetTheMilk.UI.Translators.MovementResultTemplates
{
    [Serializable]
    public class MovementExtraDataTemplate
    {
        [XmlAttribute(AttributeName="ObjectsInRangeTemplate")]
        public string MessageForObjectsInRange { get; set; }

        [XmlAttribute(AttributeName = "ObjectsInCellTemplate")]
        public string MessageForObjectsInCell { get; set; }

    }
}
