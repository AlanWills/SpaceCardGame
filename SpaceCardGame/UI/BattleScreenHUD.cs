using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    public class BattleScreenHUD : UIObjectContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our player
        /// </summary>
        private Player Player { get; set; }

        private float padding = 35;

        #endregion

        public BattleScreenHUD(Player player, string hudTextureAsset) :
            base(ScreenManager.Instance.ScreenDimensions, ScreenManager.Instance.ScreenCentre, hudTextureAsset)
        {
            Player = player;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up callbacks for when we draw a card
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            Player.OnCardDraw += AddPlayerHandCardUI;

            base.LoadContent();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// A callback every time we draw a card.
        /// Adds appropriate UI for the newly drawn card.
        /// </summary>
        /// <param name="drawnCard"></param>
        private void AddPlayerHandCardUI(CardData drawnCard)
        {
            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;
            Vector2 size = new Vector2(screenDimensions.X * 0.1f, screenDimensions.Y * 0.15f);

            PlayerHandCardUI cardUI = AddObject(new PlayerHandCardUI(drawnCard, size, Vector2.Zero), true, true);
            size = cardUI.Size;
            cardUI.LocalPosition = new Vector2((size.X + padding) * (Player.CurrentHand.Count - 1) + (size.X - screenDimensions.X) * 0.5f + padding, (screenDimensions.Y - size.Y) * 0.5f - padding);

            cardUI.OnLeftClicked += AddCardToGame;
        }

        #endregion

        #region Click Callbacks

        private void AddCardToGame(IClickable clickable)
        {

        }

        #endregion
    }
}