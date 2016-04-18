using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    public class BaseCard : Image
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
        /// A highlight module we will use to draw an outline around the card
        /// </summary>
        public OutlineOnHoverModule OutlineModule { get; private set; }

        public const string CardBackTextureAsset = "Cards\\Back";
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
            UsesCollider = true;                    // Explicitly state this because Image does not use a collider

            OutlineModule = AddModule(new OutlineOnHoverModule());
        }

        #region Virtual Functions

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
                    Colour.Value * Opacity,
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
