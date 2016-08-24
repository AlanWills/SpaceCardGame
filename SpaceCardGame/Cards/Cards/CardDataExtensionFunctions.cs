using CelesteEngine;
using SpaceCardGameData;
using System;

namespace SpaceCardGame
{
    public static class CardDataExtensionFunctions
    {
        #region Virtual Functions

        /// <summary>
        /// Creates the appropriate Card from this card data.
        /// Calls LoadContent and Initialise().
        /// </summary>
        /// <returns></returns>
        public static Card CreateCard(this CardData data, Player player)
        {
            // Remove all the whitespace from the display name
            string squashedDisplayName = data.DisplayName.Replace(" ", "");

            // Use the squashed display name to create the card itself - assumes a lot about naming, but ok for now
            Type cardType = typeof(CardData).Assembly.GetType("SpaceCardGame." + data.CardTypeName);
            DebugUtils.AssertNotNull(cardType);

            Card card = (Card)Activator.CreateInstance(cardType, player, data);
            card.LoadContent();
            card.Initialise();

            return card;
        }

        #endregion
    }
}
