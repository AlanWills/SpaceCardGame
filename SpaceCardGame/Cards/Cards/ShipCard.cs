namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent a shield in our game.
    /// </summary>
    public class ShipCard : GameCard
    {
        public ShipCard(ShipCardData shipCardData) :
            base(shipCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Looks at how many ships we have placed already and the resource costs of the card we wish to lay
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanLay(GamePlayer player, ref string error)
        {
            // Return if we have no more room!
            if (player.CurrentShipsPlaced >= GamePlayer.MaxShipNumber)
            {
                error = "Maximum number of ships deployed";
                return false;
            }

            // Check resource requirements
            return base.CanLay(player, ref error);
        }

        /// <summary>
        /// The valid target for a ship card is only another ship card, so return whether the input is a CardShipPair with non-zero health.
        /// </summary>
        /// <param name="pairToValidate"></param>
        /// <returns></returns>
        public override bool CanUseOn(CardObjectPair pairToValidate)
        {
            if (pairToValidate is CardShipPair)
            {
                // If we are targeting another ship, it is valid if it not dead
                return !(pairToValidate as CardShipPair).Ship.DamageModule.Dead;
            }

            // Otherwise the target is invalid
            return false;
        }

        #endregion
    }
}
