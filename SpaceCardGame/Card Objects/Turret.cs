using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;
using System;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents a weapon on our ship.
    /// Handles firing bullets and damaging ships.
    /// </summary>
    public class Turret : ShipAddOn
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

        /// <summary>
        /// The number of shots with this turret we have left
        /// </summary>
        public int ShotsLeft { get; private set; }

        /// <summary>
        /// A flag to indicate whether this is a default turret or a custom one
        /// </summary>
        private bool DefaultTurret { get; set; }

        // A string which represents the default turret all ships have 
        private const string defaultTurretDataAsset = "Content\\Data\\Cards\\Weapons\\DefaultTurret.xml";

        #endregion

        // Constructor used for creating a custom turret from a card
        public Turret(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            UsesCollider = false;
            DefaultTurret = false;
        }

        // Constructor used for creating a default turret for each ship
        public Turret(float turretAttack, Vector2 localPosition) :
            this(localPosition, defaultTurretDataAsset)
        {
            Damage = turretAttack;
            DefaultTurret = true;
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
            ShotsLeft = TurretData.Shots;

            BulletData = AssetManager.LoadData<BulletData>(TurretData.BulletDataAsset);
            DebugUtils.AssertNotNull(BulletData);

            // If we're created a default turret we need to set the bullet damage to be our ship's damage which is passed in via the constructor
            if (DefaultTurret)
            {
                BulletData.Damage = Damage;
                ShotsLeft = (int)Damage;     // We fire as many shots with our default turret as our attack
            }

            base.LoadContent();
        }

        /// <summary>
        /// Adds a turret to the inputted ship
        /// </summary>
        /// <param name="ship"></param>
        public override void AddToShip(Ship ship)
        {
            Debug.Fail("TODO");
        }

        #endregion

        #region Firing Functions

        /// <summary>
        /// Spawns a bullet from our turret at the inputted target
        /// </summary>
        /// <param name="target"></param>
        public void Attack(GameObject target)
        {
            DebugUtils.AssertNotNull(BulletData);
            Debug.Assert(target is IDamageable);        // Need to make sure our target is a damageable object
            Debug.Assert((target as IDamageable).Health > 0);

            (target as IDamageable).Damage(BulletData.Damage);

            if (DefaultTurret)
            {
                // Add script to space out bullet firings if we are using a Default turret -  we fire as many bullets as our attack cos why not
                for (int i = 0; i < BulletData.Damage; i++)
                {
                    SpawnBullet(target);
                }
            }
            else
            {
                SpawnBullet(target);
            }
        }

        /// <summary>
        /// Spawns a bullet which will travel to the inputted target game object.
        /// </summary>
        /// <param name="target"></param>
        private void SpawnBullet(GameObject target)
        {
            // Add to current screen so that the bullets are drawn over everything
            Bullet bullet = ScreenManager.Instance.CurrentScreen.AddGameObject(new Bullet(target, WorldPosition, BulletData), true, true);
            bullet.LocalRotation = WorldRotation;
            bullet.SetParent(null);

            ShotsLeft--;

            if (ShotsLeft == 0)
            {
                // If we are out of shots then we reset the colour to be white
                Colour = Color.White;
            }
        }

        #endregion
    }
}
