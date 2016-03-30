using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class to handle the UI objects in the game board for a player
    /// </summary>
    public class PlayerUIBoardSection : UIObjectContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the battle screen
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        /// <summary>
        /// A reference to the player which this UI section is for
        /// </summary>
        private GamePlayer Player { get; set; }

        /// <summary>
        /// A reference to the player's deck UI
        /// </summary>
        private PlayerDeckUI PlayerDeckUI { get; set; }

        /// <summary>
        /// A reference to the player's hand UI
        /// </summary>
        public PlayerHandUI PlayerHandUI { get; private set; }

        #endregion

        public PlayerUIBoardSection(GamePlayer player, Vector2 localPosition, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f), localPosition, textureAsset)
        {
            Player = player;

            PlayerDeckUI = AddObject(new PlayerDeckUI(Player, Vector2.Zero));
            PlayerHandUI = AddObject(new PlayerHandUI(Player, new Vector2(Size.X * 0.8f, Size.Y * 0.25f), new Vector2(0, Size.Y * 0.4f)));

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            BattleScreen.OnCardPlacementStateStarted += SetupUIForCardPlacement;
            BattleScreen.OnBattleStateStarted += SetupUIForBattle;
        }

        #region Virtual Functions

        /// <summary>
        /// Do some size fixup - when we resize cards this may not be necessary
        /// </summary>
        public override void Begin()
        {
            // Do this before we call base.Begin, because we need the new size before we call the begin function in there
            PlayerDeckUI.Size *= 0.5f;
            PlayerDeckUI.LocalPosition = (Size - PlayerDeckUI.Size) * 0.5f;

            base.Begin();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Changes the UI and objects in our game for the card placement phase.
        /// </summary>
        private void SetupUIForBattle(TurnState turnState)
        {
            PlayerDeckUI.Hide();
            PlayerHandUI.Hide();
        }

        /// <summary>
        /// Changes the UI and objects in our game for the battle phase.
        /// </summary>
        private void SetupUIForCardPlacement(TurnState turnState)
        {
            PlayerDeckUI.Show();
            PlayerHandUI.Show();

            CardFlipState flipState = Player == BattleScreen.ActivePlayer ? CardFlipState.kFaceUp : CardFlipState.kFaceDown;

            // Flip the cards in our hand face up if it is our turn
            foreach (BaseUICard card in PlayerHandUI)
            {
                card.Flip(flipState);
            }
        }

        #endregion
    }
}
