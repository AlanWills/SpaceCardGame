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
        /// A reference to the card we will be placing
        /// </summary>
        private GameCard Card { get; set; }

        /// <summary>
        /// A reference to our batte screen
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        private Vector2 cardSize;

        #endregion

        public PlaceCardScript(BaseUICard cardThumbnail) :
            base()
        {
            DebugUtils.AssertNotNull(cardThumbnail.CardData);
            CardData = cardThumbnail.CardData;

            cardSize = cardThumbnail.Size;
            cardThumbnail.Die();
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

            Card.Size = cardSize;

            GameMouse.Instance.AddChild(Card);

            base.LoadContent();
        }

        /// <summary>
        /// Hides the card thumbnail.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            BattleScreen = ParentScreen as BattleScreen;
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
                if (CheckValidTarget())
                {
                    string error = "";
                    if (Card.CanLay(BattleScreen.ActivePlayer, ref error))
                    {
                        AddCardToGame();
                    }
                    else
                    {
                        SendCardBackToHand();
                        ScriptManager.Instance.AddChild(new FlashingTextScript(error, ScreenManager.Instance.ScreenCentre, Color.White, 2), true, true);
                    }
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
            Card.Reparent(BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection);

            Die();
        }

        /// <summary>
        /// Cancels the placement and sends the card back to the player's hand.
        /// Deals with any UI and kills the script.
        /// </summary>
        private void SendCardBackToHand()
        {
            BattleScreen.ActivePlayer.AddCardToHand(CardData);

            Card.Die();
            Die();
        }

        /// <summary>
        /// A function used on a card type basis to deterine whether we have a valid set up to place the card
        /// </summary>
        /// <returns></returns>
        private bool CheckValidTarget()
        {
            if (Card is AbilityCard)
            {

            }
            else if (Card is DefenceCard)
            {
                return true;
            }
            else if (Card is ResourceCard)
            {
                return true;
            }
            else if (Card is ShipCard)
            {
                DebugUtils.AssertNotNull(BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl.Collider);
                return BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl.Collider.CheckIntersects(GameMouse.Instance.InGamePosition);
            }
            else if (Card is WeaponCard)
            {

            }
            else
            {
                Debug.Fail("Card mismatch in place card script");
            }

            return false;
        }

        #endregion
    }
}
