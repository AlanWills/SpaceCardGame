using System;
using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// Represents a Shield on our ship
    /// </summary>
    public class Shield : ShipAddOn
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the shield data for this shield
        /// </summary>
        private DefenceCardData ShieldData { get; set; }

        #endregion

        // A constructor used for creating a custom shield from a card
        public Shield(DefenceCardData shieldData) :
            base(Vector2.Zero, "")
        {
            UsesCollider = true;

            DebugUtils.AssertNotNull(shieldData);
            ShieldData = shieldData;
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the shield object data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return ShieldData;
        }

        /// <summary>
        /// Loads the shield data and sets up it's stats
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            ShieldData = Data as DefenceCardData;
            DebugUtils.AssertNotNull(ShieldData);

            base.LoadContent();
        }

        /// <summary>
        /// Add a circle collider for this shield
        /// </summary>
        /// <returns></returns>
        protected override Collider AddCollider()
        {
            // Add a circle collider for this
            return base.AddCollider();
        }

        /// <summary>
        /// Adds a shield to the inputted ship
        /// </summary>
        /// <param name="ship"></param>
        public override void AddToShip(Ship ship)
        {
            Debug.Fail("");
        }

        #endregion
    }
}
