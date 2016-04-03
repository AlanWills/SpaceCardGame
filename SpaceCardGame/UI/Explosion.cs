using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an explosion in our game.
    /// Nothing more than an animated image, but as it will be useful it makes sense to add it to as a separate class.
    /// </summary>
    public class Explosion : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// The animation for our explosion image
        /// </summary>
        private Animation Animation { get; set; }

        /// <summary>
        /// Fixes the centre of the object so that the explosion appears at our inputted position
        /// </summary>
        protected override Vector2 TextureCentre
        {
            get
            {
                DebugUtils.AssertNotNull(Animation);
                return Animation.Centre;
            }
        }

        #endregion

        public Explosion(Vector2 localPosition) :
            this(Vector2.Zero, localPosition)
        {

        }

        public Explosion(Vector2 size, Vector2 localPosition) :
            base(localPosition, "Sprites\\Effects\\Explosion")
        {
            UsesCollider = false;
            Animation = new Animation("Content\\Data\\GameObjects\\Explosion.xml");
        }

        #region Virtual Functions

        /// <summary>
        /// Loads our animation
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            Animation.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Begins playing our animation
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Animation.IsPlaying = true;
        }

        /// <summary>
        /// Updates our explosion animation and kills the explosion if the animation is complete
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Animation.Update(elapsedGameTime);

            if (!Animation.IsPlaying)
            {
                Die();
            }
        }

        /// <summary>
        /// Draws our current frame in the animation
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            SourceRectangle = Animation.CurrentSourceRectangle;

            base.Draw(spriteBatch);
        }

        #endregion
    }
}