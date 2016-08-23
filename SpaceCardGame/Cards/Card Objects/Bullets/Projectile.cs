using CelesteEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The base class for all elements fired from our turrets.
    /// </summary>
    public abstract class Projectile : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the target we are firing the bullet at
        /// </summary>
        protected GameObject Target { get; private set; }

        /// <summary>
        /// A reference to our firing sfx.
        /// </summary>
        protected CustomSoundEffect FiringSFX { get; private set; }

        /// <summary>
        /// A reference to our explosion SFX.
        /// </summary>
        protected CustomSoundEffect ExplosionSFX { get; private set; }

        #endregion

        public Projectile(GameObject target, Vector2 worldPosition, ProjectileData projectileData) :
            base(worldPosition, "")
        {
            Target = target;
            Data = projectileData;

            float angle = MathUtils.AngleBetweenPoints(worldPosition, target.WorldPosition);
            LocalRotation = angle;
        }

        #region Virtual Functions

        /// <summary>
        /// Get our firing sfx
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            Debug.Assert(Data is ProjectileData);
            ProjectileData projectileData = Data as ProjectileData;

            FiringSFX = new CustomSoundEffect(projectileData.FiringSFXAsset);
            ExplosionSFX = new CustomSoundEffect(projectileData.ExplosionSFXAsset);

            base.LoadContent();
        }

        #endregion
    }
}
