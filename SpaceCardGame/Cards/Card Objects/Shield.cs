using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

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
        private ShieldData ShieldData { get; set; }

        /// <summary>
        /// A reference to our damage module - used elsewhere to know our health etc.
        /// </summary>
        public DamageableObjectModule DamageModule { get; private set; }

        /// <summary>
        /// A reference to our flashing module - used as a cool battle effect
        /// </summary>
        public FlashingObjectModule FlashingModule { get; private set; }

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

            DamageModule = AddModule(new DamageableObjectModule(ShieldData.MaxShieldStrength));
            FlashingModule = AddModule(new FlashingObjectModule(0.15f, 1, 1));

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

        #region Utility Functions

        /// <summary>
        /// Recharges our Shield by the amount in our ShieldData.
        /// Clamped to the max health value.
        /// </summary>
        public void Recharge()
        {
            // Works out how much shield strength we can recharge so that our shield does not exceed it's maximum
            float amountToHeal = MathHelper.Clamp(DamageModule.Health + ShieldData.ShieldRechargePerTurn, 0, ShieldData.MaxShieldStrength);

            // Heal the shield by damaging a negative amount
            DamageModule.Damage(-ShieldData.ShieldRechargePerTurn);
        }

        #endregion
    }
}
