using CelesteEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    public class CardHandAnimationModule : BaseObjectModule
    {
        #region Properties and Fields

        /// <summary>
        /// Used for some effects - our card if the mouse is over will move up the screen slightly
        /// </summary>
        public Vector2 RestingPosition { get; private set; }
        private Vector2 HighlightedPosition { get; set; }

        /// <summary>
        /// A vector property that is a local offset from the position of this card to it's highlighted position
        /// </summary>
        public Vector2 OffsetToHighlightedPosition { private get; set; }

        /// <summary>
        /// A flag to indicate whether we wish the card to increase in size whilst our mouse is over it.
        /// True by default
        /// </summary>
        public bool EnlargeOnHover { private get; set; }

        /// <summary>
        /// A reference to our size we will use to alter the size of this card if hovered over.
        /// This size really drives the size of the card
        /// </summary>
        public Vector2 DrawingSize { get; private set; }

        #endregion

        public CardHandAnimationModule() :
            base()
        {
            EnlargeOnHover = true;
            OffsetToHighlightedPosition = new Vector2(0, -140);
        }

        #region Virtual Functions

        /// <summary>
        /// Set up some constants for our animation effects here.
        /// By now the local position should be set.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(AttachedBaseObject.Size != Vector2.Zero);
            DrawingSize = AttachedBaseObject.Size;

            UpdatePositions(AttachedBaseObject.LocalPosition);
        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(AttachedBaseObject.Collider);
            if (AttachedBaseObject.Collider.IsMouseOver)
            {
                // We are sufficiently far away from the end position
                if (AttachedBaseObject.LocalPosition.Y - HighlightedPosition.Y > 2)
                {
                    // Move upwards slightly if we are hovering over
                    AttachedBaseObject.LocalPosition = Vector2.Lerp(AttachedBaseObject.LocalPosition, HighlightedPosition, elapsedGameTime * 5);
                }
                else
                {
                    // We are close enough to be at the end position
                    AttachedBaseObject.LocalPosition = HighlightedPosition;
                }

                if (EnlargeOnHover)
                {
                    // If our card is face up and the mouse has no attached children (like other cards we want to place), increase the size
                    DrawingSize = AttachedBaseObject.Size * 2;
                }
                else
                {
                    // If the mouse is not over the card, it's size should go back to normal
                    DrawingSize = AttachedBaseObject.Size;
                }
            }
            else
            {
                // We are sufficiently far away from the initial position
                if (RestingPosition.Y - AttachedBaseObject.LocalPosition.Y > 2)
                {
                    // Otherwise move back down to initial position
                    AttachedBaseObject.LocalPosition = Vector2.Lerp(AttachedBaseObject.LocalPosition, RestingPosition, elapsedGameTime * 5);
                }
                else
                {
                    // We are close enough to be at the initial position
                    AttachedBaseObject.LocalPosition = RestingPosition;
                }

                // If the mouse is not over the card, it's size should go back to normal
                DrawingSize = AttachedBaseObject.Size;
            }
        }

        /// <summary>
        /// Recalculates the extra positions we use for our lerping effect
        /// </summary>
        /// <param name="newLocalPosition"></param>
        public void UpdatePositions(Vector2 newLocalPosition)
        {
            AttachedBaseObject.LocalPosition = newLocalPosition;
            RestingPosition = newLocalPosition;
            HighlightedPosition = newLocalPosition + OffsetToHighlightedPosition;
        }

        #endregion
    }
}
