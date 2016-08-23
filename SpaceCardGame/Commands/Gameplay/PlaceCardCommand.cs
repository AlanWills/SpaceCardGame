using CelesteEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A command which handles placing a card onto the game board.
    /// </summary>
    public class PlaceCardCommand : Command
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card we are placing
        /// </summary>
        private Card Card { get; set; }

        /// <summary>
        /// A reference to our batte screen
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        #endregion

        public PlaceCardCommand(Card cardThumbnail) :
            base()
        {
            Card = cardThumbnail;
            Card.ReparentTo(GameMouse.Instance);
            Card.HandAnimationModule.EnlargeOnHover = false;
        }

        #region Virtual Functions
        
        /// <summary>
        /// Hides the card thumbnail.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            BattleScreen = ParentScreen as BattleScreen;
            Card.LocalPosition = Vector2.Zero;
        }

        /// <summary>
        /// Handles input from the mouse - left clicking will place a new card into our game board.
        /// Right clicking will cancel the action and place it back into our hand.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (GameMouse.Instance.IsClicked(MouseButton.kLeftButton))
            {
                // Add the card to the game if we have selected a valid target for the card
                AddCardToGame();
            }
            else if (GameMouse.Instance.IsClicked(MouseButton.kRightButton))
            {
                SendCardBackToHand();

                BattleScreen.ActivePlayer.AlterResources(Card, ChargeType.kRefund);
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Adds the inputted card data to the game screen and removes any UI we have that we no longer need of.
        /// Also kills the command.
        /// </summary>
        private void AddCardToGame()
        {
            BattleScreen.Board.ActivePlayerBoardSection.GameBoardSection.AddCard(Card, GameMouse.Instance.InGameWorldPosition);
            Debug.Assert(!BattleScreen.ActivePlayer.CurrentHand.Exists(x => x == Card));

            Die();
        }

        /// <summary>
        /// Cancels the placement and sends the card back to the player's hand.
        /// Deals with any UI and kills the command.
        /// </summary>
        private void SendCardBackToHand()
        {
            BattleScreen.ActivePlayer.AddCardToHand(Card);

            Die();
        }

        #endregion
    }
}
