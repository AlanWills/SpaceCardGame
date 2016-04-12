using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// Specifically handles the UI for the player's hand
    /// </summary>
    public class HandUI : GridControl
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the player who's hand we're representing
        /// </summary>
        private Player Player { get; set; }

        #endregion

        public HandUI(Player player, Vector2 size, Vector2 localPosition, string backgroundTextureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(player.MaxHandSize, size, localPosition, backgroundTextureAsset)
        {
            Player = player;
            Player.OnCardAddedToHand += AddPlayerHandCardUI;

            // Don't want any cut off on this UI
            UseScissorRectangle = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Rebuilds the spacing of the thumbnails
        /// </summary>
        protected override void RebuildList()
        {
            base.RebuildList();

            foreach (BaseUICard thumbnail in Children)
            {
                thumbnail.UpdatePositions(thumbnail.LocalPosition);
            }
        }

        /// <summary>
        /// This does a check to make sure we are ONLY adding BaseUICards and sets up some positions for fancy animation!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override T AddChild<T>(T uiObjectToAdd, bool load = false, bool initialise = false)
        {
            Debug.Assert(uiObjectToAdd is BaseUICard);
            (uiObjectToAdd as BaseUICard).OffsetToHighlightedPosition = new Vector2(0, -100);

            return base.AddChild(uiObjectToAdd, load, initialise);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Tries to find the card thumbnail which represents the inputted card data's display name
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public BaseUICard FindCardThumbnail(CardData cardData)
        {
            // We use the find function here because it searched the objects to add too.
            // Sometimes we may draw a card, so the card data is immediately added, but the UIObject takes one frame to be added
            BaseUICard card = Children.FindChild<BaseUICard>(x => x.Name == cardData.DisplayName);

            DebugUtils.AssertNotNull(card);
            return card;
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// A callback every time we draw a card.
        /// Adds appropriate UI for the newly drawn card.
        /// </summary>
        /// <param name="drawnCard"></param>
        private void AddPlayerHandCardUI(CardData drawnCard)
        {
            BaseUICard cardUI = AddChild(new BaseUICard(drawnCard, new Vector2(Size.X / Player.MaxHandSize, Size.Y), Vector2.Zero), true, true);
            cardUI.Name = drawnCard.DisplayName;

            CardFlipState cardFlipState = Player == BattleScreen.Player ? CardFlipState.kFaceUp : CardFlipState.kFaceDown;
            cardUI.Flip(cardFlipState);

            cardUI.OnLeftClicked += RunPlaceCardScript;
            cardUI.OnDeath += SyncPlayerHand;
            cardUI.OnDeath += RebuildCallback;
        }

        /// <summary>
        /// A callback which adds a card in our hand to the game
        /// </summary>
        /// <param name="clickable"></param>
        private void RunPlaceCardScript(IClickable clickable)
        {
            Debug.Assert(clickable is BaseUICard);
            BaseUICard card = clickable as BaseUICard;

            string error = "";
            if (card.CardData.CanLay(Player, ref error))
            {
                ScriptManager.Instance.AddChild(new PlaceCardScript(card), true, true);
            }
            else
            {
                ScriptManager.Instance.AddChild(new FlashingTextScript(error, ScreenManager.Instance.ScreenCentre, Color.White, 2), true, true);
            }
        }

        /// <summary>
        /// Removes the card data the dead card thumbnail UI represents from the player's hand.
        /// </summary>
        private void SyncPlayerHand(BaseCard cardThumbnail)
        {
            Player.CurrentHand.Remove(cardThumbnail.CardData);
        }

        /// <summary>
        /// Indicates that we need to rebuild this UI
        /// </summary>
        private void RebuildCallback(BaseCard cardThumbnail)
        {
            NeedsRebuild = true;
        }

        #endregion
    }
}