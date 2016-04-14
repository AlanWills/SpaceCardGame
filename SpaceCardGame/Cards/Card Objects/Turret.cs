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
        private int ShotsLeft { get; set; }

        /// <summary>
        /// A read only property which returns the conditions necessary for this turret to be able to fire
        /// </summary>
        public bool CanFire
        {
            get
            {
                // Make sure we have shots left and our ship is ready
                DebugUtils.AssertNotNull(CardShipPair);
                return ShotsLeft > 0 && CardShipPair.IsReady;
            }
        }

        /// <summary>
        /// A flag to indicate whether this is a default turret or a custom one
        /// </summary>
        private bool DefaultTurret { get; set; }

        /// <summary>
        /// A reference to the card ship pair that this turret is equipped to - for convenience purposes.
        /// </summary>
        private CardShipPair cardShipPair;
        private CardShipPair CardShipPair
        {
            get
            {
                // We perform this rather odd setup, because our weapon card is initially added to a card container rather than a ship whilst we look for ships to lay it on.
                // Initialise this the first time it is called - it won't be called until it is parented under a ship
                if (cardShipPair == null)
                {
                    // Get the ship this turret is parented under - it should be not null by this point
                    Debug.Assert(Parent is CardWeaponPair);
                    Debug.Assert(Parent.Parent is CardShipPair);
                    cardShipPair = Parent.Parent as CardShipPair;
                }

                DebugUtils.AssertNotNull(cardShipPair);
                return cardShipPair;
            }
            set { cardShipPair = value; }
        }

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
                BulletData.Damage = CardShipPair.Ship.ShipData.Attack;
                TurretData.ShotsPerTurn = (int)BulletData.Damage;      // We fire as many shots with our default turret as our attack
            }

            ShotsLeft = TurretData.ShotsPerTurn;

            // Connect the turret's colour to the ship - this is for highlighting purposes
            Colour.Connect(CardShipPair.Ship.Colour);
            Colour.ComputeFunction += ColourComputeFunction;        // Our special compute function for our turret colour

            base.LoadContent();
        }

        /// <summary>
        /// We wish to enforce that only bullets are added as children.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="childToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override K AddChild<K>(K childToAdd, bool load = false, bool initialise = false)
        {
            Debug.Assert(childToAdd is Bullet);

            return base.AddChild(childToAdd, load, initialise);
        }

        #endregion

        #region Firing Functions

        /// <summary>
        /// Spawns a bullet from our turret at the inputted ship, but switches target if the ship has appropriate defences.
        /// </summary>
        /// <param name="target"></param>
        public void Attack(Ship targetShip)
        {
            // Shouldn't be firing unless we have shots left
            Debug.Assert(CanFire);
            DebugUtils.AssertNotNull(BulletData);

            DamageableObjectModule shipDamagebaleModule = targetShip.FindModule<DamageableObjectModule>();       // Need to make sure our target has a damageable module
            DebugUtils.AssertNotNull(shipDamagebaleModule);
            Debug.Assert(!shipDamagebaleModule.Dead);

            // Initially select our ship as the target
            DamageableObjectModule targetModule = shipDamagebaleModule;

            // However, if we have a shield that is still alive, target that
            if (targetShip.Shield != null)
            {
                DamageableObjectModule shieldDamageableModule = targetShip.Shield.FindModule<DamageableObjectModule>();
                DebugUtils.AssertNotNull(shieldDamageableModule);

                if (!shieldDamageableModule.Dead)
                {
                    targetModule = shieldDamageableModule;
                }
            }

            // Damage our target
            targetModule.Damage(BulletData.Damage);

            // Spawn bullet(s) at our target
            if (DefaultTurret)
            {
                // Add script to space out bullet firings if we are using a Default turret -  we fire as many bullets as our attack cos why not
                for (int i = 0; i < BulletData.Damage; i++)
                {
                    Debug.Assert(targetModule.AttachedBaseObject is BaseObject);
                    SpawnBullet(targetModule.AttachedBaseObject as GameObject);
                }
            }
            else
            {
                Debug.Assert(targetModule.AttachedBaseObject is BaseObject);
                SpawnBullet(targetModule.AttachedBaseObject as GameObject);
            }

            // Need to stop this firing all shots
            if (targetShip.Turret.CanFire)
            {
                targetShip.Turret.Attack(CardShipPair.Ship);
            }
        }

        /// <summary>
        /// Spawns a bullet which will travel to the inputted target game object.
        /// </summary>
        /// <param name="target"></param>
        private void SpawnBullet(GameObject target)
        {
            RotateToTarget(target.WorldPosition);

            // Add to current screen so that the bullets are drawn over everything
            Bullet bullet = ScreenManager.Instance.CurrentScreen.AddGameObject(new Bullet(target, WorldPosition, BulletData), true, true);
            bullet.LocalRotation = WorldRotation;

            ShotsLeft--;

            if (ShotsLeft == 0)
            {
                // If we are out of shots then we reset the colour to be white
                Colour.Value = Color.White;
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Sets the rotation of the turret to be so that it is pointing at the inputted world space position.
        /// </summary>
        /// <param name="worldSpaceTarget"></param>
        public void RotateToTarget(Vector2 worldSpaceTarget)
        {
            float desiredAngle = MathUtils.AngleBetweenPoints(WorldPosition, worldSpaceTarget);

            DebugUtils.AssertNotNull(Parent);
            LocalRotation = desiredAngle - Parent.WorldRotation;
        }

        /// <summary>
        /// Reloads our turret's shots to the max value
        /// </summary>
        public void Reload()
        {
            ShotsLeft = TurretData.ShotsPerTurn;
        }

        #endregion

        #region Property Compute Functions

        /// <summary>
        /// Our compute function we will use to work out the colour of the turret.
        /// Return white if we have no shots left or our parent ship is not ready
        /// </summary>
        /// <param name="parentProperty"></param>
        private Color ColourComputeFunction(Color parentColour)
        {
            if (CanFire)
            {
                return Color.LightGreen;
            }
            else
            {
                return parentColour;
            }
        }

        #endregion
    }
}
