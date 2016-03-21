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
        private PlayerHandCardThumbnail CardThumbnail { get; set; }

        /// <summary>
        /// The card image UI we will be manipulating in this script
        /// </summary>
        private Image CardImage { get; set; }

        #endregion

        public PlaceCardScript(PlayerHandCardThumbnail cardThumbnail) :
            base()
        {
            DebugUtils.AssertNotNull(cardThumbnail.CardData);
            CardData = cardThumbnail.CardData;
            CardThumbnail = cardThumbnail;

            CardThumbnail.Hide();
        }

        #region Virtual Functions

        public override void LoadContent()
        {
            CheckShouldLoad();

            DebugUtils.AssertNotNull(ParentScreen);
            CardImage = ParentScreen.AddScreenUIObject(new Image(CardThumbnail.Size, Vector2.Zero, CardData.TextureAsset), true, true);
            CardImage.SetParent(GameMouse.Instance, true);

            base.LoadContent();
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
                if ((ParentScreen as BattleScreen).CurrentActivePlayer.CanLayCard(CardData))
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
        /// Converts the inputted card data into a game card and removes any UI we have that we no longer need.
        /// Also kills the script.
        /// </summary>
        /// <param name="cardData"></param>
        private void AddCardToGame()
        {
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            DebugUtils.AssertNotNull(battleScreen);

            // Set the size first so that when we add the object it will have the correct size.
            // Later on when we fix up the assets, we can remove this
            Card card = CardFactory.CreateCard(CardData);
            card.Size = CardImage.Size;
            card = battleScreen.GameBoard.PlayerGameBoardSection.AddObject(card, true, true);

            CardImage.Die();
            CardThumbnail.Die();
            Die();
        }

        /// <summary>
        /// Cancels the placement and sends the card back to the player's hand.
        /// Deals with any UI and kills the script.
        /// </summary>
        private void SendCardBackToHand()
        {
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            DebugUtils.AssertNotNull(battleScreen);

            CardThumbnail.Show();

            CardImage.Die();
            Die();
        }

        #endregion
    }
}
