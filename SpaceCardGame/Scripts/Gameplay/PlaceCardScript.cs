using _2DEngine;
using CardGameEngine;
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
        /// A reference to the UI we will use to represent our card as we place it
        /// </summary>
        private BaseUICard CardThumbnail { get; set; }

        /// <summary>
        /// A reference to our batte screen
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        #endregion

        public PlaceCardScript(BaseUICard cardThumbnail) :
            base()
        {
            DebugUtils.AssertNotNull(cardThumbnail.CardData);
            CardData = cardThumbnail.CardData;

            CardThumbnail = cardThumbnail;
            CardThumbnail.Reparent(GameMouse.Instance);
            CardThumbnail.EnlargeOnHover = false;
        }

        #region Virtual Functions
        
        /// <summary>
        /// Hides the card thumbnail.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            BattleScreen = ParentScreen as BattleScreen;
            CardThumbnail.LocalPosition = Vector2.Zero;
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
                    if (CardData.CanLay(BattleScreen.ActivePlayer, ref error))
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
        /// Adds the inputted card data to the game screen and removes any UI we have that we no longer need of.
        /// Also kills the script.
        /// </summary>
        private void AddCardToGame()
        {
            BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.AddCard(CardData, CardThumbnail.Size);

            CardThumbnail.Die();
            Die();
        }

        /// <summary>
        /// Cancels the placement and sends the card back to the player's hand.
        /// Deals with any UI and kills the script.
        /// </summary>
        private void SendCardBackToHand()
        {
            BattleScreen.ActivePlayer.AddCardToHand(CardData);

            CardThumbnail.Die();
            Die();
        }

        /// <summary>
        /// A function used on a card type basis to deterine whether we have a valid set up to place the card
        /// </summary>
        /// <returns></returns>
        private bool CheckValidTarget()
        {
            if (CardData is AbilityCardData)
            {
                Debug.Fail("TO DO");
            }
            else if (CardData is ShieldCardData)
            {
                return true;
            }
            else if (CardData is ResourceCardData)
            {
                return true;
            }
            else if (CardData is ShipCardData)
            {
                DebugUtils.AssertNotNull(BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl.Collider);
                return BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl.Collider.CheckIntersects(GameMouse.Instance.InGamePosition);
            }
            else if (CardData is WeaponCardData)
            {
                return true;
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
