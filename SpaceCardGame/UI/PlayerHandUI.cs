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

        public PlayerHandUI(Player player, Vector2 size, Vector2 localPosition, string backgroundTextureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
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

            foreach (BaseUICard thumbnail in UIObjects)
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
        public override T AddObject<T>(T uiObjectToAdd, bool load = false, bool initialise = false)
        {
            Debug.Assert(uiObjectToAdd is BaseUICard);
            (uiObjectToAdd as BaseUICard).OffsetToHighlightedPosition = new Vector2(0, -100);

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
            BaseUICard cardUI = AddObject(new BaseUICard(drawnCard, new Vector2(Size.X * 0.1f, 1.2f * Size.Y), Vector2.Zero), true, true);

            cardUI.OnLeftClicked += AddCardToGame;
            cardUI.OnDeath += SyncPlayerHand;
            cardUI.OnDeath += RebuildCallback;
        }

        /// <summary>
        /// A callback which adds a card in our hand to the game
        /// </summary>
        /// <param name="clickable"></param>
        private void AddCardToGame(IClickable clickable)
        {
            Debug.Assert(clickable is BaseUICard);

            ScriptManager.Instance.AddObject(new PlaceCardScript(clickable as BaseUICard), true, true);
        }

        /// <summary>
        /// Removes the card data the dead card thumbnail UI represents from the player's hand.
        /// </summary>
        public void SyncPlayerHand(BaseUICard cardThumbnail)
        {
            Player.CurrentHand.Remove(cardThumbnail.CardData);
        }

        /// <summary>
        /// Indicates that we need to rebuild this UI
        /// </summary>
        public void RebuildCallback(BaseUICard cardThumbnail)
        {
            NeedsRebuild = true;
        }

        #endregion
    }
}
