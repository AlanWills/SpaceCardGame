using _2DEngine;
using _2DEngineData;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CardGameEngine
{
    public enum CardFlipState
    {
        kFaceUp,
        kFaceDown,
    }

    public delegate void OnFlipHandler(CardFlipState newFlipState);
    public delegate void OnUICardDeathHandler(BaseUICard cardThumbnail);

    /// <summary>
    /// A base class for any UI card object
    /// </summary>
    public class BaseUICard : ClickableImage
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
        /// An event that will be called after this object dies
        /// </summary>
        public OnUICardDeathHandler OnDeath;

        /// <summary>
        /// Used for some effects - our card if the mouse is over will move up the screen slightly
        /// </summary>
        private Vector2 RestingPosition { get; set; }
        private Vector2 HighlightedPosition { get; set; }

        #endregion

        public const string CardBackTextureAsset = "Sprites\\Cards\\Back";
        public static Texture2D CardBackTexture;

        public BaseUICard(CardData cardData, Vector2 localPosition) :
            this(cardData, Vector2.Zero, localPosition)
        {
            
        }

        public BaseUICard(CardData cardData, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, "")
        {
            DebugUtils.AssertNotNull(cardData);

            CardData = cardData;
            TextureAsset = cardData.TextureAsset;
            FlipState = CardFlipState.kFaceUp;
        }

        #region Virtual Functions

        /// <summary>
        /// Create and set up our CardInfoImage.
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            DebugUtils.AssertNotNull(ScreenManager.Instance.CurrentScreen);

            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;
            CardInfoImage = new Image(new Vector2(screenDimensions.X * 0.5f, screenDimensions.Y * 0.5f), Vector2.Zero, CardData.TextureAsset);
            CardInfoImage.IsAlive.Connect(IsAlive); // Set the card info object to die when the thumbnail dies
            CardInfoImage.Hide();
            CardInfoImage.SetParent(this, true);

            base.Initialise();
        }

        /// <summary>
        /// Add our CardInfoImage to the parent screen.
        /// Set up some constants for our animation effects here.
        /// By now the local position should be set.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // We do this here, because we want this to be drawn on top.  If we add the object in load content or initialise, it will be added before the BaseUICard and so will be drawn underneath
            ScreenManager.Instance.CurrentScreen.AddScreenUIObject(CardInfoImage, true, true);

            UpdatePositions(LocalPosition);

            // Experimental - have the card drop down to it's resting position
            LocalPosition = HighlightedPosition;
        }

        /// <summary>
        /// If the mouse is over the card we show the info image, otherwise we hide it
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(Collider);
            if (Collider.IsMouseOver)
            {
                // We are sufficiently far away from the end position
                if (LocalPosition.Y - HighlightedPosition.Y > 2)
                {
                    // Move upwards slightly if we are hovering over
                    LocalPosition = Vector2.Lerp(LocalPosition, RestingPosition - new Vector2(0, 25), elapsedGameTime * 5);
                }
                else
                {
                    // We are close enough to be at the end position
                    LocalPosition = HighlightedPosition;
                }

                // If our card is face up, show the info image and hide our base card
                if (FlipState == CardFlipState.kFaceUp)
                {
                    CardInfoImage.Show();
                    ShouldDraw.Value = false;
                }
                else
                {
                    ShouldDraw.Value = true;
                }
            }
            else
            {
                // We are sufficiently far away from the initial position
                if (RestingPosition.Y - LocalPosition.Y > 2)
                {
                    // Otherwise move back down to initial position
                    LocalPosition = Vector2.Lerp(LocalPosition, RestingPosition, elapsedGameTime * 5);
                }
                else
                {
                    // We are close enough to be at the initial position
                    LocalPosition = RestingPosition;
                }

                CardInfoImage.Hide();
                ShouldDraw.Value = true;
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

        /// <summary>
        /// Calls the OnDeath event if it is hooked up
        /// </summary>
        public override void Die()
        {
            base.Die();

            if (OnDeath != null)
            {
                OnDeath(this);
            }
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

        /// <summary>
        /// Recalculates the extra positions we use for our lerping effect
        /// </summary>
        /// <param name="newLocalPosition"></param>
        public void UpdatePositions(Vector2 newLocalPosition)
        {
            LocalPosition = newLocalPosition;
            RestingPosition = newLocalPosition;
            HighlightedPosition = newLocalPosition - new Vector2(0, 35);
        }

        #endregion
    }
}
