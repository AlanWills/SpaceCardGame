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

        /// <summary>
        /// A list of references to the cards we opened from our pack.
        /// We use these to work out when we can open a new pack etc.
        /// </summary>
        private List<BaseUICard> CardsFromPack { get; set; }

        public OpenCardPacksScreen(string screenDataAsset = "Content\\Data\\Screens\\OpenCardPacksScreen.xml") :
            base(screenDataAsset)
        {
            CardsFromPack = new List<BaseUICard>();
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
            // Remove any previous cards from packs we have opened
            foreach (BaseUICard card in CardsFromPack)
            {
                card.Die();
            }

            // Clear our previous packs from the reference list
            CardsFromPack.Clear();

            // Remove the image in the grid control
            Debug.Assert(clickable is ClickableImage);
            (clickable as ClickableImage).Die();

            // Remove an available pack from our player
            PlayerCardRegistry.Instance.AvailablePacksToOpen--;

            // Pick random cards from the registry
            List<CardData> cardData = CentralCardRegistry.PickCardsForPackOpening();
            Debug.Assert(cardData.Count == CentralCardRegistry.PackSize);

            // Add cards to our screen to show these new cards
            for (int i = 0; i < cardData.Count; i++)
            {
                BaseUICard card = AddScreenUIObject(new BaseUICard(cardData[i], new Vector2(300 * (i + 1), 300)), true, true);
                card.OnLeftClicked += OnCardLeftClicked;
                card.Flip(CardFlipState.kFaceDown);         // Make sure the card is face down initially, so we have the excitement of turning it over!

                CardsFromPack.Add(card);
            }

            // Add the newly unlocked cards to the Player's card registry
            PlayerCardRegistry.Instance.AvailableCards.AddRange(cardData);

            // Stop our grid control from accepting input
            PacksGridControl.ShouldHandleInput.Value = false;
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

            if (CheckAllCardsFlippedFaceUp())
            {
                // Allow our grid control to accept input again now that all the cards are face up
                PacksGridControl.ShouldHandleInput.Value = true;
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Checks to see if one of the cards from our pack is still face down
        /// </summary>
        /// <returns>True if all are face up and false if at least one is face down</returns>
        private bool CheckAllCardsFlippedFaceUp()
        {
            Debug.Assert(CardsFromPack.Count == CentralCardRegistry.PackSize);

            foreach (BaseUICard card in CardsFromPack)
            {
                if (card.FlipState == CardFlipState.kFaceDown)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
