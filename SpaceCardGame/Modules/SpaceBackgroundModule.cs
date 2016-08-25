using CelesteEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which adds extra space decals to a screen.  The screen still has to implement the background texture.
    /// </summary>
    public class SpaceBackgroundModule : Module
    {
        /// <summary>
        /// An array of references to the asteroids in our scene - this will be fixed size based on the density and the number of different asteroid textures we are using.
        /// The asteroids will be constantly recycled - when they leave the right hand side of the screen they will be reinserted into the left hand side with a new rigid body setup and position.
        /// </summary>
        private Asteroid[] Asteroids { get; set; }

        /// <summary>
        /// The area that our asteroids will be bounded to
        /// </summary>
        private Vector2 Region { get; set; }

        private const int asteroidDensity = 5;
        private const string backgroundDataAsset = "GameObjects\\SpaceBackground";

        /// <summary>
        /// Restrict this module to just screens
        /// </summary>
        /// <param name="baseScreen"></param>
        public SpaceBackgroundModule(BaseScreen baseScreen)
        {
            Region = ScreenManager.Instance.ScreenDimensions;
        }

        #region Virtual Functions

        /// <summary>
        /// Create the asteroids
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            SpaceBackgroundData data = AssetManager.GetData<SpaceBackgroundData>(backgroundDataAsset);

            // Add any extra decals first so that they appear underneath our board sections
            // This will prevent anything obstructing our cards and card objects
            // Create the fixed size array of asteroids
            Asteroids = new Asteroid[data.AsteroidTextureAssets.Count * asteroidDensity];

            Debug.Assert(AttachedComponent is BaseScreen);
            BaseScreen screen = AttachedComponent as BaseScreen;
            Vector2 size = ScreenManager.Instance.ScreenDimensions;

            int asteroidTypeIndex = 0;
            foreach (string asteroidTextureAsset in data.AsteroidTextureAssets)
            {
                for (int i = 0; i < asteroidDensity; ++i)
                {
                    float xPos = MathUtils.GenerateFloat(-Region.X * 0.1f, Region.X * 0.9f);
                    float yPos = MathUtils.GenerateFloat(0, Region.Y);

                    // Make sure this is all rendered at the back by doing add game object
                    Asteroids[asteroidTypeIndex * asteroidDensity + i] = screen.AddGameObject(new Asteroid(new Vector2(xPos, yPos), asteroidTextureAsset), true);
                }

                asteroidTypeIndex++;
            }

            base.LoadContent();
        }

        /// <summary>
        /// Loops through our asteroids and moves any that have gone off the side of the RHS of the board back over onto the LHS.
        /// Then gives the asteroid a new y position and speed (to simulate more is happening than actually is).
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Vector2 size = ScreenManager.Instance.ScreenDimensions;

            foreach (Asteroid asteroid in Asteroids)
            {
                if (asteroid.LocalPosition.X > Region.X + asteroid.Size.X * 0.5f)
                {
                    float yPos = MathUtils.GenerateFloat(0, Region.Y);
                    float speed = MathUtils.GenerateFloat(1, 20);

                    asteroid.LocalPosition = new Vector2(-Region.X * 0.1f, yPos);
                    asteroid.RigidBody.LinearVelocity = new Vector2(speed, 0);
                }
            }
        }

        #endregion
    }
}
