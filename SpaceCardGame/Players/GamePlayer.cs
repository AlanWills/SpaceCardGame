using CardGameEngine;
using CardGameEngineData;

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
        /// An event which will be fired when we begin a new turn
        /// </summary>
        public event NewTurnHandler OnNewTurn;

        public const int ResourceCardsCanLay = 2;

        #endregion

        public GamePlayer(Deck chosenDeck) :
            base(chosenDeck)
        {
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

        #region Card Utility Functions

        /// <summary>
        /// Analyses the cost of the card compared to our available resources to work out whether we have the resources to lay this
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns><c>true</c>We have enough resources available to lay this card.<c>false</c>We do not have enough resources to lay this card</returns>
        public bool CanLayCard(CardData cardData)
        {
            // Check to make sure we haven't laid 2 resource cards already
            if (cardData.Type == "Resource" && ResourceCardsPlacedThisTurn >= ResourceCardsCanLay)
            {
                return false;
            }

            for (int i = 0; i < (int)ResourceType.kNumResourceTypes; i++)
            {
                if (AvailableResources[i] < cardData.ResourceCosts[i])
                {
                    // We do not have enough of the current resource we are analysing to lay this card so return false
                    return false;
                }
            }

            // We have enough of each resource type for this card
            return true;
        }

        #endregion
    }
}
