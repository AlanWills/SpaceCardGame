using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A script which takes in a card and waits for player input to choose a valid target for that card from the friendly ships.
    /// Adds UI to indicate valid/invalid targets and current proposed target.
    /// Really a wrapper around the card's own IsValidTarget function.
    /// </summary>
    public class ChooseFriendlyShipScript : ChooseShipScript
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether we have selected a ship when this script dies
        /// </summary>
        private bool ShipChosen { get; set; }

        #endregion

        public ChooseFriendlyShipScript(CardObjectPair cardObjectPair) :
            base(cardObjectPair)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Creates a reference to the battle screen and sets up our targeting line
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            ContainerToLookThrough = BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl;
            SelectingLine.Colour = Color.Green;
        }

        /// <summary>
        /// Loops through our friendly ships and attempts to select one that passes the validity check for our card.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // We handle what happens if we have left clicked on our mouse
            if (GameMouse.Instance.IsClicked(MouseButton.kLeftButton))
            {
                if (Target != null)
                {
                    ShipChosen = true;
                    Die();
                }
            }
        }
        
        /// <summary>
        /// If we have chosen a target we add the card to the target.
        /// Otherwise we kill the card and add it back to the player's hand
        /// </summary>
        public override void Die()
        {
            base.Die();

            if (ShipChosen)
            {
                // We have clicked on the target we were hovering over so we are good to go
                // Add the card to the ship we have selected
                DebugUtils.AssertNotNull(Target);
                CardToChooseTargetFor.AddToCardShipPair(Target);
            }
            else
            {
                // The script has ended, but we have not chosen a ship
                // Therefore we must send the card back to our hand and refund the resources
                BattleScreen.ActivePlayer.AddCardToHand(CardToChooseTargetFor.Card.CardData);
                BattleScreen.Board.ActivePlayerBoardSection.PlayerGameBoardSection.RefundCardResources(CardToChooseTargetFor.Card.CardData);

                CardToChooseTargetFor.Die();
            }
        }

        #endregion
    }
}
