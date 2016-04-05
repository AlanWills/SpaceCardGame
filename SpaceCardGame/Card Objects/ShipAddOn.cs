using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class to represent any extra objects we can add to a ship
    /// </summary>
    public abstract class ShipAddOn : GameObject
    {
        public ShipAddOn(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Adds this component to the inputted ship
        /// </summary>
        /// <param name="ship"></param>
        public abstract void AddToShip(Ship ship);

        #endregion
    }
}