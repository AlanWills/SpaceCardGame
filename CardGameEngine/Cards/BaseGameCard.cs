using _2DEngine;
using _2DEngineData;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CardGameEngine
{
    /// <summary>
    /// A base class for any game object card
    /// </summary>
    public abstract class BaseGameCard : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// The card data for this card
        /// </summary>
        public CardData CardData { get; private set; }

        /// <summary>
        /// A reference to a larger version of the card the player will see when mousing over
        /// </summary>
        protected Image CardInfoImage { get; private set; }

        /// <summary>
        /// The current flip state of this card
        /// </summary>
        private CardFlipState FlipState { get; set; }

        /// <summary>
        /// An event which is called when the card flip state is changed.
        /// Passes the new flip state.
        /// </summary>
        public event OnFlipHandler OnFlip;

        /// <summary>
        /// A flag to indicate whether this card has been placed on the board
        /// </summary>
        public bool IsPlaced { get; set; }

        #endregion

        public const string CardBackTextureAsset = "Sprites\\Cards\\Back";
        public static Texture2D CardBackTexture;

        // Our card is always going to be added to a specific location, so don't bother inputting a position
        public BaseGameCard(CardData cardData) :
            base(Vector2.Zero, "")
        {
            DebugUtils.AssertNotNull(cardData);
            CardData = cardData;
            FlipState = CardFlipState.kFaceUp;
        }

        #region Virtual Functions

        /// <summary>
        /// Create and set up our card info image.
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            DebugUtils.AssertNotNull(ScreenManager.Instance.CurrentScreen);
            Debug.Assert(ScreenManager.Instance.CurrentScreen is GameplayScreen);

            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;
            CardInfoImage = new Image(new Vector2(screenDimensions.X * 0.5f, screenDimensions.Y * 0.5f), Vector2.Zero, CardData.TextureAsset);
            CardInfoImage.IsAlive.Connect(IsAlive); // Set the card info object to die when the thumbnail dies
            CardInfoImage.Hide();
            CardInfoImage.SetParent(this, true);

            base.Initialise();
        }

        /// <summary>
        /// Return our card data rather than reloading data files
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return CardData;
        }

        /// <summary>
        /// Add our card info image to the parent screen
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // We do this here, because we want this to be drawn on top.  If we add the object in load content or initialise, it will be added before the BaseUICard and so will be drawn underneath
            ScreenManager.Instance.CurrentScreen.AddScreenUIObject(CardInfoImage, true, true);
        }

        /// <summary>
        /// If the mouse is over the card we show the info image, otherwise we hide it
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (IsPlaced)
            {
                DebugUtils.AssertNotNull(Collider);
                if (Collider.IsMouseOver)
                {
                    CardInfoImage.Show();
                }
                else
                {
                    CardInfoImage.Hide();
                }
            }
        }

        /// <summary>
        /// Either draw our normal card if we are face up, or the back of the card if we are face down
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipState == CardFlipState.kFaceUp)
            {
                base.Draw(spriteBatch);
            }
            else
            {
                DebugUtils.AssertNotNull(CardBackTexture);
                spriteBatch.Draw(
                    CardBackTexture,
                    WorldPosition,
                    null,
                    SourceRectangle,
                    TextureCentre,
                    WorldRotation,
                    Vector2.Divide(Size, new Vector2(CardBackTexture.Width, CardBackTexture.Height)),
                    Colour * Opacity,
                    SpriteEffect,
                    0);
            }
        }

        /// <summary>
        /// If we hide this, we also wish to hide the detail image if it is still showing
        /// </summary>
        /// <param name="showChildren"></param>
        public override void Hide(bool showChildren = true)
        {
            base.Hide(showChildren);

            CardInfoImage.Hide();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Flips our card state so that we are now facing up
        /// </summary>
        public void Flip(CardFlipState flipState)
        {
            FlipState = flipState;

            // Call our on flip event if it's not null
            if (OnFlip != null)
            {
                OnFlip(FlipState);
            }
        }

        #endregion
    }
}
