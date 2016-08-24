using CelesteEngineData;
using System;
using System.Collections.Generic;

namespace SpaceCardGameData
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

        /// <summary>
        /// How much this card costs to buy from the shop
        /// </summary>
        public int Price { get; set; }

        #endregion
    }
}
