using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// Specifically handles the UI for the player's hand
    /// </summary>
    public class PlayerHandUI : UIObjectContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the player who's hand we're representing
        /// </summary>
        private Player Player { get; set; }

        /// <summary>
        /// A list of all the current player card thumbnails, used for rebuilding the UI
        /// </summary>
        private List<PlayerHandCardThumbnail> PlayerCardThumbnails { get; set; }

        /// <summary>
        /// A flag to indicate whether we should rebuild the UI.
        /// </summary>
        private bool NeedsRebuild { get; set; }

        private float padding = 35;

        #endregion

        public PlayerHandUI(Player player, Vector2 localPosition, string backgroundTextureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.15f), localPosition, backgroundTextureAsset)
        {
            Player = player;
            Player.OnCardAddedToHand += AddPlayerHandCardUI;
            PlayerCardThumbnails = new List<PlayerHandCardThumbnail>();
        }

        #region Virtual Functions

        /// <summary>
        /// Rebuilds the UI if needed
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            PlayerCardThumbnails.RemoveAll(x => x.IsAlive == false);
            if (NeedsRebuild)
            {
                Rebuild();
            }
        }

        #endregion

        #region UI rebuilding

        /// <summary>
        /// Rebuilds the spacing of the thumbnails
        /// </summary>
        private void Rebuild()
        {
            Debug.Assert(NeedsRebuild);

            int index = 0;
            foreach (PlayerHandCardThumbnail thumbnail in PlayerCardThumbnails)
            {
                thumbnail.UpdatePositions(new Vector2((thumbnail.Size.X + padding) * index + (thumbnail.Size.X - Size.X) * 0.5f + padding, 0));
                index++;
            }

            NeedsRebuild = false;
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

            PlayerCardThumbnails.Add(cardUI);
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

            Image infoImage = AddObject(new Image(new Vector2(screenDimensions.X * 0.5f, screenDimensions.Y * 0.5f), Vector2.Zero, cardTextureAsset), true, true);
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
