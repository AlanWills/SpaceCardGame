using CelesteEngine;
using CelesteEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents a weapon on our ship.
    /// Handles firing bullets and damaging ships.
    /// It's children can ONLY be bullets
    /// </summary>
    public abstract class Turret : CardObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the turret data
        /// </summary>
        public TurretData TurretData { get; private set; }

        /// <summary>
        /// Our bullet data - only need one copy for the bullets we spawn
        /// </summary>
        public ProjectileData BulletData { get; private set; }

        /// <summary>
        /// The number of shots with this turret we have left
        /// </summary>
        public int ShotsLeft { get; set; }

        /// <summary>
        /// A read only property which returns the conditions necessary for this turret to be able to fire
        /// </summary>
        public bool CanFire
        {
            get
            {
                // Make sure we have shots left and our ship is ready
                DebugUtils.AssertNotNull(CardShipPair);
                DebugUtils.AssertNotNull(BulletData);
                return CardShipPair.ShipCard.CalculateAttack(null) > 0 && ShotsLeft > 0 && CardShipPair.IsReady;
            }
        }

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

        /// <summary>
        /// Determines whether this is the default turret created for a ship.
        /// </summary>
        public bool IsDefaultTurret { get; private set; }

        // A string which represents the default turret all ships have 
        public const string DefaultTurretDataAsset = "Cards\\Weapons\\DefaultTurret.xml";

        #endregion

        /// <summary>
        /// Constructor used for creating a turret from a card
        /// </summary>
        /// <param name="weaponCardData"></param>
        public Turret(string turretDataAsset) :
            base(Vector2.Zero, turretDataAsset)
        {
            IsDefaultTurret = turretDataAsset == DefaultTurretDataAsset;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up the turret's stats and load the bullet data
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            TurretData = Data as TurretData;

            BulletData = AssetManager.GetData<ProjectileData>(TurretData.ProjectileDataAsset);
            DebugUtils.AssertNotNull(BulletData);

            ShotsLeft = TurretData.ShotsPerTurn;

            base.LoadContent();
        }

        /// <summary>
        /// Update the colour based on whether we can fire
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Parent.Colour = CanFire ? Color.Green : Color.Red;
        }

        /// <summary>
        /// Reloads our turret when we start our card placement turn again and changes our position to be to the side of the ship card
        /// </summary>
        public override void OnTurnBegin()
        {
            base.OnTurnBegin();

            Reload();
        }

        /// <summary>
        /// Move this turret so that it is directly over the ship
        /// </summary>
        public override void MakeReadyForBattle()
        {
            base.MakeReadyForBattle();

            LocalRotation = 0;
        }

        /// <summary>
        /// Reloads our turret ready for our opponent's battle phase.
        /// </summary>
        public override void OnTurnEnd()
        {
            base.OnTurnEnd();

            Reload();
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
        public void Attack(Ship targetShip, bool isCounterAttacking)
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
            targetModule.Damage(CardShipPair.ShipCard.CalculateAttack(targetShip.Parent), CardShipPair);

            // Spawn bullet(s) at our target
            Debug.Assert(targetModule.AttachedBaseObject is BaseObject);
            FireBullet(targetModule.AttachedBaseObject as GameObject);

            // Need to stop this firing all shots
            if (!isCounterAttacking && targetShip.Turret.CanFire)
            {
                targetShip.Turret.Attack(CardShipPair.Ship, true);
            }
        }

        /// <summary>
        /// Rotates to point towards the target and reduces the number of shots.
        /// Inherited classes will implement the creation of bullets.
        /// </summary>
        /// <param name="target"></param>
        private void FireBullet(GameObject target)
        {
            RotateToTarget(target.WorldPosition);

            CreateBullet(target);

            ShotsLeft--;
        }

        /// <summary>
        /// An abstract function our derived classes will implement to create bullets when we fire.
        /// </summary>
        protected abstract void CreateBullet(GameObject target);

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
    }
}
