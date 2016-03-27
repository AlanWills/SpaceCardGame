using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A screen where the player opens packs which they have earnt.
    /// Not really anything fancy in terms of gameplay; just fancy UI.
    /// </summary>
    public class OpenCardPacksScreen : MenuScreen
    {
        /// <summary>
        /// A reference to the grid control we will use to store our packs we have left to open.
        /// </summary>
        private GridControl PacksGridControl { get; set; }

        public OpenCardPacksScreen(string screenDataAsset = "Content\\Data\\Screens\\OpenCardPacksScreen.xml") :
            base(screenDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Set up our grid control for the packs we have to open
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            float gridHeight = ScreenDimensions.Y * 0.25f;
            PacksGridControl = AddScreenUIObject(new GridControl(8, new Vector2(ScreenDimensions.X, gridHeight), new Vector2(ScreenCentre.X, ScreenDimensions.Y - gridHeight * 0.5f)));
            for (int i = 0; i < PlayerCardRegistry.Instance.AvailablePacksToOpen; i++)
            {
                ClickableImage cardPack = PacksGridControl.AddObject(new ClickableImage(new Vector2(gridHeight * 0.8f), Vector2.Zero, BaseUICard.CardBackTextureAsset));
                cardPack.OnLeftClicked += OnPackLeftClicked;
            }
        }

        /// <summary>
        /// Transition back to our lobby screen if we press esc
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new LobbyMenuScreen());
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// A click callback when we click on a pack.
        /// Opens the pack and populates our screen with cards
        /// </summary>
        /// <param name="clickable"></param>
        private void OnPackLeftClicked(IClickable clickable)
        {
            Debug.Assert(clickable is ClickableImage);
            (clickable as ClickableImage).Die();

            List<CardData> cardData = CentralCardRegistry.PickCardsForPackOpening();
            Debug.Assert(cardData.Count == CentralCardRegistry.PackSize);

            for (int i = 0; i < cardData.Count; i++)
            {
                BaseUICard card = AddScreenUIObject(new BaseUICard(cardData[i], new Vector2(300 * (i + 1), 300)), true, true);
                card.OnLeftClicked += OnCardLeftClicked;
                card.Flip(CardFlipState.kFaceDown);         // Make sure the card is face down initially, so we have the excitement of turning it over!
            }
        }

        /// <summary>
        /// A click callback when we click on a pack.
        /// Opens the pack and populates our screen with cards.
        /// </summary>
        /// <param name="clickable"></param>
        private void OnCardLeftClicked(IClickable clickable)
        {
            Debug.Assert(clickable is BaseUICard);
            (clickable as BaseUICard).Flip(CardFlipState.kFaceUp);
        }

        #endregion
    }
}
