using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    public class Missile : Projectile
    {
        #region Properties and Fields

        bool finishedJutting;
        Vector2 jutFinishPosition;

        /// <summary>
        /// A reference to our Engine (used as fancy animation only)
        /// </summary>
        private Engine Engine { get; set; }

        #endregion

        public Missile(GameObject target, Vector2 worldPosition, ProjectileData projectileData) :
            base(target, worldPosition, projectileData)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Add an engine for our missile
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            int side = (MathUtils.GenerateInt(0, 1) * 2) - 1;
            jutFinishPosition = LocalPosition + Vector2.Transform(new Vector2(side * 50, 0), Matrix.CreateRotationZ(WorldRotation)); ;

            Engine = AddChild(new Engine(0, new Vector2(0, Size.Y * 0.5f)), true, true);
            Engine.Scale(new Vector2(0.2f, 1));
        }

        /// <summary>
        /// Updates our bullet's position and kill's it if it has collided with our target
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (!finishedJutting)
            {
                LocalPosition = Vector2.Lerp(LocalPosition, jutFinishPosition, 5 * elapsedGameTime);
                if ((LocalPosition - jutFinishPosition).LengthSquared() < 10)
                {
                    finishedJutting = true;
                    Engine.Show();      // Have to call show because by default our Engine is hidden (since it is a CardObject)
                }
            }
            else
            {
                Vector2 diff = Target.WorldPosition - WorldPosition;
                diff.Normalize();

                float angle = MathUtils.AngleBetweenPoints(WorldPosition, Target.WorldPosition);
                LocalRotation = angle;

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
        }
        
        #endregion
    }
}
