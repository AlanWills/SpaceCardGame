using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    public enum TurnState
    {
        kPlaceCards,
        kBattle,
    }

    public delegate void TurnStateChangeHandler(TurnState newTurnState);

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
        /// A reference to our opponent
        /// </summary>
        public static GamePlayer Opponent { get; private set; }

        /// <summary>
        /// A reference to the current player who's turn it is
        /// </summary>
        public GamePlayer ActivePlayer { get; private set; }

        /// <summary>
        /// A reference to the non active player
        /// </summary>
        public GamePlayer NonActivePlayer { get; private set; }

        /// <summary>
        /// A reference to our GameBoard - handles all the actual Game Objects
        /// </summary>
        public Board Board { get; private set; }

        /// <summary>
        /// A button which will progress to the next turn state
        /// </summary>
        public Button ProgressTurnButton { get; private set; }

        /// <summary>
        /// Events for when the state of the turn changes, called right at the end of state change after player has been updated etc.
        /// </summary>
        public event TurnStateChangeHandler OnCardPlacementStateStarted;
        public event TurnStateChangeHandler OnBattleStateStarted;

        /// <summary>
        /// A variable to indicate what state we are in the current turn
        /// </summary>
        public TurnState TurnState { get; private set; }

        #endregion

        public BattleScreen(Deck playerChosenDeck, string screenDataAsset) :
            base(screenDataAsset)
        {
            Player = new GamePlayer(playerChosenDeck);
            Opponent = new GamePlayer(playerChosenDeck);
        }

        #region Virtual Functions

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

            ProgressTurnButton = AddScreenUIObject(new Button(GetTurnStateButtonText(), Vector2.Zero));
            ProgressTurnButton.OnLeftClicked += OnProgressTurnButtonLeftClicked;
        }

        /// <summary>
        /// Fixes up some UI
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            ProgressTurnButton.LocalPosition = new Vector2(ScreenDimensions.X - ProgressTurnButton.Size.X * 0.5f, ScreenCentre.Y);
        }

        /// <summary>
        /// Set up our game board.
        /// We do this here (BAD) because we need to make sure that the current screen is the battle screen
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Board = AddGameObject(new Board(ScreenCentre), true, true);         // TODO rethink this

            // Set the current active player to be the opponent, so that when we call NewPlayerTurn at the end of the script, we begin the game for the player
            ActivePlayer = Opponent;
            TurnState = TurnState.kBattle;
            ScriptManager.Instance.AddObject(new NewGameScript(), true, true);
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// A callback to progress the current state of the game.
        /// </summary>
        /// <param name="clickable"></param>
        private void OnProgressTurnButtonLeftClicked(IClickable clickable)
        {
            // Change the state of the turn and call any appropriate functions
            switch (TurnState)
            {
                case TurnState.kPlaceCards:
                    {
                        TurnState = TurnState.kBattle;
                        if (OnBattleStateStarted != null)
                        {
                            OnBattleStateStarted(TurnState);
                        }

                        break;
                    }

                case TurnState.kBattle:
                    {
                        TurnState = TurnState.kPlaceCards;
                        NewPlayerTurn();

                        if (OnCardPlacementStateStarted != null)
                        {
                            OnCardPlacementStateStarted(TurnState);
                        }

                        break;
                    }

                default:
                    {
                        Debug.Fail("");
                        break;
                    }
            }

            // Update the button text
            ProgressTurnButton.Label.Text = GetTurnStateButtonText();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Gets the appropriate text for the turn state button based on the current turn state
        /// </summary>
        /// <returns></returns>
        private string GetTurnStateButtonText()
        {
            switch (TurnState)
            {
                case TurnState.kPlaceCards:
                    return "Start Battle";

                case TurnState.kBattle:
                    return "End Turn";

                default:
                    Debug.Fail("");
                    return "";
            }
        }

        /// <summary>
        /// Updates the current active player and triggers all the events for a new turn.
        /// </summary>
        public void NewPlayerTurn()
        {
            if (ActivePlayer == Player)
            {
                ActivePlayer = Opponent;
                NonActivePlayer = Player;

                ScriptManager.Instance.AddObject(new AITurnScript(ActivePlayer, Board.CurrentActivePlayerBoardSection), true, true);
            }
            else
            {
                ActivePlayer = Player;
                NonActivePlayer = Opponent;
            }
            
            ActivePlayer.NewTurn();
        }

        #endregion
    }
}