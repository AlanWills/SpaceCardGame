using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    public enum ChargeType
    {
        kCharge,
        kRefund
    }

    /// <summary>
    /// A delegate for an event that will be fired when we draw cards
    /// </summary>
    /// <param name="drawnCard"></param>
    public delegate void OnCardDrawHandler(Card drawnCard);

    /// <summary>
    /// A delegate for an event that will be fired when we add cards to our hand
    /// </summary>
    /// <param name="drawnCard"></param>
    public delegate void OnCardAddedToHandHandler(Card drawnCard);

    public class Player
    {
        public delegate void NewTurnHandler(Player newActivePlayer);

        #region Properties and Fields

        /// <summary>
        /// The list of cards we have instantiated from our inputted Deck
        /// </summary>
        private DeckInstance Deck { get; set; }

        /// <summary>
        /// A list of the current cards that the player has in their hand
        /// </summary>
        public List<Card> CurrentHand { get; private set; }

        /// <summary>
        /// An int representing the number of cards left in our deck
        /// </summary>
        public int CardsLeftInDeck { get { return Deck.Count; } }

        /// <summary>
        /// An array of resource card lists.
        /// Allows us to influence the UI by manipulating the resources.
        /// </summary>
        public List<ResourceCard>[] Resources { get; private set; }

        /// <summary>
        /// A cap to limit the number of resource cards we can lay
        /// </summary>
        public int ResourceCardsPlacedThisTurn { get; set; }

        /// <summary>
        /// A cap to limit the number of ships the player can have placed at any one time
        /// </summary>
        public int CurrentShipsPlaced { get; set; }

        /// <summary>
        /// An event which will be fired when we begin a new turn
        /// </summary>
        public event NewTurnHandler OnNewTurn;

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

        /// <summary>
        /// The number of cards our player will draw at the start of their turn.
        /// By default set to 3.
        /// </summary>
        public int CardsToDrawPerTurn { get; set; }

        public const int ResourceCardsCanLay = 10;
        public const int MaxShipNumber = 6;

        #endregion

        public Player(Deck chosenDeck)
        {
            ResourceCardsPlacedThisTurn = 0;
            CurrentShipsPlaced = 0;
            MaxHandSize = 10;
            CardsToDrawPerTurn = 3;

            // Set up our resources array
            Resources = new List<ResourceCard>[(int)ResourceType.kNumResourceTypes]
            {
                new List<ResourceCard>(),
                new List<ResourceCard>(),
                new List<ResourceCard>(),
                new List<ResourceCard>()
            };

            CurrentHand = new List<Card>();
            Deck = new DeckInstance(this, chosenDeck);
        }

        #region Virtual Functions

        /// <summary>
        /// Calls our new turn event handler.
        /// Called after our CurrentActivePlayer is updated in our screen.
        /// </summary>
        public void NewTurn()
        {
            // Draw a card - in the draw method we will add it to our hand if we can
            for (int i = 0; i < CardsToDrawPerTurn; i++)
            {
                if (CardsLeftInDeck > 0)
                {
                    DrawCard();
                }
            }

            ResourceCardsPlacedThisTurn = 0;

            // Refresh all our resources so they are all available for use now
            for (ResourceType resourceIndex = ResourceType.Crew; resourceIndex != ResourceType.kNumResourceTypes; resourceIndex++)
            {
                foreach (ResourceCard card in Resources[(int)resourceIndex])
                {
                    card.Used = false;
                }
            }

            OnNewTurn?.Invoke(this);
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
                Card card = Deck.Draw();
                TriggerDrawCardEvents(card);
            }
        }

        /// <summary>
        /// Fires the events for card draw
        /// </summary>
        /// <param name="cardData"></param>
        protected void TriggerDrawCardEvents(Card card)
        {
            OnCardDraw?.Invoke(card);

            if (CurrentHand.Count < MaxHandSize)
            {
                AddCardToHand(card);
            }
        }

        /// <summary>
        /// Adds the card to the player's hand and triggers the OnCardAddedToHand event
        /// </summary>
        /// <param name="cardData"></param>
        public void AddCardToHand(Card card)
        {
            CurrentHand.Add(card);
            OnCardAddedToHand?.Invoke(card);
        }

        /// <summary>
        /// Removes the card from the player's hand
        /// </summary>
        /// <param name="cardData"></param>
        public void RemoveCardFromHand(Card card)
        {
            CurrentHand.Remove(card);
        }

        /// <summary>
        /// Draws a card from the deck of a certain type - useful for the beginning of the game to enforce certain cards appear in the player's hand.
        /// Also useful in certain abilities which add a card type to your hand.
        /// For example, to add a MissileBarrageAbilityCard to your hand, use DrawCard("MissileBarrageAbilityCard").
        /// </summary>
        /// <param name="resourceType"></param>
        public void DrawCard(string cardTypeName)
        {
            Debug.Assert(Deck.Exists(x => x.GetType().Name == cardTypeName));
            Card card = Deck.Find(x => x.GetType().Name == cardTypeName);
            Deck.Remove(card);

            TriggerDrawCardEvents(card);
        }

        /// <summary>
        /// A function which obtains the data for the station in the player's deck and removes it from the deck so that we cannot draw it.
        /// Used at the start of the game to get the station data so we can add it to our board straightaway.
        /// </summary>
        /// <returns></returns>
        public StationCard GetStationData()
        {
            // This assert should NEVER trigger.  If it does, it means that the player has no station chosen.
            // This quite simply CANNOT happen otherwise we have no basis for a game!
            Debug.Assert(Deck.Exists(x => x is StationCard));
            StationCard station = Deck.Find(x => x is StationCard) as StationCard;
            Deck.Remove(station);

            return station;
        }

        /// <summary>
        /// A function which works out whether we have enough resources to lay the inputted card data.
        /// Outputs an error message based on what resource was lacking.
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HaveSufficientResources(Card card, ref string error)
        {
            for (int i = 0; i < (int)ResourceType.kNumResourceTypes; i++)
            {
                if (Resources[i].FindAll(x => !x.Used).Count < card.CardData.ResourceCosts[i])
                {
                    // We do not have enough of the current resource we are analysing to lay this card so return false
                    error = "Insufficient " + Enum.GetNames(typeof(ResourceType))[i];
                    return false;
                }
            }

            // We have enough of each resource type for this card
            return true;
        }

        /// <summary>
        /// Alters the current resources by the amount specified in the card's card data.
        /// Either charges them or refunds them based on the input enum
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="charge"></param>
        public void AlterResources(Card card, ChargeType charge)
        {
            for (int typeIndex = 0; typeIndex < (int)ResourceType.kNumResourceTypes; typeIndex++)
            {
                AlterResources((ResourceType)typeIndex, card.CardData.ResourceCosts[typeIndex], charge);
            }
        }

        /// <summary>
        /// Alters the current resources of the inputted type by the inputted amount.
        /// Either charges them or refunds them based on the input enum
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="charge"></param>
        public void AlterResources(ResourceType resourceType, int amount, ChargeType charge)
        {
            int typeIndex = (int)resourceType;
            int numAvailableResources = Resources[typeIndex].Count;

            Debug.Assert(numAvailableResources >= amount);

            for (int i = 0; i < amount; ++i)
            {
                // If the 'charge' enum is set to kRefund, we are refunding so the resource should not be used
                // If the 'charge' enum is set to kCharge, we are charging the player the resources, so they should be used
                Resources[typeIndex][i].Used = charge == ChargeType.kCharge ? true : false;
            }
        }

        #endregion
    }
}
