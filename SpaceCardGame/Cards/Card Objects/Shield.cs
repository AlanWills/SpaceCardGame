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
    public class Shield : ShipAddOn, IDamageable
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the shield data for this shield
        /// </summary>
        private ShieldData ShieldData { get; set; }

        /// <summary>
        /// A float to represent the health of this ship
        /// </summary>
        private float health;
        public float Health
        {
            get { return health; }
            private set
            {
                health = value;
                HandleCurrentHealth();
            }
        }

        /// <summary>
        /// A bool to use to indicate that this object is dead, without having to change it's IsAlive property.
        /// </summary>
        private bool dead = false;
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        #endregion

        // A constructor used for creating a custom shield from a card
        public Shield(string shieldDataAsset) :
            base(Vector2.Zero, shieldDataAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the shield object data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.LoadData<ShieldData>(DataAsset);
        }

        /// <summary>
        /// Loads the shield data and sets up it's stats
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            ShieldData = Data as ShieldData;
            DebugUtils.AssertNotNull(ShieldData);

            Health = ShieldData.MaxShieldStrength;

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

        #region IDamageable Interface Implementation

        /// <summary>
        /// Damages the shield
        /// </summary>
        /// <param name="damage"></param>
        public void Damage(float damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// If the health of the shield goes to 0 then we kill it and it's parent
        /// </summary>
        public void HandleCurrentHealth()
        {
            if (Health <= 0)
            {
                DebugUtils.AssertNotNull(Parent);
                Dead = true;
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Recharges our Shield by the amount in our ShieldData.
        /// Clamped to the max health value.
        /// </summary>
        public void Recharge()
        {
            Health += ShieldData.ShieldRechargePerTurn;
            Health = MathHelper.Clamp(Health, 0, ShieldData.MaxShieldStrength);
        }

        #endregion
    }
}
