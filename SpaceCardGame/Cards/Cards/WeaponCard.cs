namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent a weapon in our game.
    /// </summary>
    public class WeaponCard : GameCard
    {
        public WeaponCard(WeaponCardData weaponCardData) :
            base(weaponCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Defence cards can only be targetted on ships, so we just check that we have a CardShipPair which is not dead
        /// </summary>
        /// <param name="pairToValidate"></param>
        /// <returns></returns>
        public override bool CanUseOn(CardObjectPair pairToValidate)
        {
            if (pairToValidate is CardShipPair)
            {
                // If we are targeting a ship, it is valid if it not dead
                return !(pairToValidate as CardShipPair).Ship.DamageModule.Dead;
            }

            // Otherwise the target is invalid
            return false;
        }

        #endregion
    }
}
