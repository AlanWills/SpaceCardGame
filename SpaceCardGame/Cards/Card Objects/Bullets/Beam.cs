using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    public class Beam : Projectile
    {
        #region Properties and Fields

        /// <summary>
        /// Our beam will fade in and then out - this represents the current opacity of the beam
        /// </summary>
        private float CurrentOpacityLerp { get; set; }

        private bool fadeOut;

        #endregion

        public Beam(GameObject target, Vector2 worldPosition, ProjectileData projectileData) :
            base(target, worldPosition, projectileData)
        {
            Opacity = 0;
            Size = new Vector2(5, (target.WorldPosition - worldPosition).Length());
            LocalPosition = (target.WorldPosition + worldPosition) * 0.5f;
        }

        #region Virtual Functions

        /// <summary>
        /// Start playing the firing SFX.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            FiringSFX.Play();
        }

        /// <summary>
        /// Update our beam's opacity - first fades in and then out.
        /// Continue to play the firing sfx until we are done.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (!fadeOut)
            {
                // Keep increasing opacity
                CurrentOpacityLerp += 2 * elapsedGameTime;
                if (CurrentOpacityLerp >= 1)
                {
                    // If we have reached full opacity we fade out
                    fadeOut = true;

                    // Trigger the event when at full opacity
                    Target.OnCollisionWith(this);
                }
            }
            else
            {
                // Lerp out
                CurrentOpacityLerp -= 2 * elapsedGameTime;
                if (CurrentOpacityLerp <= 0)
                {
                    // Die when we reach 0 opacity again
                    Die();
                }
            }

            Opacity = MathHelper.Lerp(0, 1, CurrentOpacityLerp);
        }

        #endregion
    }
}
