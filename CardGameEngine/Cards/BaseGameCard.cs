using _2DEngine;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    /// <summary>
    /// An extension of the base card used for cards as game objects
    /// </summary>
    public abstract class BaseGameCard : BaseCard
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether this card has been placed on the board
        /// </summary>
        public bool IsPlaced { get; set; }

        #endregion

        // Our card is always going to be added to a specific location, so don't bother inputting a position
        public BaseGameCard(CardData cardData) :
            base(cardData, Vector2.Zero)
        {
            DebugUtils.AssertNotNull(cardData);
            IsPlaced = false;
        }

        #region Virtual Functions

        /// <summary>
        /// If the card is not placed, undo the hover enlarge animation
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (!IsPlaced)
            {
                DrawingSize = Size;
            }
        }

        #endregion
    }
}
