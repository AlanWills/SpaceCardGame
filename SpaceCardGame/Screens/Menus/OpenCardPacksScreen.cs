using _2DEngine;
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
        private List<Card> CardsFromPack { get; set; }

        public OpenCardPacksScreen(string screenDataAsset = "Screens\\OpenCardPacksScreen.xml") :
            base(screenDataAsset)
        {
            CardsFromPack = new List<Card>();
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our grid control for the packs we have to open
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            float gridHeight = ScreenDimensions.Y * 0.25f;
            PacksGridControl = AddScreenUIObject(new GridControl(1, 8, new Vector2(ScreenDimensions.X, gridHeight), new Vector2(ScreenCentre.X, ScreenDimensions.Y - gridHeight * 0.5f)));
            for (int i = 0; i < PlayerCardRegistry.Instance.AvailablePacksToOpen; i++)
            {
                ClickableImage cardPack = PacksGridControl.AddChild(new ClickableImage(new Vector2(gridHeight * 0.8f), Vector2.Zero, Card.CardBackTextureAsset));
                cardPack.ClickableModule.OnLeftClicked += OnPackLeftClicked;
            }
        }

        /// <summary>
        /// Transition back to our lobby screen if we press esc.
        /// Also, saves our current assets to disc - i.e. we retain our newly added cards.
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            PlayerCardRegistry.Instance.SaveAssets();
            Transition(new LobbyMenuScreen());
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// A click callback when we click on a pack.
        /// Opens the pack and populates our screen with cards
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnPackLeftClicked(BaseObject baseObject)
        {
            // Remove any previous cards from packs we have opened
            foreach (Card card in CardsFromPack)
            {
                card.Die();
            }

            // Clear our previous packs from the reference list
            CardsFromPack.Clear();

            // Remove the image in the grid control
            baseObject.Die();

            // Remove an available pack from our player
            PlayerCardRegistry.Instance.AvailablePacksToOpen--;

            // Pick random cards from the registry
            List<CardData> cardData = CentralCardRegistry.PickCardsForPackOpening();
            Debug.Assert(cardData.Count == CentralCardRegistry.PackSize);

            // Add cards to our screen to show these new cards
            for (int i = 0; i < cardData.Count; i++)
            {
                // Position the cards incrementally along the screen and halfway between the top of the grid control and the top of the screen
                Card card = AddScreenUIObject(cardData[i].CreateCard(null), true, true);
                card.LocalPosition = new Vector2(300 * (i + 1), (PacksGridControl.WorldPosition.Y - PacksGridControl.Size.Y * 0.5f) * 0.5f);
                card.ClickableModule.OnLeftClicked += OnCardLeftClicked;
                card.Flip(CardFlipState.kFaceDown);         // Make sure the card is face down initially, so we have the excitement of turning it over!
                card.StoredObject = PlayerCardRegistry.Instance.PlayerOwnsCard(cardData[i]);

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
        /// <param name="baseObject"></param>
        private void OnCardLeftClicked(BaseObject baseObject)
        {
            Debug.Assert(baseObject is Card);
            Card card = (baseObject as Card);
            card.Flip(CardFlipState.kFaceUp);

            // Add some UI if our card is new
            DebugUtils.AssertNotNull(card.StoredObject);
            if (!(bool)card.StoredObject)
            {
                Image newCardIndicator = card.AddChild(new Image(new Vector2(card.Size.X, -card.Size.Y) * 0.5f, "UI\\NewCardIndicator"), true, true);
                newCardIndicator.Colour.Value = Color.Yellow;
            }

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

            foreach (Card card in CardsFromPack)
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
