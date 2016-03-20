using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    /// <summary>
    /// A class to represent a card in our player's cand - deals with animation effects and playing it on the board
    /// </summary>
    public class PlayerHandCardThumbnail : ClickableImage
    {
        /// <summary>
        /// A reference to the card data for this card UI
        /// </summary>
        public CardData CardData { get; private set; }

        /// <summary>
        /// Used for some effects - our card if the mouse is over will move up the screen slightly
        /// </summary>
        private Vector2 RestingPosition { get; set; }
        private Vector2 HighlightedPosition { get; set; }

        public PlayerHandCardThumbnail(CardData cardData, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, cardData.TextureAsset)
        {
            CardData = cardData;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up some constants for our animation effects here.
        /// By now the local position should be set.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            RestingPosition = LocalPosition;
            HighlightedPosition = RestingPosition - new Vector2(0, 35);

            // Experimental - have the card drop down to it's resting position
            LocalPosition = HighlightedPosition;
        }

        /// <summary>
        /// Handles some simple animation if our card has it's mouse over
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

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
            }
        }

        #endregion
    }
}
