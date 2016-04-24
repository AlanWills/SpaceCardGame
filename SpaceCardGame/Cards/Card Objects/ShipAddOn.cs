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
        /// Kills our parent which will kill us and the card we are attached too
        /// </summary>
        public override void Die()
        {
            // Make sure we call Die so that when our parent calls Die on us again, we will already be dead and not have this function called again
            base.Die();

            DebugUtils.AssertNotNull(Parent);
            Parent.Die();
        }

        #endregion
    }
}