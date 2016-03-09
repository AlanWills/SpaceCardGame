using _2DEngine;
using CardGameEngineData;
using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// A class representing our current usable deck - cards chosen from the player's registry
    /// </summary>
    public class Deck : Component
    {
        /// <summary>
        /// A list of our deck's card data
        /// </summary>
        public List<CardData> CardData { get; private set; }

        public Deck()
        {

        }
    }
}
