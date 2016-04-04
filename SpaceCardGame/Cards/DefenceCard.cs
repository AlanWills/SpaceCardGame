using SpaceCardGameData;

namespace SpaceCardGame
{
    public class DefenceCard : GameCard
    {
        public DefenceCard(DefenceCardData defenceCardData) :
            base(defenceCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Defence cards can only be targetted on ships, so we just check that we have a CardShipPair which is not dead
        /// </summary>
        /// <param name="pairToValidate"></param>
        /// <returns></returns>
        public override bool IsValidTargetForInput(CardObjectPair pairToValidate)
        {
            if (pairToValidate is CardShipPair)
            {
                // If we are targeting a ship, it is valid if it not dead
                return !(pairToValidate as CardShipPair).Ship.Dead;
            }

            // Otherwise the target is invalid
            return false;
        }

        #endregion
    }
}
