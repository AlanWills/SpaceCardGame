using _2DEngine;
using _2DEngineData;
using CardGameEngineData;
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

    /// <summary>
    /// A base class for our card object
    /// </summary>
    public abstract class Card : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// The card data for this card
        /// </summary>
        public CardData CardData { get; private set; }

        /// <summary>
        /// A reference to a larger version of the card the player will see when mousing over
        /// </summary>
        protected Image CardInfoImage { get; private set; }

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
        /// Sets up our CardInfoImage
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            DebugUtils.AssertNotNull(ScreenManager.Instance.CurrentScreen);
            Debug.Assert(ScreenManager.Instance.CurrentScreen is GameplayScreen);

            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;
            CardInfoImage = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new Image(new Vector2(screenDimensions.X * 0.5f, screenDimensions.Y * 0.5f), ScreenManager.Instance.ScreenCentre, CardData.TextureAsset), true, true);
            CardInfoImage.IsAlive.Connect(IsAlive); // Set this object to die when the thumbnail dies
            CardInfoImage.Hide();

            base.LoadContent();
        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(Collider);
            if (Collider.IsMouseOver)
            {
                CardInfoImage.Show();
            }
            else
            {
                CardInfoImage.Hide();
            }
        }

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
        /// If we hide this, we also wish to hide the detail image if it is still showing
        /// </summary>
        /// <param name="showChildren"></param>
        public override void Hide(bool showChildren = true)
        {
            base.Hide(showChildren);

            CardInfoImage.Hide();
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
