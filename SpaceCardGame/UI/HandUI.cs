using _2DEngine;
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
        private GamePlayer Player { get; set; }

        #endregion

        public HandUI(GamePlayer player, Vector2 size, Vector2 localPosition, string backgroundTextureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
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

            foreach (Card thumbnail in Children)
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
            foreach (Card card in Children)
            {
                card.CardOutline.Valid = card.CanLay(Player, ref error);
            }
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// A callback every time we draw a card.
        /// Adds appropriate UI for the newly drawn card.
        /// </summary>
        /// <param name="drawnCard"></param>
        private void AddPlayerHandCardUI(Card drawnCard)
        {
            AddChild(drawnCard);

            CardFlipState cardFlipState = Player == BattleScreen.Player ? CardFlipState.kFaceUp : CardFlipState.kFaceDown;
            drawnCard.Flip(cardFlipState);

            drawnCard.ClickableModule.OnLeftClicked += RunPlaceCardCommand;
            drawnCard.ClickableModule.OnLeftClicked += RebuildCallback;
        }

        /// <summary>
        /// A callback which adds a card in our hand to the game
        /// </summary>
        /// <param name="clickable"></param>
        private void RunPlaceCardCommand(BaseObject baseObject)
        {
            Debug.Assert(baseObject is Card);
            Card card = baseObject as Card;

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            // Only lay our cards during the card placement phase
            if (battleScreen.TurnState == TurnState.kPlaceCards)
            {
                string error = "";
                if (card.CanLay(Player, ref error))
                {
                    CommandManager.Instance.AddChild(new PlaceCardCommand(card), true, true);
                }
                else
                {
                    CommandManager.Instance.AddChild(new FlashingTextCommand(error, ScreenManager.Instance.ScreenCentre, Color.White, false, 2), true, true);
                }
            }
        }

        /// <summary>
        /// Indicates that we need to rebuild this UI
        /// </summary>
        private void RebuildCallback(BaseObject clickedObject)
        {
            NeedsRebuild = true;
        }

        #endregion
    }
}