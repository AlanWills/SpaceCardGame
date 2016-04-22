using _2DEngine;
using CardGameEngine;
using System;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper around the CardData for specific use in our game.
    /// Provides extra functionality for creating specific objects etc.
    /// </summary>
    public abstract class GameCardData : CardData
    {
        #region Virtual Functions

        /// <summary>
        /// A pure abstract function that we will override with specific implementations for each card type.
        /// A function which can be used to determine whether the player can lay a card - could be used for resources, limiting a specific number of cards per turn etc.
        /// </summary>
        /// <param name="player">The player attempting to lay the card</param>
        /// <param name="error">An error string which is returned for displaying error UI</param>
        /// <returns></returns>
        public abstract bool CanLay(Player player, ref string error);

        /// <summary>
        /// Creates a card object pair using this card data
        /// </summary>
        /// <returns></returns>
        public CardObjectPair CreateCardObjectPair()
        {
            Type cardObjectPairType = typeof(GameCardData).Assembly.GetType("SpaceCardGame.Card" + Type + "Pair");
            DebugUtils.AssertNotNull(cardObjectPairType);

            return (CardObjectPair)Activator.CreateInstance(cardObjectPairType, this);
        }

        /// <summary>
        /// Creates the appropriate GameCard from this card data
        /// </summary>
        /// <returns></returns>
        public GameCard CreateCard()
        {
            // Remove all the whitespace from the display name
            string squashedDisplayName = DisplayName.Replace(" ", "");

            // Use the squashed display name to create the card itself - assumes a lot about naming, but ok for now
            Type cardType = typeof(GameCardData).Assembly.GetType("SpaceCardGame." + squashedDisplayName + "Card");
            if (cardType == null)
            {
                // Try using the original type instead
                cardType = typeof(GameCardData).Assembly.GetType("SpaceCardGame." + Type + "Card");
                DebugUtils.AssertNotNull(cardType);
            }

            return (GameCard)Activator.CreateInstance(cardType, this);
        }

        #endregion
    }
}
