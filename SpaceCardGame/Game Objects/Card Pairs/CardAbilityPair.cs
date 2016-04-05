using System;
using _2DEngine;
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

        public CardAbilityPair(AbilityCard abilityCard, Ability ability) :
            base(abilityCard, ability)
        {
            DebugUtils.AssertNotNull(abilityCard);
            DebugUtils.AssertNotNull(ability);

            AbilityCard = abilityCard;
            Ability = ability;
        }

        #region Virtual Functions

        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("TODO");
        }

        #endregion
    }
}
