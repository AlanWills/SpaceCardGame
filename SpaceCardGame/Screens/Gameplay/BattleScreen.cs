using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    public enum TurnState
    {
        kPlaceCards,
        kBattle,
    }

    public delegate void TurnStateChangeHandler();

    /// <summary>
    /// The screen where our main gameplay will take place between a player and an opponent
    /// </summary>
    public class BattleScreen : GameplayScreen
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our human playable character
        /// </summary>
        public static Player Player { get; private set; }

        /// <summary>
        /// A reference to our opponent
        /// </summary>
        public static Player Opponent { get; private set; }

        /// <summary>
        /// A reference to the current player who's turn it is
        /// </summary>
        public Player ActivePlayer { get; private set; }

        /// <summary>
        /// A reference to the non active player
        /// </summary>
        public Player NonActivePlayer { get; private set; }

        /// <summary>
        /// A reference to our GameBoard - handles all the actual Game Objects
        /// </summary>
        public Board Board { get; private set; }

        /// <summary>
        /// A button which will progress to the next turn state
        /// </summary>
        public ProgressTurnButton ProgressTurnButton { get; private set; }

        /// <summary>
        /// Events for when the state of the turn changes, called right at the end of state change after player has been updated etc.
        /// </summary>
        public event TurnStateChangeHandler OnCardPlacementStateStarted;
        public event TurnStateChangeHandler OnBattleStateStarted;

        /// <summary>
        /// An event that is triggered as we change state to kPlaceCards, but called before OnCardPlacementStateStarted;
        /// </summary>
        public event TurnStateChangeHandler OnTurnEnd;

        /// <summary>
        /// A variable to indicate what state we are in the current turn
        /// </summary>
        public TurnState TurnState { get; private set; }

        #endregion

        public BattleScreen(Deck playerChosenDeck, Deck opponentChosenDeck, string screenDataAsset) :
            base(screenDataAsset)
        {
            Player = new Player(playerChosenDeck);
            Opponent = new Player(opponentChosenDeck);
        }

        #region Virtual Functions

        /// <summary>
        /// Add our custom escape dialog for this specific game.
        /// </summary>
        /// <returns></returns>
        protected override InGameEscapeDialog AddInGameEscapeDialog()
        {
            return new CustomEscapeDialog();
        }

        /// <summary>
        /// Sets up our ambient light as white with full intensity for now
        /// </summary>
        protected override void AddInitialLights()
        {
            Lights.AddChild(new AmbientLight(Color.White));
        }

        /// <summary>
        /// Set up our game HUD.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            ProgressTurnButton = AddScreenUIObject(new ProgressTurnButton(Vector2.Zero));
            ProgressTurnButton.ClickableModule.OnLeftClicked += OnProgressTurnButtonLeftClicked;
            ProgressTurnButton.Hide();
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
        /// Add our new game command.
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            CommandManager.Instance.AddChild(new NewGameCommand(), true, true);
        }

        /// <summary>
        /// Set up our game board.
        /// We do this here (BAD) because we need to make sure that the current screen is the battle screen
        /// </summary>
        public override void Begin()
        {
            Board = AddGameObject(new Board(ScreenCentre), true, true);         // TODO rethink this

            base.Begin();

            Board.PlayerBoardSection.GameBoardSection.ShipCardControl.FindChild<CardStationPair>().OnStationDestroyed += OnPlayerDefeated;
            Board.OpponentBoardSection.GameBoardSection.ShipCardControl.FindChild<CardStationPair>().OnStationDestroyed += OnOpponentDefeated;

            // Set the current active player to be the opponent, so that when we call NewPlayerTurn at the end of the new game command, we begin the game for the player
            ActivePlayer = Opponent;
            TurnState = TurnState.kBattle;
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// A callback to progress the current state of the game.
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnProgressTurnButtonLeftClicked(BaseObject baseObject)
        {
            // Change the state of the turn and call any appropriate functions
            switch (TurnState)
            {
                // We are currently in the placing cards state
                case TurnState.kPlaceCards:
                    {
                        // Change to the battle state
                        TurnState = TurnState.kBattle;
                        OnBattleStateStarted?.Invoke();

                        break;
                    }

                // We are currently in the battle state
                case TurnState.kBattle:
                    {
                        // Change to the place cards state and change the turns
                        TurnState = TurnState.kPlaceCards;

                        OnTurnEnd?.Invoke();

                        NewPlayerTurn();

                        OnCardPlacementStateStarted?.Invoke();

                        break;
                    }

                default:
                    {
                        Debug.Fail("");
                        break;
                    }
            }
        }

        /// <summary>
        /// Adds the defeat menu UI to the user when they have been beaten
        /// </summary>
        private void OnPlayerDefeated()
        {
            Board.ShouldHandleInput.Value = false;
            Board.ShouldUpdate.Value = false;

            AddScreenUIObject(new PlayerDefeatedUI(), true, true);
        }

        protected virtual void OnOpponentDefeated() { }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Updates the current active player and triggers all the events for a new turn.
        /// </summary>
        public void NewPlayerTurn()
        {
            if (ActivePlayer == Player)
            {
                ActivePlayer = Opponent;
                NonActivePlayer = Player;

                CommandManager.Instance.AddChild(new AITurnCommand(ActivePlayer, Board.ActivePlayerBoardSection), true, true);
            }
            else
            {
                ActivePlayer = Player;
                NonActivePlayer = Opponent;
            }

            // Update the appropriate board sections so that we cannot interact unless we are the current active player
            //Board.NonActivePlayerBoardSection.ShouldHandleInput.Value = false;
            //Board.ActivePlayerBoardSection.ShouldHandleInput.Value = true;

            ActivePlayer.NewTurn();
        }

        #endregion
    }
}