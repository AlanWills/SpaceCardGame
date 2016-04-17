using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an engine blaze in our game.
    /// Nothing more than an animated image, but as it will be useful it makes to have a separate class for it.
    /// </summary>
    public class EngineBlaze : AnimatedGameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our particle emitter we will use to generate smoke
        /// </summary>
        private ParticleEmitter SmokeParticleEmitter { get; set; }

        #endregion

        public EngineBlaze(Vector2 localPosition) :
            base(localPosition, "GameObjects\\Animations\\EngineBlaze.xml")
        {
            UsesCollider = false;
        }
        
        #region Virtual Functions

        /// <summary>
        /// Set up our particle emitter after we have set the size
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            SetUpParticleEmitter();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Create the particle emitter with it's default values
        /// </summary>
        private void SetUpParticleEmitter()
        {
            Vector2 particleSize = TextureCentre;

            SmokeParticleEmitter = AddChild(new ParticleEmitter(
                particleSize, particleSize, Vector2.Zero, new Vector2(0, 15f), new Vector2(0, 4f),
                Color.White, Color.White, 0.5f, 0.25f, 0.25f, new Vector2(0, Size.Y * 0.5f), "GameObjects\\Animations\\Smoke.xml"), true, true);
        }

        #endregion
    }
}
