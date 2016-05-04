using _2DEngine;
using CardGameEngine;
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
            base(1, player.MaxHandSize, size, localPosition, backgroundTextureAsset)
        {
            Player = player;
            Player.OnCardAddedToHand += AddPlayerHandCardUI;

            // Don't want any cut off on this UI
            UseScissorRectangle = false;
            BorderPadding = Vector2.Zero;       // No border padding for the HandUI
            ScrollingEnabled = false;
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
        /// Loops through the cards in our hand and updates them based on whether we can lay them
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            string error = "";
            foreach (BaseUICard card in Children)
            {
                Debug.Assert(card.CardData is GameCardData);
                card.CardOutline.Valid = (card.CardData as GameCardData).CanLay(Player, ref error);
            }
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

            cardUI.ClickableModule.OnLeftClicked += RunPlaceCardCommand;
            cardUI.OnDeath += SyncPlayerHand;
            cardUI.OnDeath += RebuildCallback;
        }

        /// <summary>
        /// A callback which adds a card in our hand to the game
        /// </summary>
        /// <param name="clickable"></param>
        private void RunPlaceCardCommand(BaseObject baseObject)
        {
            Debug.Assert(baseObject is BaseUICard);
            BaseUICard card = baseObject as BaseUICard;
            Debug.Assert(card.CardData is GameCardData);

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            // Only lay our cards during the card placement phase
            if (battleScreen.TurnState == TurnState.kPlaceCards)
            {
                string error = "";
                if ((card.CardData as GameCardData).CanLay(Player, ref error))
                {
                    CommandManager.Instance.AddChild(new PlaceCardCommand(card), true, true);
                }
                else
                {
                    CommandManager.Instance.AddChild(new FlashingTextCommand(error, ScreenManager.Instance.ScreenCentre, Color.White, 2), true, true);
                }
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