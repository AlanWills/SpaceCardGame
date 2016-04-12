using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Abilities.
    /// </summary>
    public class CardAbilityPair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a AbilityCard (saves casting elsewhere)
        /// </summary>
        public AbilityCard AbilityCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a Ability (saves casting elsewhere)
        /// </summary>
        public Ability Ability { get; private set; }

        #endregion

        public CardAbilityPair(AbilityCardData abilityCardData) :
            base(abilityCardData)
        {
            Ability = AddChild(new Ability(abilityCardData.ObjectDataAsset));
            CardObject = Ability;

            Debug.Assert(Card is AbilityCard);
            AbilityCard = Card as AbilityCard;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds an ability to our ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("Empty");
        }

        // Other virtual functions we do not currently need to implement

        #endregion
    }
}
