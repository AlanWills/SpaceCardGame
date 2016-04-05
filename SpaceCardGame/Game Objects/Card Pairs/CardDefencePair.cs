using System;
using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Defence objects.
    /// </summary>
    public class CardDefencePair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a DefenceCard (saves casting elsewhere)
        /// </summary>
        public DefenceCard DefenceCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a ShipAddOn object (saves casting elsewhere)
        /// </summary>
        public ShipAddOn DefenceObject { get; private set; }

        #endregion

        public CardDefencePair(DefenceCard defenceCard, ShipAddOn shipAddOn) :
            base(defenceCard, shipAddOn)
        {
            DebugUtils.AssertNotNull(defenceCard);
            DebugUtils.AssertNotNull(shipAddOn);

            DefenceCard = defenceCard;
            DefenceObject = shipAddOn;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a defence object to a ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            
        }

        #endregion
    }
}
