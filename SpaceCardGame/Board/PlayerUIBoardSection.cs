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
        public PlayerDeckUI PlayerDeckUI { get; private set; }

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
        /// Fixup the position the Deck
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Do this before we call base.Begin, because we need the new size before we call the begin function in there
            // In the Begin function for the board section we change this local position, so we must set it here
            PlayerDeckUI.Size *= 0.5f;

            Vector2 offset = new Vector2(MathHelper.Max(PlayerDeckUI.Size.X, PlayerDeckUI.DeckCountLabel.Size.X) + 10, PlayerDeckUI.Size.Y + 25);
            PlayerDeckUI.LocalPosition = (Size - offset) * 0.5f;
            PlayerDeckUI.DeckCountLabel.LocalPosition = new Vector2(0, -(PlayerDeckUI.Size.Y + PlayerDeckUI.DeckCountLabel.Size.Y) * 0.5f - 10);  // Can't do this in DeckUI because of fixup which occurs elsewhere
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
