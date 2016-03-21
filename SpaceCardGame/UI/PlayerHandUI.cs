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
    public class PlayerHandUI : GridControl
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the player who's hand we're representing
        /// </summary>
        private Player Player { get; set; }

        #endregion

        public PlayerHandUI(Player player, Vector2 localPosition, string backgroundTextureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(10, new Vector2(ScreenManager.Instance.ScreenDimensions.X * 0.8f, ScreenManager.Instance.ScreenDimensions.Y * 0.15f), localPosition, backgroundTextureAsset)
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

            foreach (PlayerHandCardThumbnail thumbnail in UIObjects)
            {
                thumbnail.UpdatePositions(thumbnail.LocalPosition);
            }
        }

        /// <summary>
        /// This just does a check to make sure we are ONLY adding PlayerHandCardThumbnails
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override T AddObject<T>(T uiObjectToAdd, bool load = false, bool initialise = false)
        {
            Debug.Assert(uiObjectToAdd is PlayerHandCardThumbnail);

            return base.AddObject(uiObjectToAdd, load, initialise);
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
            Vector2 size = new Vector2(Size.X * 0.1f, Size.Y);
            PlayerHandCardThumbnail cardUI = AddObject(new PlayerHandCardThumbnail(drawnCard, size, Vector2.Zero), true, true);
            size = cardUI.Size;
            cardUI.LocalPosition = new Vector2((size.X + padding) * (Player.CurrentHand.Count - 1) + (size.X - Size.X) * 0.5f + padding, 0);

            cardUI.OnLeftClicked += AddCardToGame;
            cardUI.OnRightClicked += AddInfoImage;
            cardUI.OnDeath += SyncPlayerHand;
            cardUI.OnDeath += RebuildCallback;
        }

        /// <summary>
        /// A callback which adds a card in our hand to the game
        /// </summary>
        /// <param name="clickable"></param>
        private void AddCardToGame(IClickable clickable)
        {
            Debug.Assert(clickable is PlayerHandCardThumbnail);

            ScriptManager.Instance.AddObject(new PlaceCardScript(clickable as PlayerHandCardThumbnail), true, true);
        }

        /// <summary>
        /// A callback which displays an image with more in depth information about a card, bound to a click dismiss script (which will remove it as soon as we click anywhere)
        /// </summary>
        /// <param name="clickable"></param>
        private void AddInfoImage(IClickable clickable)
        {
            Debug.Assert(clickable is UIObject);

            string cardTextureAsset = (clickable as UIObject).TextureAsset;
            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;

            // Add the info image to the screen rather than this, because we want this to be exclusively for PlayerHandCardThumbnails
            Image infoImage = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new Image(new Vector2(screenDimensions.X * 0.5f, screenDimensions.Y * 0.5f), ScreenManager.Instance.ScreenCentre, cardTextureAsset), true, true);
            ScriptManager.Instance.AddObject(new ClickDismissScript(infoImage), true, true);
        }

        /// <summary>
        /// Removes the card data the dead card thumbnail UI represents from the player's hand.
        /// </summary>
        public void SyncPlayerHand(PlayerHandCardThumbnail cardThumbnail)
        {
            Player.CurrentHand.RemoveAll(x => ReferenceEquals(x, cardThumbnail.CardData));
        }

        /// <summary>
        /// Indicates that we need to rebuild this UI
        /// </summary>
        public void RebuildCallback(PlayerHandCardThumbnail cardThumbnail)
        {
            NeedsRebuild = true;
        }

        #endregion
    }
}
