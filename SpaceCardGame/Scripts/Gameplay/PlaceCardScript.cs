using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A script which handles placing a card onto the game board.
    /// </summary>
    public class PlaceCardScript : Script
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card data
        /// </summary>
        private CardData CardData { get; set; }

        /// <summary>
        /// A reference to our card thumbnail
        /// </summary>
        private BaseUICard CardThumbnail { get; set; }

        /// <summary>
        /// A reference to the card we will be placing
        /// </summary>
        private GameCard Card { get; set; }

        #endregion

        public PlaceCardScript(BaseUICard cardThumbnail) :
            base()
        {
            DebugUtils.AssertNotNull(cardThumbnail.CardData);
            CardData = cardThumbnail.CardData;
            CardThumbnail = cardThumbnail;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up our card and sets it's parent to be the mouse
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            DebugUtils.AssertNotNull(ParentScreen);
            Card = CardFactory.CreateCard(CardData);

            // Set the size first so that when we add the object it will have the correct size.
            // Later on when we fix up the assets, we can remove this
            Card.Size = CardThumbnail.Size;

            ParentScreen.AddGameObject(Card, true, true);
            Card.SetParent(GameMouse.Instance, true);

            base.LoadContent();
        }

        /// <summary>
        /// Hides the card thumbnail.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // We do this here, because we add this script during the thumbnail's update script.
            // It will then continue to run it's handle input function and make the info image visible.
            // Since this is executed one frame later, this will successfully hide the UI.
            CardThumbnail.Hide();
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
                Debug.Assert(ParentScreen is BattleScreen);
                DebugUtils.AssertNotNull((ParentScreen as BattleScreen).CurrentActivePlayer);

                if (Card.CanLay((ParentScreen as BattleScreen).CurrentActivePlayer))
                {
                    AddCardToGame();
                }
                else
                {
                    SendCardBackToHand();
                }
            }
            else if (GameMouse.Instance.IsClicked(MouseButton.kRightButton))
            {
                SendCardBackToHand();
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Adds the inputted card to the game screen and removes any UI we have that we no longer need.
        /// Also kills the script.
        /// </summary>
        private void AddCardToGame()
        {
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            DebugUtils.AssertNotNull(battleScreen);

            battleScreen.GameBoard.PlayerGameBoardSection.AddObject(Card);

            CardThumbnail.Die();
            Die();
        }

        /// <summary>
        /// Cancels the placement and sends the card back to the player's hand.
        /// Deals with any UI and kills the script.
        /// </summary>
        private void SendCardBackToHand()
        {
            CardThumbnail.Show();

            Card.Die();
            Die();
        }

        #endregion
    }
}
