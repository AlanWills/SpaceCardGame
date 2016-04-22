using CardGameEngine;

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
        /// Creates a specific card for use in a general CardObjectPair wrapper
        /// </summary>
        /// <returns></returns>
        //protected abstract GameCard CreateCard();

        /// <summary>
        /// Creates a card object pair using this card data
        /// </summary>
        /// <returns></returns>
        public abstract CardObjectPair CreateCardObjectPair();

        #endregion
    }
}
