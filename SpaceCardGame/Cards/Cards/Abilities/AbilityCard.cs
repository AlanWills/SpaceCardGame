namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent an ability in our game.
    /// </summary>
    public abstract class AbilityCard : Card
    {
        public AbilityCard(CardData abilityCardData) :
            base(abilityCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Abilities create a CardAbilityPair
        /// </summary>
        /// <returns></returns>
        public override CardObjectPair CreateCardObjectPair()
        {
            return new CardAbilityPair(this);
        }

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

        /// <summary>
        /// Override this to perform the actual implementation for the ability
        /// </summary>
        /// <param name="target"></param>
        public virtual void UseAbility(CardObjectPair target) { }

        #endregion
    }
}
