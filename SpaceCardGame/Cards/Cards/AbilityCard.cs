namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent an ability in our game.
    /// </summary>
    public class AbilityCard : GameCard
    {
        public AbilityCard(AbilityCardData abilityCardData) :
            base(abilityCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Certain ability cards can only be used on certain items.
        /// By default though we return true.
        /// </summary>
        /// <param name="pairToValidate"></param>
        /// <returns></returns>
        public override bool CanUseOn(CardObjectPair pairToValidate)
        {
            return base.CanUseOn(pairToValidate);
        }

        #endregion
    }
}
