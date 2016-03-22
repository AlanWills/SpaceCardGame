using CardGameEngine;

namespace SpaceCardGame
{
    public class GamePlayer : Player
    {
        public delegate void NewTurnHandler(GamePlayer newActivePlayer);

        #region Properties and Fields

        /// <summary>
        /// An int lookup over the number of resources we have left to use this turn.
        /// Driven by the UI on our PlayerGameBoardSection.
        /// </summary>
        public int[] AvailableResources { get; private set; }

        /// <summary>
        /// A cap to limit the number of resource cards we can lay
        /// </summary>
        public int ResourceCardsPlacedThisTurn { get; set; }

        /// <summary>
        /// A cap to limit the number of ships the player can have placed at any one time
        /// </summary>
        public int CurrentShipsPlaced { get; set; }

        /// <summary>
        /// An event which will be fired when we begin a new turn
        /// </summary>
        public event NewTurnHandler OnNewTurn;

        public const int ResourceCardsCanLay = 2;
        public const int MaxShipNumber = 8;

        #endregion

        public GamePlayer(Deck chosenDeck) :
            base(chosenDeck)
        {
            ResourceCardsPlacedThisTurn = 0;
            CurrentShipsPlaced = 0;

            AvailableResources = new int[(int)ResourceType.kNumResourceTypes];
            for (ResourceType resourceType = ResourceType.Crew; resourceType < ResourceType.kNumResourceTypes; resourceType++)
            {
                AvailableResources[(int)resourceType] = 0;
            }
        }

        #region Virtual Functions

        /// <summary>
        /// Calls our new turn event handler.
        /// Called after our CurrentActivePlayer is updated in our screen.
        /// </summary>
        public override void NewTurn()
        {
            base.NewTurn();

            ResourceCardsPlacedThisTurn = 0;

            if (OnNewTurn != null)
            {
                OnNewTurn(this);
            }
        }

        #endregion
    }
}
