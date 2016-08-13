using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Represents a Shield on our ship
    /// </summary>
    public class Shield : CardObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the shield data for this shield
        /// </summary>
        public ShieldData ShieldData { get; private set; }

        /// <summary>
        /// A reference to our damage module - used elsewhere to know our health etc.
        /// </summary>
        public DamageableObjectModule DamageModule { get; private set; }

        /// <summary>
        /// A reference to our flashing module - used as a cool battle effect
        /// </summary>
        public FlashingObjectModule FlashingModule { get; private set; }

        /// <summary>
        /// A reference to our shield explosion SFX that is triggered when this is hit by a bullet.
        /// </summary>
        private CustomSoundEffect ShieldExplosionSFX { get; set; }

        #endregion

        // A constructor used for creating a custom shield from a card
        public Shield(string shieldDataAsset) :
            base(Vector2.Zero, shieldDataAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the shield data and sets up it's stats
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            ShieldData = Data as ShieldData;
            DebugUtils.AssertNotNull(ShieldData);

            ShieldExplosionSFX = new CustomSoundEffect("Explosions\\ShieldExplosion");

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
        /// Recharges our ShieldStrength by the amount in our ShieldData
        /// </summary>
        public override void OnTurnBegin()
        {
            base.OnTurnBegin();

            Recharge();
        }

        /// <summary>
        /// If we collide with a bullet we reset our shield flashing - gives a flare.
        /// If we are dead we call Die.
        /// </summary>
        /// <param name="collidedObject"></param>
        public override void OnCollisionWith(GameObject collidedObject)
        {
            if (collidedObject is Projectile)
            {
                // Adds an explosion
                ScreenManager.Instance.CurrentScreen.AddInGameUIObject(new Explosion(WorldPosition), true, true);

                ShieldExplosionSFX.Play();
                FlashingModule.Reset();

                if (DamageModule.Dead)
                {
                    Die();
                }
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
            // Works out how much shield strength we can recharge so that our shield does not exceed it's maximum
            float amountToHeal = MathHelper.Clamp(DamageModule.Health + ShieldData.ShieldRechargePerTurn, 0, ShieldData.MaxShieldStrength);

            // Heal the shield by damaging a negative amount
            DamageModule.Damage(-ShieldData.ShieldRechargePerTurn, null);
        }

        #endregion
    }
}
