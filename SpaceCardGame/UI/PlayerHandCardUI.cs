using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class to represent a card in our player's cand - deals with animation effects and playing it on the board
    /// </summary>
    public class PlayerHandCardUI : ClickableImage
    {
        /// <summary>
        /// A reference to the card data for this card
        /// </summary>
        private CardData CardData { get; set; }

        /// <summary>
        /// Used for some effects - our card if the mouse is over will move up the screen slightly
        /// </summary>
        private Vector2 InitialPosition { get; set; }
        private Vector2 EndPosition { get; set; }

        public PlayerHandCardUI(CardData cardData, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, cardData.TextureAsset)
        {
            CardData = cardData;
            InitialPosition = localPosition;
            EndPosition = localPosition -= new Vector2(0, 35);
        }

        #region Virtual Functions

        /// <summary>
        /// Handles some simple animation if our card has it's mouse over
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            /*if (Collider.IsMouseOver)
            {
                // We are sufficiently far away from the end position
                if (LocalPosition.Y - EndPosition.Y > 2)
                {
                    // Move upwards slightly if we are hovering over
                    LocalPosition = Vector2.Lerp(LocalPosition, InitialPosition -= new Vector2(0, 25), elapsedGameTime);
                }
                else
                {
                    // We are close enough to be at the end position
                    LocalPosition = EndPosition;
                }
            }
            else
            {
                // We are sufficiently far away from the initial position
                if (InitialPosition.Y - LocalPosition.Y > 2)
                {
                    // Otherwise move back down to initial position
                    LocalPosition = Vector2.Lerp(LocalPosition, InitialPosition, elapsedGameTime);
                }
                else
                {
                    // We are close enough to be at the initial position
                    LocalPosition = InitialPosition;
                }
            }*/
        }

        #endregion
    }
}
