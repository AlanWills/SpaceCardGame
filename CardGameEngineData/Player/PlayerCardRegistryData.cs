using System.Collections.Generic;
using System.Xml.Serialization;

namespace CardGameEngineData
{
    /// <summary>
    /// A class which holds the names of all the cards the player has unlocked, as well as information about their current decks.
    /// </summary>
    public class PlayerCardRegistryData : CardRegistryData
    {
        /// <summary>
        /// A list of cards representing a user deck
        /// </summary>
        [XmlArrayItem(ElementName = "DeckData")]
        public List<DeckData> Decks { get; set; }
    }
}
