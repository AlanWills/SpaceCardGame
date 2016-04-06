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

        public CardAbilityPair(AbilityCardData abilityCardData) :
            base(abilityCardData)
        {

        }

        #region Virtual Functions

        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("TODO");
        }

        #endregion
    }
}
