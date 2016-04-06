using _2DEngineData;
using System.Collections.Generic;
using System.Xml.Serialization;

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
        [XmlArrayItem(ElementName = "Resource")]
        public List<int> ResourceCosts { get; set; }

        #endregion

        #region Virtual Functions

        /// <summary>
        /// A pure abstract function that we will override with specific implementations for each card type.
        /// A function which can be used to determine whether the player can lay a card - could be used for resources, limiting a specific number of cards per turn etc.
        /// </summary>
        /// <param name="player">The player attempting to lay the card</param>
        /// <param name="error">An error string which is returned for displaying error UI</param>
        /// <returns></returns>
        public abstract bool CanLay(Player player, ref string error);

        #endregion
    }
}