using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// A delegate for an event that will be fired when we draw cards
    /// </summary>
    /// <param name="drawnCard"></param>
    public delegate void OnCardDrawHandler(CardData drawnCard);

    /// <summary>
    /// A delegate for an event that will be fired when we add cards to our hand
    /// </summary>
    /// <param name="drawnCard"></param>
    public delegate void OnCardAddedToHandHandler(CardData drawnCard);

    /// <summary>
    /// A class used in the battle screen to control the logic of each player's during the battle
    /// </summary>
    public abstract class Player
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

        /// <summary>
        /// An event that is fired each time we add a card to our hand.
        /// Is called after the card is added to our hand.
        /// </summary>
        public event OnCardAddedToHandHandler OnCardAddedToHand;

        /// <summary>
        /// The maximum number of cards allowed in our hand.
        /// </summary>
        public int MaxHandSize { get; private set; }

        #endregion

        public Player(Deck chosenDeck)
        {
            ChosenDeck = new DeckInstance(chosenDeck);
            CurrentHand = new List<CardData>();
            MaxHandSize = 10;
        }

        #region Virtual Functions

        /// <summary>
        /// The behaviour to perform when we start a new turn
        /// </summary>
        public virtual void NewTurn()
        {
            // Draw a card - in the draw method we will add it to our hand if we can
            if (CardsLeftInDeck > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    DrawCard();
                }
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Draw cards from our deck instance if we have room in our hand
        /// </summary>
        /// <param name="numberOfCards"></param>
        public void DrawCard(int numberOfCards = 1)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                CardData cardData = ChosenDeck.Draw();
                if (OnCardDraw != null)
                {
                    OnCardDraw(cardData);
                }

                if (CurrentHand.Count < MaxHandSize)
                {
                    AddCardToHand(cardData);
                }
            }
        }

        /// <summary>
        /// Adds the card data to the player's hand and triggers the OnCardAddedToHand event
        /// </summary>
        /// <param name="cardData"></param>
        public void AddCardToHand(CardData cardData)
        {
            CurrentHand.Add(cardData);
            if (OnCardAddedToHand != null)
            {
                OnCardAddedToHand(cardData);
            }
        }

        #endregion
    }
}
