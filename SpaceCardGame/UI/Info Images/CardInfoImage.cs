using CelesteEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A piece of UI that displays all relevant information about a card including Name, Image, Price
    /// </summary>
    public class CardInfoImage : Image
    {
        #region Properties and Fields

        /// <summary>
        /// The card data we are displaying
        /// </summary>
        private CardData CardData { get; set; }

        #endregion

        public CardInfoImage(CardData cardData, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(cardData, Vector2.Zero, localPosition, textureAsset)
        {

        }

        public CardInfoImage(CardData cardData, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            base(size, localPosition, textureAsset)
        {
            CardData = cardData;
        }

        #region Virtual Functions

        /// <summary>
        /// Create various pieces of UI for the card's info
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            Image cardImage = AddChild(new Image(Size * 0.8f, Vector2.Zero, CardData.TextureAsset));
            Label cardName = cardImage.AddChild(new Label(CardData.DisplayName, Anchor.kTopCentre, 2));
            Label cardPrice = cardImage.AddChild(new Label("100 - Hard Coded", Anchor.kBottomCentre, 2));

            base.LoadContent();
        }

        #endregion
    }
}
