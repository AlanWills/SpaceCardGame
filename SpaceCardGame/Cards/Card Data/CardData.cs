using _2DEngine;
using _2DEngineData;
using System;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper around the CardData for specific use in our game.
    /// Provides extra functionality for creating specific objects etc.
    /// </summary>
    public class CardData : GameObjectData
    {
        #region Properties and Fields

        /// <summary>
        /// The object on the card's data asset - potentially empty if not used in game
        /// </summary>
        public string ObjectDataAsset { get; set; }

        /// <summary>
        /// The type of this card
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The name of the card (for UI purposes)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A string to indicate the rarity of this card
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// A list of the resources required to lay this card
        /// </summary>
        public List<int> ResourceCosts { get; set; }

        /// <summary>
        /// The string used reflectively to instantiate the correct card class in CreateCard
        /// </summary>
        public string CardTypeName { get; set; }

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Creates the appropriate GameCard from this card data
        /// </summary>
        /// <returns></returns>
        public Card CreateCard()
        {
            // Remove all the whitespace from the display name
            string squashedDisplayName = DisplayName.Replace(" ", "");

            // Use the squashed display name to create the card itself - assumes a lot about naming, but ok for now
            Type cardType = typeof(CardData).Assembly.GetType("SpaceCardGame." + CardTypeName);
            DebugUtils.AssertNotNull(cardType);

            return (Card)Activator.CreateInstance(cardType, this);
        }

        #endregion
    }
}
