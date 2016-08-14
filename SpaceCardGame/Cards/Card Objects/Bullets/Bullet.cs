using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Fires a bullet which will not track the target, but instead just move in a straight line under the influence of the rigid body module
    /// </summary>
    public class Bullet : Projectile
    {
        #region Properties and Fields

        /// <summary>
        /// The rigidbody responsible for controlling the movement of this bullet
        /// </summary>
        private RigidBodyModule RigidBody { get; set; }

        public const string defaultBulletDataAsset = "Cards\\Weapons\\DefaultBullet.xml";

        #endregion

        public Bullet(GameObject target, Vector2 worldPosition, ProjectileData projectileData) :
            base(target, worldPosition, projectileData)
        {
            RigidBody = AddModule(new RigidBodyModule(new Vector2(0, 1000)));
        }

        #region Virtual Functions

        /// <summary>
        /// Play our firing sound effect
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            FiringSFX.Play();
        }

        /// <summary>
        /// Updates our bullet's position and kill's it if it has collided with our target
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // This bullet may have been killed by a lifetime module or something, so do not register the collision if this is the case - the bullet has died!
            if (!IsAlive) { return; }

            DebugUtils.AssertNotNull(Collider);
            DebugUtils.AssertNotNull(Target.Collider);
            if (Collider.CheckCollisionWith(Target.Collider))
            {
                // Kills the projectile if it has collided with the target
                Die();

                ExplosionSFX.Play();
                Target.OnCollisionWith(this);
            }
        }

        #endregion
    }
}