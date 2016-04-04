using _2DEngine;
using _2DEngineData;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CardGameEngine
{
    public delegate void OnGameCardFlippedHandler(BaseGameCard baseGaneCard, CardFlipState newFlipState, CardFlipState oldFlipState);
    public delegate void OnGameCardDeathHandler(BaseGameCard gameCard);

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
        /// The current flip state of this card
        /// </summary>
        public CardFlipState FlipState { get; private set; }

        /// <summary>
        /// An event which is called when the card flip state is changed.
        /// Passes the new flip state.
        /// </summary>
        public event OnGameCardFlippedHandler OnFlip;

        /// <summary>
        /// An event which is called when this card dies.
        /// Passes the dead object.
        /// </summary>
        public event OnGameCardDeathHandler OnDeath;

        /// <summary>
        /// A flag to indicate whether this card has been placed on the board
        /// </summary>
        public bool IsPlaced { get; set; }

        /// <summary>
        /// A reference to our size we will use to alter the size of this card if hovered over.
        /// This size really drives the size of the card
        /// </summary>
        private Vector2 DrawingSize { get; set; }

        #endregion

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

            Debug.Assert(Size != Vector2.Zero);
            DrawingSize = Size;
        }

        /// <summary>
        /// If the mouse is over the card (but with no attached children - i.e. another card we are placing) we show the info image, otherwise we hide it
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (IsPlaced)
            {
                DebugUtils.AssertNotNull(Collider);
                if (GameMouse.Instance.Children.Count == 0 && Collider.IsMouseOver)
                {
                    DrawingSize = Size * 2;
                }
                else
                {
                    DrawingSize = Size;
                }
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
                DebugUtils.AssertNotNull(BaseUICard.CardBackTexture);
                spriteBatch.Draw(
                    BaseUICard.CardBackTexture,
                    WorldPosition,
                    null,
                    SourceRectangle,
                    TextureCentre,
                    WorldRotation,
                    Vector2.Divide(Size, new Vector2(BaseUICard.CardBackTexture.Width, BaseUICard.CardBackTexture.Height)),
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
