using CardGameEngine;
using System;

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

        public const int ResourceCardsCanLay = 10;
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

        #region Utility Functions

        /// <summary>
        /// A function which works out whether we have enough resources to lay the inputted card data
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HaveSufficientResources(CardData cardData)
        {
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

        /// <summary>
        /// A function which works out whether we have enough resources to lay the inputted card data.
        /// Outputs an error message based on what resource was lacking.
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HasSufficientResources(CardData cardData, ref string error)
        {
            for (int i = 0; i < (int)ResourceType.kNumResourceTypes; i++)
            {
                if (AvailableResources[i] < cardData.ResourceCosts[i])
                {
                    // We do not have enough of the current resource we are analysing to lay this card so return false
                    error = "Insufficient " + Enum.GetNames(typeof(ResourceType))[i];
                    return false;
                }
            }

            // We have enough of each resource type for this card
            return true;
        }

        #endregion
    }
}
