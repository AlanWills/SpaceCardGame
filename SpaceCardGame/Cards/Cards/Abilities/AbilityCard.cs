using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent an ability in our game.
    /// </summary>
    public abstract class AbilityCard : Card
    {
        public AbilityCard(Player player, CardData abilityCardData) :
            base(player, abilityCardData)
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
        /// Override this to perform the actual implementation for the ability
        /// </summary>
        /// <param name="target"></param>
        public virtual void UseAbility(CardObjectPair target) { }

        #endregion
    }
}
