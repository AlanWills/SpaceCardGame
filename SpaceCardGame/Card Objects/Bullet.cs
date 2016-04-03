using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    public class Bullet : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the target we are firing the bullet at
        /// </summary>
        private GameObject Target { get; set; }

        /// <summary>
        /// A reference to our bullet data
        /// </summary>
        private BulletData BulletData { get; set; }

        #endregion

        public Bullet(GameObject target, Vector2 worldPosition, BulletData bulletData) :
            base(worldPosition, "")
        {
            Target = target;
            BulletData = bulletData;

            DebugUtils.AssertNotNull(BulletData);
        }

        #region Virtual Functions

        /// <summary>
        /// Return our loaded bullet data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return BulletData;
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
                // Kills the bullet if it has collided with the target
                Die();

                // Adds an explosion
                ScreenManager.Instance.CurrentScreen.AddInGameUIObject(new Explosion(WorldPosition), true, true);

                // Kill the target if we marked it as dead during our initial damage step
                Debug.Assert(Target is IDamageable);
                if ((Target as IDamageable).Dead)
                {
                    Target.Die();
                }
            }
        }

        #endregion
    }
}
