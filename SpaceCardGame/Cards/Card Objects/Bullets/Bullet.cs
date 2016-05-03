using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    public class Bullet : Projectile
    {
        #region Properties and Fields

        public const string defaultBulletDataAsset = "Cards\\Weapons\\DefaultBullet.xml";

        #endregion

        public Bullet(GameObject target, Vector2 worldPosition, ProjectileData projectileData) :
            base(target, worldPosition, projectileData)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Play our firing sound effect
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            FiringSFX.Play(OptionsManager.SFXVolume, 0, 0);
        }

        /// <summary>
        /// Updates our bullet's position and kill's it if it has collided with our target
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Vector2 diff = Target.WorldPosition - WorldPosition;
            diff.Normalize();

            LocalPosition += diff * 700 * elapsedGameTime;

            DebugUtils.AssertNotNull(Collider);
            DebugUtils.AssertNotNull(Target.Collider);
            if (Collider.CheckCollisionWith(Target.Collider))
            {
                // Kills the projectile if it has collided with the target
                Die();

                Target.OnCollisionWith(this);
            }
        }

        #endregion
    }
}