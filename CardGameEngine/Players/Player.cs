using CardGameEngineData;
using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// A delegate for an event that will be fired when we draw cards
    /// </summary>
    /// <param name="drawnCard"></param>
    public delegate void OnCardDrawHandler(CardData drawnCard);

    /// <summary>
    /// A class used in the battle screen to control the logic of each player's during the battle
    /// </summary>
    public class Player
    {
        #region Properties and Fields

        /// <summary>
        /// An instance of the deck that we have chosen for this battle.
        /// Should not be exposed beyond this class as we can alter our deck otherwise.
        /// </summary>
        private DeckInstance ChosenDeck { get; set; }

        /// <summary>
        /// A list of the current cards that the player has in their hand
        /// </summary>
        public List<CardData> CurrentHand { get; private set; }

        /// <summary>
        /// An int representing the number of cards left in our deck
        /// </summary>
        public int CardsLeftInDeck { get { return ChosenDeck.Count; } }

        /// <summary>
        /// An event that is fired each time we draw a card.
        /// Is called after the card is removed from the deck.
        /// </summary>
        public event OnCardDrawHandler OnCardDraw;

        #endregion

        public Player(Deck chosenDeck)
        {
            ChosenDeck = new DeckInstance(chosenDeck);
            CurrentHand = new List<CardData>();
        }

        #region Utility Functions

        /// <summary>
        /// Draw cards from our deck instance
        /// </summary>
        /// <param name="numberOfCards"></param>
        public void Draw(int numberOfCards = 1)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                CardData cardData = ChosenDeck.Draw();
                CurrentHand.Add(cardData);

                if (OnCardDraw != null)
                {
                    OnCardDraw(cardData);
                }
            }
        }

        #endregion
    }
}
