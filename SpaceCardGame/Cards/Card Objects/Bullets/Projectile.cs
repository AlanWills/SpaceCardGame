using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceCardGameData;

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
        /// A reference to our bullet data
        /// </summary>
        private ProjectileData ProjectileData { get; set; }

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
            ProjectileData = projectileData;

            float angle = MathUtils.AngleBetweenPoints(worldPosition, target.WorldPosition);
            LocalRotation = angle;

            DebugUtils.AssertNotNull(ProjectileData);
        }

        #region Virtual Functions

        /// <summary>
        /// Return our loaded bullet data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return ProjectileData;
        }

        /// <summary>
        /// Get our firing sfx
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            FiringSFX = new CustomSoundEffect(ProjectileData.FiringSFXAsset);
            ExplosionSFX = new CustomSoundEffect(ProjectileData.ExplosionSFXAsset);

            base.LoadContent();
        }

        #endregion
    }
}
