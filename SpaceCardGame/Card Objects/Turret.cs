using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents a weapon on our ship.
    /// Handles firing bullets and damaging ships.
    /// </summary>
    public class Turret : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the turret data
        /// </summary>
        private TurretData TurretData { get; set; }

        /// <summary>
        /// Our bullet data - only need one copy for the bullets we spawn
        /// </summary>
        private BulletData BulletData { get; set; }

        /// <summary>
        /// The amount of damage our turret does per bullet
        /// </summary>
        private float Damage { get; set; }

        // A string which represents the default turret all ships have 
        private const string defaultTurretDataAsset = "Content\\Data\\Cards\\Weapons\\DefaultTurret.xml";

        #endregion

        // Constructor used for creating a custom turret from a card
        public Turret(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            UsesCollider = false;
        }

        // Constructor used for creating a default turret for each ship
        public Turret(float turretAttack, Vector2 localPosition) :
            this(localPosition, defaultTurretDataAsset)
        {
            Damage = turretAttack;
        }

        #region Properties and Fields

        /// <summary>
        /// Loads our turret data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.LoadData<TurretData>(DataAsset);
        }

        /// <summary>
        /// Sets up the turret's stats and load the bullet data
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            TurretData = Data as TurretData;

            BulletData = AssetManager.LoadData<BulletData>(TurretData.BulletDataAsset);
            DebugUtils.AssertNotNull(BulletData);

            // If we're created a default turret we need to set the bullet damage to be our ship's damage which is passed in via the constructor
            if (DataAsset == defaultTurretDataAsset)
            {
                BulletData.Damage = Damage;
            }

            base.LoadContent();
        }

        #endregion

        #region Firing Functions

        /// <summary>
        /// Fires a bullet from our turret at the inputted target
        /// </summary>
        /// <param name="target"></param>
        public void Fire(GameObject target)
        {
            DebugUtils.AssertNotNull(BulletData);
            // Add to current screen so that the bullets are drawn over everything
            Bullet bullet = ScreenManager.Instance.CurrentScreen.AddGameObject(new Bullet(target, WorldPosition, BulletData), true, true);
            bullet.SetParent(null);
        }

        #endregion
    }
}
