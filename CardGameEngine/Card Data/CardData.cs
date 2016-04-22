using _2DEngineData;
using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// Normally data classes are kept in an independant project.
    /// However, CardData is so ingrained in our game, rather than just as a container for information that it makes sense to add extra functionality to it.
    /// </summary>
    public abstract class CardData : GameObjectData
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

        #endregion
    }
}