using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    public class Ship : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the data for the ship this object represents
        /// </summary>
        private ShipData ShipData { get; set; }

        #endregion

        // The ship is tied to the card, so it's position will be amended when the card is added to the screen
        public Ship(ShipCardData cardData) :
            base(Vector2.Zero, cardData.ObjectDataAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the ship object data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            ShipData = AssetManager.LoadData<ShipData>(DataAsset);
            return ShipData;
        }

        #endregion
    }
}
