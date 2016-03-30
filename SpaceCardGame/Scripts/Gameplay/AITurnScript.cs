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
                OnPlaceCardsState();
            }
            else
            {
                OnBattleState();
            }
        }

        #endregion

        #region Turn State Functions

        /// <summary>
        /// Lay as many resource cards as possible and add appropriate ships/weapons etc.
        /// </summary>
        private void OnPlaceCardsState()
        {
            // Check to see if we have laid the resource cards we can this turn and we have resources in our hand we can lay
            if (Player.ResourceCardsPlacedThisTurn < GamePlayer.ResourceCardsCanLay && Player.CurrentHand.Exists(x => x.Type == "Resource"))
            {
                // Lay a resource card
                CardData resourceCardData = Player.CurrentHand.Find(x => x.Type == "Resource");
                GameCard card = CardFactory.CreateCard(resourceCardData);

                BaseUICard cardThumbnail = PlayerBoardSection.PlayerUIBoardSection.PlayerHandUI.FindCardThumbnail(resourceCardData);
                cardThumbnail.Die();

                card.Size = cardThumbnail.Size;

                PlayerBoardSection.PlayerGameBoardSection.AddObject(card);

                // TODO Can improve this by analysing the resource costs of the other cards and working out what cards would be best to lay
            }
            else
            {
                BattleScreen.ProgressTurnButton.ForceClick();
            }
        }

        /// <summary>
        /// Attack the opponent ships
        /// </summary>
        private void OnBattleState()
        {
            BattleScreen.ProgressTurnButton.ForceClick();
            Die();
        }

        #endregion
    }
}
