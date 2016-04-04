using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Ships.
    /// This is so we can have extra functions for adding shields/weapons etc.
    /// </summary>
    public class CardShipPair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a ShipCard (saves casting elsewhere)
        /// </summary>
        public ShipCard ShipCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a Ship (saves casting elsewhere)
        /// </summary>
        public Ship Ship { get; private set; }

        #endregion

        public CardShipPair(ShipCard shipCard, Ship ship) :
            base(shipCard, ship)
        {
            DebugUtils.AssertNotNull(shipCard);
            DebugUtils.AssertNotNull(ship);

            ShipCard = shipCard;
            Ship = ship;
        }

        #region Utility Functions

        // Do functions for adding shields, weapons
        // Will need to set up the objects on the ship, but also store the cards next to the ShipCard for reference

        #endregion
    }
}
