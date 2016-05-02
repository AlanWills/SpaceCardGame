using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A command which takes in a card and waits for player input to choose a valid target for that card from the friendly ships.
    /// Adds UI to indicate valid/invalid targets and current proposed target.
    /// Really a wrapper around the card's own IsValidTarget function.
    /// </summary>
    public class ChooseFriendlyShipCommand : ChooseShipCommand
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether we have selected a ship when this command dies
        /// </summary>
        private bool ShipChosen { get; set; }

        #endregion

        public ChooseFriendlyShipCommand(GameCard cardObjectPair) :
            base(cardObjectPair)
        {
            ValidTargetFunction += ValidIfCanUseOn;
        }

        #region Virtual Functions

        /// <summary>
        /// Creates a reference to the battle screen and sets up our targeting line
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            ContainerToLookThrough = BattleScreen.Board.ActivePlayerBoardSection.GameBoardSection.ShipCardControl;
            SelectingLine.Colour.Value = Color.Green;
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
                (CardToChooseTargetFor.Parent as CardObjectPair).AddToCardShipPair(Target);
            }
            else
            {
                // The command has ended, but we have not chosen a ship
                // Therefore we must send the card back to our hand and refund the resources
                BattleScreen.ActivePlayer.AddCardToHand(CardToChooseTargetFor.CardData);

                bool refunding = false;
                BattleScreen.ActivePlayer.AlterResources(CardToChooseTargetFor.CardData, refunding);

                CardToChooseTargetFor.Die();
            }
        }

        #endregion

        #region Validity Callbacks

        /// <summary>
        /// A function which returns whether the card we are looking to use can be used on the current target
        /// </summary>
        /// <param name="cardToChooseTargetFor"></param>
        /// <param name="currentTarget"></param>
        /// <returns></returns>
        private bool ValidIfCanUseOn(GameCard cardToChooseTargetFor, CardShipPair currentTarget)
        {
            return cardToChooseTargetFor.CanUseOn(currentTarget);
        }

        #endregion
    }
}
