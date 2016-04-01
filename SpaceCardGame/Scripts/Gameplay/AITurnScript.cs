using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A script which runs decision algorithms for an AI player
    /// </summary>
    public class AITurnScript : Script
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the AI player we will be controlling in this script
        /// </summary>
        private GamePlayer Player { get; set; }
        
        /// <summary>
        /// A reference to the board section we will be controlling in this script
        /// </summary>
        private PlayerBoardSection PlayerBoardSection { get; set; }

        /// <summary>
        /// A reference to our battle screen
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        private float timeBetweenCardLays = 1;
        private float currentTimeBetweenCardLays = 0;

        #endregion

        public AITurnScript(GamePlayer player, PlayerBoardSection playerBoardSection) :
            base()
        {
            Player = player;
            PlayerBoardSection = playerBoardSection;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up the reference to our battle screen
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ParentScreen is BattleScreen);
            BattleScreen = ParentScreen as BattleScreen;
        }

        /// <summary>
        /// Updates our player's state based on the turn state and various other factors
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (BattleScreen.TurnState == TurnState.kPlaceCards)
            {
                // Updates our AI player when in the card placement state
                OnPlaceCardsState(elapsedGameTime);
            }
            else
            {
                OnBattleState(elapsedGameTime);
            }
        }

        #endregion

        #region Turn State Functions

        /// <summary>
        /// Lay as many resource cards as possible and add appropriate ships/weapons etc.
        /// </summary>
        private void OnPlaceCardsState(float elapsedGameTime)
        {
            currentTimeBetweenCardLays += elapsedGameTime;

            // Check to see if we have laid the resource cards we can this turn and we have resources in our hand we can lay
            if (Player.ResourceCardsPlacedThisTurn < GamePlayer.ResourceCardsCanLay && Player.CurrentHand.Exists(x => x.Type == "Resource"))
            {
                // Lay a resource card
                CardData resourceCardData = Player.CurrentHand.Find(x => x.Type == "Resource");

                if (currentTimeBetweenCardLays >= timeBetweenCardLays)
                {
                    // TODO Can improve this by analysing the resource costs of the other cards and working out what cards would be best to lay
                    LayCard(resourceCardData);
                }

                return;
            }
            // Check to see if we have laid the ships we can and we have ships in our hand we can lay
            else if (Player.CurrentShipsPlaced < GamePlayer.MaxShipNumber && Player.CurrentHand.Exists(x => x.Type == "Ship" && Player.HaveSufficientResources(x)))
            {
                // Lay a ship card
                CardData shipCardData = Player.CurrentHand.Find(x => x.Type == "Ship" && Player.HaveSufficientResources(x));

                if (currentTimeBetweenCardLays >= timeBetweenCardLays)
                {
                    LayCard(shipCardData);
                }

                return;
            }

            currentTimeBetweenCardLays = 0;
            BattleScreen.ProgressTurnButton.ForceClick();
        }

        /// <summary>
        /// Attack the opponent ships
        /// </summary>
        private void OnBattleState(float elapsedGameTime)
        {
            BattleScreen.ProgressTurnButton.ForceClick();
            Die();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Creates and adds a card using the inputted card data and resets the timer so we have spacing between adding cards.
        /// </summary>
        /// <param name="cardData"></param>
        private void LayCard(CardData cardData)
        {
            Debug.Assert(currentTimeBetweenCardLays >= timeBetweenCardLays);
            GameCard card = CardFactory.CreateCard(cardData);

            BaseUICard cardThumbnail = PlayerBoardSection.PlayerUIBoardSection.PlayerHandUI.FindCardThumbnail(cardData);
            cardThumbnail.Die();

            // Set the position of the card so that when we add it to the game board section it will be added to a slot
            card.LocalPosition = PlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl.WorldPosition + PlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl.GetEmptySlot();
            card.Size = cardThumbnail.Size;

            PlayerBoardSection.PlayerGameBoardSection.AddObject(card);

            currentTimeBetweenCardLays = 0;
        }

        #endregion
    }
}
