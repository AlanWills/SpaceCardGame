using _2DEngineData;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CardGameEngineData
{
    public class DeckData : BaseData
    {
        [XmlArrayItem(ElementName = "Item")]
        public List<string> CardDataAssets { get; set; }
    }
}
