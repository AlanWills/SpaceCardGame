using _2DEngine;
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

    public delegate void OnCardFlippedHandler(BaseCard baseCard, CardFlipState newFlipState, CardFlipState oldFlipState);
    public delegate void OnCardDeathHandler(BaseCard baseCard);

    /// <summary>
    /// The base class for all cards in our game
    /// </summary>
    public class BaseCard : ClickableImage
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
        public event OnCardFlippedHandler OnFlip;

        /// <summary>
        /// An event that will be called after this object dies
        /// </summary>
        public OnCardDeathHandler OnDeath;

        /// <summary>
        /// A flag to indicate whether we wish the card to increase in size whilst our mouse is over it.
        /// True by default
        /// </summary>
        public bool EnlargeOnHover { get; set; }

        /// <summary>
        /// A reference to our size we will use to alter the size of this card if hovered over.
        /// This size really drives the size of the card
        /// </summary>
        protected Vector2 DrawingSize { get; set; }

        public const string CardBackTextureAsset = "Sprites\\Cards\\Back";
        public static Texture2D CardBackTexture;

        #endregion

        public BaseCard(CardData cardData, Vector2 localPosition) :
            this(cardData, Vector2.Zero, localPosition)
        {

        }

        public BaseCard(CardData cardData, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, cardData.TextureAsset)
        {
            DebugUtils.AssertNotNull(cardData);

            CardData = cardData;
            TextureAsset = cardData.TextureAsset;
            FlipState = CardFlipState.kFaceUp;
            EnlargeOnHover = true;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up some constants for our animation effects here.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(Size != Vector2.Zero);
            DrawingSize = Size;
        }

        /// <summary>
        /// If the mouse is over the card we enlarge it, otherwise we reset the size back to normal
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(Collider);
            if (Collider.IsMouseOver && EnlargeOnHover && FlipState == CardFlipState.kFaceUp)
            {
                // If our card is face up and the mouse has no attached children (like other cards we want to place), increase the size
                DrawingSize = Size * 2;
            }
            else
            {
                // If the mouse is not over the card, it's size should go back to normal
                DrawingSize = Size;
            }
        }


        /// <summary>
        /// Updates the collider using the drawing size rather than the base size so that we don't get horrible flickering between the two
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public override void UpdateCollider(ref Vector2 position, ref Vector2 size)
        {
            position = WorldPosition;
            size = DrawingSize;
        }

        /// <summary>
        /// Either draw our normal card if we are face up, or the back of the card if we are face down
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipState == CardFlipState.kFaceUp)
            {
                // Store the size of the card, but set the Size property to the DrawingSize for drawing ONLY
                Vector2 currentSize = Size;
                Size = DrawingSize;

                base.Draw(spriteBatch);

                // Set the Size back to it's original value
                Size = currentSize;
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
            CardFlipState oldFlipState = FlipState;
            FlipState = flipState;

            // Call our on flip event if it's not null
            if (OnFlip != null)
            {
                OnFlip(this, FlipState, oldFlipState);
            }
        }

        #endregion
    }
}
