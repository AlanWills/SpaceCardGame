using CelesteEngine;
using SpaceCardGameData;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which is used in the battle screen and is created from a Deck.
    /// Rather than holding CardData it creates all the cards from the CardData
    /// </summary>
    public class DeckInstance : List<Card>
    {
        #region Properties and Fields

        /// <summary>
        /// The underlying deck that this instance is made from
        /// </summary>
        public Deck Deck { get; private set; }

        #endregion

        public DeckInstance(Player player, Deck chosenDeck) :
            base(chosenDeck.Cards.Count)
        {
            Deck = chosenDeck;

            foreach (CardData cardData in chosenDeck.Cards.FindAll(x => x is CardData))
            {
                Add(cardData.CreateCard(player));
            }
        }

        #region Utility Functions

        /// <summary>
        /// Remove a card from this deck
        /// </summary>
        public Card Draw()
        {
            Debug.Assert(Count > 0);
            int number = MathUtils.GenerateInt(0, Count - 1);

            Card card = this[number];
            RemoveAt(number);

            return card;
        }

        #endregion
    }
}
