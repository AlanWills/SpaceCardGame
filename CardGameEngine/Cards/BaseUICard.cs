using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace CardGameEngine
{
    /// <summary>
    /// An extensions of the base card for UI purposes - has fancy animations etc.
    /// </summary>
    public class BaseUICard : BaseCard
    {
        #region Properties and Fields
        
        /// <summary>
        /// A reference to the clickable module attached to this UI card.
        /// </summary>
        public ClickableObjectModule ClickableModule { get; private set; }

        /// <summary>
        /// A flag to indicate whether we wish the card to increase in size whilst our mouse is over it.
        /// True by default
        /// </summary>
        public bool EnlargeOnHover { get; set; }

        /// <summary>
        /// A reference to our size we will use to alter the size of this card if hovered over.
        /// This size really drives the size of the card
        /// </summary>
        private Vector2 DrawingSize { get; set; }

        /// <summary>
        /// Used for some effects - our card if the mouse is over will move up the screen slightly
        /// </summary>
        private Vector2 RestingPosition { get; set; }
        private Vector2 HighlightedPosition { get; set; }
        
        /// <summary>
        /// A vector property that is a local offset from the position of this card to it's highlighted position
        /// </summary>
        public Vector2 OffsetToHighlightedPosition { get; set; }

        #endregion

        public BaseUICard(CardData cardData, Vector2 localPosition) :
            this(cardData, Vector2.Zero, localPosition)
        {
            
        }

        public BaseUICard(CardData cardData, Vector2 size, Vector2 localPosition) :
            base(cardData, size, localPosition)
        {
            DebugUtils.AssertNotNull(cardData);

            EnlargeOnHover = true;
            OffsetToHighlightedPosition = new Vector2(0, -35);
            ClickableModule = AddModule(new ClickableObjectModule());       // Add our clickable module
        }

        #region Virtual Functions

        /// <summary>
        /// Set up some constants for our animation effects here.
        /// By now the local position should be set.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(Size != Vector2.Zero);
            DrawingSize = Size;

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

                if (EnlargeOnHover && FlipState == CardFlipState.kFaceUp)
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
            size = new Vector2(DrawingSize.X, DrawingSize.Y + Math.Abs(RestingPosition.Y - LocalPosition.Y));   // Not perfect, but better.  Over estimates the top
        }

        /// <summary>
        /// Either draw our normal card if we are face up, or the back of the card if we are face down
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Store the size of the card, but set the Size property to the DrawingSize for drawing ONLY
            Vector2 currentSize = Size;
            Size = DrawingSize;

            base.Draw(spriteBatch);

            // Set the Size back to it's original value
            Size = currentSize;
        }

        #endregion

        #region Utility Functions

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
