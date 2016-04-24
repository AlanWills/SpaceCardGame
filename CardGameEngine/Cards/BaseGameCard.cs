using _2DEngine;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    /// <summary>
    /// An extension of the base card used for cards as game objects
    /// </summary>
    public abstract class BaseGameCard : BaseCard
    {
        // Our card is always going to be added to a specific location, so don't bother inputting a position
        public BaseGameCard(CardData cardData) :
            base(cardData, Vector2.Zero)
        {
            DebugUtils.AssertNotNull(cardData);
        }
    }
}