using _2DEngineData;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CardGameEngineData
{
    public class DeckData : BaseData
    {
        /// <summary>
        /// The name of the deck
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of card data assets in our deck
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<string> CardDataAssets { get; set; }
    }
}
