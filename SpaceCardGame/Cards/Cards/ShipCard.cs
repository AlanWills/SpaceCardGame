using _2DEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent a shield in our game.
    /// </summary>
    public class ShipCard : GameCard
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card data for this ShipCard as ShipCardData.
        /// Only initialised if used.
        /// </summary>
        /*private ShipCardData shipCardData;
        public ShipCardData ShipCardData
        {
            get
            {
                DebugUtils.AssertNotNull(CardData);
                if (shipCardData == null)
                {
                    Debug.Assert(CardData is ShipCardData);
                    shipCardData = CardData as ShipCardData;
                }

                return shipCardData;
            }
        }*/

        /// <summary>
        /// A reference to our parent as a CardShipPair
        /// </summary>
        private CardShipPair cardShipPair;
        protected CardShipPair CardShipPair
        {
            get
            {
                if (cardShipPair == null)
                {
                    DebugUtils.AssertNotNull(Parent);
                    Debug.Assert(Parent is CardShipPair);
                    cardShipPair = Parent as CardShipPair;
                }

                return cardShipPair;
            }
        }

        #endregion

        public ShipCard(ShipCardData shipCardData) :
            base(shipCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Set up the damage calculation event for our ship.
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            Debug.Assert(CardPair is CardShipPair);
            CardShipPair shipCardPair = CardPair as CardShipPair;
            shipCardPair.Ship.DamageModule.CalculateDamage += CalculateDamage;

            base.Initialise();
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

        /// <summary>
        /// A straight pass through function for calculating damage.
        /// Override this to perform custom damage calculation for ships.
        /// </summary>
        /// <param name="objectDoingTheDamage"></param>
        /// <param name="inputDamage"></param>
        protected virtual float CalculateDamage(BaseObject objectDoingTheDamage, float inputDamage)
        {
            return inputDamage;
        }

        #endregion
    }
}
