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
        /// The current flip state of this card
        /// </summary>
        public CardFlipState FlipState { get; private set; }

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

        /// <summary>
        /// A vector property that is a local offset from the position of this card to it's highlighted position
        /// </summary>
        public Vector2 OffsetToHighlightedPosition { get; set; }

        /// <summary>
        /// A reference to our original size we will use to alter the size of this card if hovered over
        /// </summary>
        private Vector2 OriginalSize { get; set; }

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
            OffsetToHighlightedPosition = new Vector2(0, -35);
        }

        #region Virtual Functions

        /// <summary>
        /// Add our CardInfoImage to the parent screen.
        /// Set up some constants for our animation effects here.
        /// By now the local position should be set.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(Size != Vector2.Zero);
            OriginalSize = Size;

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
                    LocalPosition = Vector2.Lerp(LocalPosition, HighlightedPosition, elapsedGameTime * 5);
                }
                else
                {
                    // We are close enough to be at the end position
                    LocalPosition = HighlightedPosition;
                }

                // If our card is face up, show the info image and hide our base card
                if (FlipState == CardFlipState.kFaceUp)
                {
                    Size = OriginalSize * 1.5f;
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

                Size = OriginalSize;
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
            HighlightedPosition = newLocalPosition + OffsetToHighlightedPosition;
        }

        #endregion
    }
}
