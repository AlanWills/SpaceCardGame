using _2DEngine;
using _2DEngineData;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CardGameEngine
{
    public enum CardFlipState
    {
        kFaceUp,
        kFaceDown,
    }

    /// <summary>
    /// A base class for our card object
    /// </summary>
    public class Card : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// The card data for this card
        /// </summary>
        public CardData CardData { get; private set; }

        /// <summary>
        /// The current flip state of this card
        /// </summary>
        private CardFlipState FlipState { get; set; }

        #endregion

        public const string CardBackTextureAsset = "Sprites\\Cards\\Back";
        public static Texture2D CardBackTexture;

        // Our card is always going to be added to a specific location, so don't bother inputting a position
        public Card(CardData cardData) :
            base(Vector2.Zero, "")
        {
            DebugUtils.AssertNotNull(cardData);
            CardData = cardData;
            FlipState = CardFlipState.kFaceUp;
        }

        #region Virtual Functions

        /// <summary>
        /// Look up our data from the Card Registry rather than loading it from disc
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return CardData;
        }

        /// <summary>
        /// Override this function to do on laid abilities
        /// </summary>
        public override void Begin()
        {
            base.Begin();
        }

        // Do normal during fight ability, plus a function to determine whether we can do it?

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
        /// Override this function to do on death abilities
        /// </summary>
        public override void Die()
        {
            base.Die();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Flips our card state so that we are now facing up
        /// </summary>
        public void Flip(CardFlipState flipState)
        {
            FlipState = flipState;
        }

        #endregion
    }
}
