using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// The screen where our main gameplay will take place between a player and an opponent
    /// </summary>
    public class BattleScreen : GameplayScreen
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our human playable character
        /// </summary>
        public static GamePlayer Player { get; private set; }

        /// <summary>
        /// A reference to the current player who's turn it is
        /// </summary>
        public GamePlayer CurrentActivePlayer { get; private set; }

        /// <summary>
        /// A reference to our HUD - handles all the UI side
        /// </summary>
        public BattleScreenHUD HUD { get; private set; }

        /// <summary>
        /// A reference to our GameBoard - handles all the actual Game Objects
        /// </summary>
        public GameBoard GameBoard { get; private set; }

        #endregion

        public BattleScreen(Deck playerChosenDeck, string screenDataAsset) :
            base(screenDataAsset)
        {
            Player = new GamePlayer(playerChosenDeck);
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our game board.
        /// </summary>
        protected override void AddInitialGameObjects()
        {
            base.AddInitialGameObjects();

            GameBoard = AddGameObject(new GameBoard(ScreenCentre));
        }

        /// <summary>
        /// Sets up our ambient light as white with full intensity for now
        /// </summary>
        protected override void AddInitialLights()
        {
            Lights.AddObject(new AmbientLight(Color.White));
        }

        /// <summary>
        /// Set up our game HUD.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            HUD = AddScreenUIObject(new BattleScreenHUD(AssetManager.DefaultEmptyPanelTextureAsset));
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Updates the current active player and triggers all the events for a new turn.
        /// </summary>
        public void NewPlayerTurn()
        {
            CurrentActivePlayer = Player;
            CurrentActivePlayer.NewTurn();
        }

        #endregion
    }
}