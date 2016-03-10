using _2DEngine;
using CardGameEngineData;
using System.Collections;
using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// A class representing our current usable deck - cards chosen from the player's registry
    /// </summary>
    public class Deck : List<CardData>
    {
        /// <summary>
        /// The name of the deck
        /// </summary>
        public string Name { get; set; }

        public Deck()
        {

        }
    }
}
