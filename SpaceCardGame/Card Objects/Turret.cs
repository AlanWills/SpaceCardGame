using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

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

        /// <summary>
        /// Constructor used for creating a turret from a card
        /// </summary>
        /// <param name="weaponCardData"></param>
        public Turret(string turretDataAsset) :
            base(Vector2.Zero, turretDataAsset)
        {
            DefaultTurret = turretDataAsset == defaultTurretDataAsset;
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

            // If we're created a default turret we need to set the bullet damage to be our ship's damage which we get via the hierarchy
            if (DefaultTurret)
            {
                DebugUtils.AssertNotNull(Parent);
                DebugUtils.AssertNotNull(Parent.Parent);
                Debug.Assert(Parent.Parent is CardShipPair);
                CardShipPair shipPair = Parent.Parent as CardShipPair;
                
                DebugUtils.AssertNotNull(shipPair.Ship);

                BulletData.Damage = shipPair.Ship.ShipData.Attack;
                TurretData.ShotsPerTurn = (int)BulletData.Damage;      // We fire as many shots with our default turret as our attack
            }

            ShotsLeft = TurretData.ShotsPerTurn;

            base.LoadContent();
        }

        #endregion

        #region Firing Functions

        /// <summary>
        /// Spawns a bullet from our turret at the inputted ship, but switches target if the ship has appropriate defences.
        /// </summary>
        /// <param name="target"></param>
        public void Attack(Ship targetShip)
        {
            DebugUtils.AssertNotNull(BulletData);
            Debug.Assert(targetShip is IDamageable);        // Need to make sure our target is a damageable object
            Debug.Assert(!(targetShip as IDamageable).Dead);
            
            // Initially select our ship as the target
            GameObject target = targetShip;

            // However, if we have a shield that is still alive, target that
            if (targetShip.Shield != null)
            {
                Debug.Assert(targetShip.Shield is IDamageable);
                if (!(targetShip.Shield as IDamageable).Dead)
                {
                    target = targetShip.Shield;
                }
            }

            // Damage our target
            (target as IDamageable).Damage(BulletData.Damage);

            // Spawn bullet(s) at our target
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

            ShotsLeft--;

            if (ShotsLeft == 0)
            {
                // If we are out of shots then we reset the colour to be white
                Colour = Color.White;
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Reloads our turret's shots to the max value
        /// </summary>
        public void Reload()
        {
            ShotsLeft = TurretData.ShotsPerTurn;
        }

        #endregion
    }
}
