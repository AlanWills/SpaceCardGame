using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
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

        #endregion
    }
}
