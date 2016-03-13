using _2DEngineData;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CardGameEngineData
{
    public class CardRegistryData : BaseData
    {
        /// <summary>
        /// The central list of all the available resource cards in the game.
        /// Do not need to add 'Content\Data\Cards\' on the front, e.g. an item in the XML may be <Item>Resources\Crew\CrewResourceCard.xml</Item>
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<string> ResourceCardDataAssets { get; set; }

        /// <summary>
        /// The central list of all the available ship cards in the game.
        /// Do not need to add 'Content\Data\Cards\' on the front, e.g. an item in the XML may be <Item>Resources\Crew\CrewResourceCard.xml</Item>
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<string> ShipCardDataAssets { get; set; }
    }
}
