using _2DEngine;
using CardGameEngineData;
using System.Collections.Generic;
using System.Diagnostics;

namespace CardGameEngine
{
    /// <summary>
    /// A class used in our battle screen as a list initially starting off the same as our deck, but we can add and remove elements.
    /// Used for the gameplay rather than as a long term means of storage.
    /// </summary>
    public class DeckInstance : List<CardData>
    {
        public DeckInstance(Deck deck) :
            base(deck.Cards.Capacity)
        {
            AddRange(deck.Cards);
        }

        #region Utility Functions

        /// <summary>
        /// Remove a card from this deck
        /// </summary>
        public CardData Draw()
        {
            Debug.Assert(Count > 0);
            int number = MathUtils.GenerateInt(0, Count - 1);

            CardData card = this[number];
            RemoveAt(number);

            return card;
        }

        #endregion
    }
}
